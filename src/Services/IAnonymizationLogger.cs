namespace XperienceCommunity.DatabaseAnonymizer.Services
{
    /// <summary>
    /// Contains methods for recording anonymization operations.
    /// </summary>
    public interface IAnonymizationLogger
    {
        /// <summary>
        /// Logs the start time of the anonymization process.
        /// </summary>
        void LogStart();


        /// <summary>
        /// Updates the number of rows modified for a table.
        /// </summary>
        /// <param name="table">The database table.</param>
        /// <param name="rowsModified">The number of rows modified.</param>
        void LogModification(string table, int rowsModified);


        /// <summary>
        /// Logs the end time of the anonymization process.
        /// </summary>
        void LogEnd();


        /// <summary>
        /// Gets a string summarizing the anonymization process suitable for the Event log.
        /// </summary>
        string GetLog();
    }
}
