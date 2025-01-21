namespace XperienceCommunity.DatabaseAnonymizer.Models
{
    /// <summary>
    /// Represents a table and its columns used during anonymization.
    /// </summary>
    internal class TableConfiguration
    {
        /// <summary>
        /// The table name.
        /// </summary>
        public string? TableName { get; set; }


        /// <summary>
        /// The columns in the table to be anonymized.
        /// </summary>
        public IEnumerable<string> AnonymizeColumns { get; set; } = [];
    }
}
