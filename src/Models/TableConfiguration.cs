namespace XperienceCommunity.DatabaseAnonymizer.Models
{
    internal class TableConfiguration
    {
        public string? TableName { get; set; }


        public IEnumerable<string> AnonymizeColumns { get; set; } = [];
    }
}
