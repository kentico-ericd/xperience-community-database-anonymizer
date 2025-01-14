namespace XperienceCommunity.DatabaseAnonymizer.Services
{
    /// <summary>
    /// Contains methods for retrieving the data which should be anonymized.
    /// </summary>
    public interface IAnonymizationTableProvider
    {
        /// <summary>
        /// Gets the table names which contain columns to be anonymized.
        /// </summary>
        IEnumerable<string> GetTables();


        /// <summary>
        /// Gets the column names to be anonymized.
        /// </summary>
        /// <param name="tableName">The name of the table to retrieve columns from.</param>
        IEnumerable<string> GetColumns(string tableName);
    }
}
