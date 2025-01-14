namespace XperienceCommunity.DatabaseAnonymizer.Services
{
    /// <summary>
    /// Contains methods for anonymizing or deanonymizing database records.
    /// </summary>
    public interface IAnonymizerService
    {
        /// <summary>
        /// Determines whether the anonymization process should run, then runs it.
        /// </summary>
        void Run();
    }
}
