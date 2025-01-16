using CMS;
using CMS.Core;
using CMS.DataEngine;
using CMS.Helpers;

using System.Data;

using XperienceCommunity.DatabaseAnonymizer.Services;

[assembly: RegisterImplementation(typeof(IAnonymizerService), typeof(AnonymizerService))]
namespace XperienceCommunity.DatabaseAnonymizer.Services
{
    public class AnonymizerService(
        IEventLogService eventLogService,
        IAnonymizationTableProvider anonymizationTableProvider,
        IAppSettingsService appSettingsService) : IAnonymizerService
    {
        private byte[]? mKey;
        private string? mSalt;
        private string? mEnabled;
        private const int BATCH_SIZE = 500;
        private const string SALT_KEYNAME = "XperienceCommunityAnonymizationSalt";
        private const string ANONYMIZE_ENABLED_KEYNAME = "XperienceCommunityEnableAnonymization";
        private readonly IEventLogService eventLogService = eventLogService;
        private readonly IAppSettingsService appSettingsService = appSettingsService;
        private readonly IAnonymizationTableProvider anonymizationTableProvider = anonymizationTableProvider;


        public const string ISANONYMIZED_SETTINGNAME = "XperienceCommunityDatabaseAnonymized";


        private byte[] Key => mKey ??= SimpleEncryptionHelper.CreateKey(Salt);


        private string Salt => mSalt ??= appSettingsService[SALT_KEYNAME];


        private string AnonymizeEnabled => mEnabled ??= appSettingsService[ANONYMIZE_ENABLED_KEYNAME];


        public void Run()
        {
            // If anonymize mode or salt is not present, do nothing
            if (string.IsNullOrEmpty(AnonymizeEnabled) || string.IsNullOrEmpty(Salt))
            {
                return;
            }

            bool doAnonymize = ValidationHelper.GetBoolean(AnonymizeEnabled, false);
            bool dbIsAnonymized = SettingsKeyInfoProvider.GetBoolValue(ISANONYMIZED_SETTINGNAME);
            if (doAnonymize && !dbIsAnonymized)
            {
                RunInternal(true);
            }
            else if (!doAnonymize && dbIsAnonymized)
            {
                RunInternal(false);
            }
        }


        private void RunInternal(bool anonymize)
        {
            try
            {
                var tablesToUpdate = anonymizationTableProvider.GetTables();
                foreach (string table in tablesToUpdate)
                {
                    ModifyTable(table, anonymize);
                }

                SetDatabaseIsAnonymized(anonymize);
            }
            catch (Exception ex)
            {
                eventLogService.LogException(nameof(AnonymizerService), nameof(RunInternal), ex);
            }
        }


        private void ModifyTable(string table, bool anonymize)
        {
            var tm = new CMS.DataProviderSQL.TableManager();
            if (!tm.TableExists(table))
            {
                return;
            }

            var columnsToUpdate = anonymizationTableProvider.GetColumns(table);
            if (!columnsToUpdate.Any())
            {
                return;
            }

            var identityColumns = tm.GetPrimaryKeyColumns(table);
            if (!identityColumns.Any())
            {
                return;
            }

            int currentPage = 0;
            IEnumerable<DataRow> rows;
            do
            {
                rows = GetPagedResult(table, columnsToUpdate.Union(identityColumns), identityColumns[0], currentPage);
                var updateStatements = rows.Select(r => GetUpdateRowStatement(r, table, columnsToUpdate, identityColumns, anonymize));
                if (updateStatements.Any())
                {
                    string query = string.Join(Environment.NewLine, updateStatements);
                    ConnectionHelper.ExecuteNonQuery(query, null, QueryTypeEnum.SQLQuery);
                }

                currentPage++;
            }
            while (rows.Any());
        }


        private string? AnonymizeValue(object value, bool anonymize)
        {
            string stringRepresentation = ValidationHelper.GetString(value, string.Empty);
            if (string.IsNullOrEmpty(stringRepresentation))
            {
                return null;
            }

            try
            {
                // Decrypt() can throw an exception if the column value was not previously encrypted
                // In that case, leave the column as-is
                return anonymize ? SimpleEncryptionHelper.Encrypt(stringRepresentation, Key) :
                    SimpleEncryptionHelper.Decrypt(stringRepresentation, Key);
            }
            catch
            {
                return stringRepresentation;
            }
        }


        /// <summary>
        /// Gets paged records from a database table.
        /// </summary>
        /// <param name="table">The table name.</param>
        /// <param name="columns">The columns to retrieve in the SELECT statement.</param>
        /// <param name="orderColumn">The column to use in the ORDER BY clause (required for paging).</param>
        /// <param name="currentPage">The page to retrieve.</param>
        private static IEnumerable<DataRow> GetPagedResult(string table, IEnumerable<string> columns, string orderColumn, int currentPage)
        {
            int offset = currentPage * BATCH_SIZE;
            string queryText = $"SELECT {string.Join(", ", columns)} FROM {table} ORDER BY {orderColumn} OFFSET {offset} ROWS FETCH NEXT {BATCH_SIZE} ROWS ONLY";
            var result = ConnectionHelper.ExecuteQuery(queryText, null, QueryTypeEnum.SQLQuery);
            if (result.Tables.Count == 0)
            {
                return [];
            }

            return result.Tables[0].Rows.OfType<DataRow>();
        }


        /// <summary>
        /// Gets a SQL UPDATE statement used to anonymize or deanonymize the columns of a record.
        /// </summary>
        /// <param name="row">The record to update.</param>
        /// <param name="table">The name of the table containing the record.</param>
        /// <param name="columnsToUpdate">The columns of the record that should be updated.</param>
        /// <param name="identityColumns">The identity columns of the table.</param>
        /// <param name="anonymize">If <c>true</c>, the record data should be anonymized. Otherwise, it is deanonymized.</param>
        private string GetUpdateRowStatement(
            DataRow row,
            string table,
            IEnumerable<string> columnsToUpdate,
            IEnumerable<string> identityColumns,
            bool anonymize)
        {
            var values = new List<string>();
            foreach (string column in columnsToUpdate)
            {
                object currentValue = row[column];
                if (currentValue is null)
                {
                    continue;
                }

                string? newValue = AnonymizeValue(currentValue, anonymize);
                if (newValue is null)
                {
                    continue;
                }

                values.Add($"{column} = '{newValue}'");
            }
            var where = identityColumns.Select(col => $"{col} = {row[col]}");

            return $"UPDATE {table} SET {string.Join(",", values)} WHERE {string.Join(" AND ", where)}";
        }


        private void SetDatabaseIsAnonymized(bool isAnonymized)
        {
            eventLogService.LogInformation(nameof(AnonymizerService), isAnonymized ? "ANONYMIZED" : "DEANONYMIZED");
            SettingsKeyInfoProvider.SetGlobalValue(ISANONYMIZED_SETTINGNAME, isAnonymized);
        }
    }
}
