using CMS;
using CMS.ContactManagement;

using XperienceCommunity.DatabaseAnonymizer.Services;

[assembly: RegisterImplementation(typeof(IAnonymizationTableProvider), typeof(AnonymizationTableProvider))]
namespace XperienceCommunity.DatabaseAnonymizer.Services
{
    public class AnonymizationTableProvider : IAnonymizationTableProvider
    {
        private readonly Dictionary<string, string[]> tablesToAnonymize = new()
        {
            { "OM_Contact", new string[]
                {
                    nameof(ContactInfo.ContactFirstName),
                    nameof(ContactInfo.ContactMiddleName),
                    nameof(ContactInfo.ContactLastName),
                    nameof(ContactInfo.ContactJobTitle),
                    nameof(ContactInfo.ContactAddress1),
                    nameof(ContactInfo.ContactCity),
                    nameof(ContactInfo.ContactZIP),
                    nameof(ContactInfo.ContactMobilePhone),
                    nameof(ContactInfo.ContactBusinessPhone),
                    nameof(ContactInfo.ContactEmail),
                    nameof(ContactInfo.ContactNotes),
                    nameof(ContactInfo.ContactCompanyName),
                }
            }
        };


        public IEnumerable<string> GetTables() => tablesToAnonymize.Keys;


        public IEnumerable<string> GetColumns(string tableName) =>
            tablesToAnonymize.FirstOrDefault(x => x.Key.Equals(tableName, StringComparison.OrdinalIgnoreCase)).Value;
    }
}
