using System.Data;
using System.Security.Cryptography;
using System.Text;

using CMS;
using CMS.DataEngine;
using CMS.Helpers;

using XperienceCommunity.DatabaseAnonymizer.Models;
using XperienceCommunity.DatabaseAnonymizer.Services;

using TableManager = CMS.DataProviderSQL.TableManager;

[assembly: RegisterImplementation(typeof(IAnonymizerService), typeof(AnonymizerService))]
namespace XperienceCommunity.DatabaseAnonymizer.Services
{
    internal class AnonymizerService(IAnonymizationLogger anonmyzationLogger) : IAnonymizerService
    {
        private TableManager? mTableManager;
        private const int BATCH_SIZE = 500;
        private readonly IAnonymizationLogger anonymizationLogger = anonmyzationLogger;
        private readonly char[] chars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890".ToCharArray();


        private TableManager TableManager => mTableManager ??= new TableManager();


        public void Anonymize(ConnectionSettings connectionSettings, TablesConfiguration tablesConfiguration)
        {
            anonymizationLogger.LogStart();
            ConnectionHelper.ConnectionString = connectionSettings.ToConnectionString();
            foreach (var table in tablesConfiguration.Tables)
            {
                AnonymizeTable(table);
            }

            anonymizationLogger.LogEnd();
        }


        private void AnonymizeTable(TableConfiguration table)
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(table.TableName);
            if (!TableManager.TableExists(table.TableName))
            {
                return;
            }

            if (!table.AnonymizeColumns.Any())
            {
                return;
            }

            var identityColumns = TableManager.GetPrimaryKeyColumns(table.TableName);
            if (!identityColumns.Any())
            {
                return;
            }

            int currentPage = 0;
            IEnumerable<DataRow> rows;
            do
            {
                rows = GetPagedResult(table.TableName, table.AnonymizeColumns.Union(identityColumns), identityColumns[0], currentPage);
                var updateStatements = rows.Select(r => GetUpdateRowStatement(r, table.TableName, table.AnonymizeColumns, identityColumns));
                if (updateStatements.Any())
                {
                    string query = string.Join(Environment.NewLine, updateStatements);
                    int rowsModified = ConnectionHelper.ExecuteNonQuery(query, null, QueryTypeEnum.SQLQuery);
                    anonymizationLogger.LogModification(table.TableName, rowsModified);
                }

                currentPage++;
            }
            while (rows.Any());
        }


        private string? AnonymizeValue(object value)
        {
            string stringRepresentation = ValidationHelper.GetString(value, string.Empty);
            if (string.IsNullOrEmpty(stringRepresentation))
            {
                return null;
            }

            int size = stringRepresentation.Length;
            byte[] data = new byte[4 * size];
            using (var crypto = RandomNumberGenerator.Create())
            {
                crypto.GetBytes(data);
            }
            var result = new StringBuilder(size);
            for (int i = 0; i < size; i++)
            {
                uint rnd = BitConverter.ToUInt32(data, i * 4);
                long idx = rnd % chars.Length;

                result.Append(chars[idx]);
            }

            return result.ToString();

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
        private string GetUpdateRowStatement(
            DataRow row,
            string table,
            IEnumerable<string> columnsToUpdate,
            IEnumerable<string> identityColumns)
        {
            var values = new List<string>();
            foreach (string column in columnsToUpdate)
            {
                object currentValue = row[column];
                if (currentValue is null)
                {
                    continue;
                }

                string? newValue = AnonymizeValue(currentValue);
                if (newValue is null)
                {
                    continue;
                }

                values.Add($"{column} = '{newValue}'");
            }
            if (!values.Any())
            {
                return string.Empty;
            }

            var where = identityColumns.Select(col => $"{col} = {row[col]}");

            return $"UPDATE {table} SET {string.Join(",", values)} WHERE {string.Join(" AND ", where)}";
        }
    }
}
