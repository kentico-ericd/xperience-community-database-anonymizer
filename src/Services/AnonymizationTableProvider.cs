using CMS;
using CMS.ContactManagement;
using CMS.Core;
using CMS.Ecommerce;
using CMS.EmailEngine;
using CMS.Membership;

using XperienceCommunity.DatabaseAnonymizer.Services;

[assembly: RegisterImplementation(typeof(IAnonymizationTableProvider), typeof(AnonymizationTableProvider), Priority = RegistrationPriority.Fallback)]
namespace XperienceCommunity.DatabaseAnonymizer.Services
{
    public class AnonymizationTableProvider : IAnonymizationTableProvider
    {
        private readonly Dictionary<string, string[]> tablesToAnonymize = new()
        {
            // CMS tables
            { "CMS_Email", new string[]
                {
                    nameof(EmailInfo.EmailFrom),
                    nameof(EmailInfo.EmailTo),
                    nameof(EmailInfo.EmailCc),
                    nameof(EmailInfo.EmailBcc),
                    nameof(EmailInfo.EmailSubject),
                    nameof(EmailInfo.EmailBody),
                    nameof(EmailInfo.EmailPlainTextBody),
                }
            },
            { "CMS_User", new string[]
                {
                    nameof(UserInfo.FirstName),
                    nameof(UserInfo.MiddleName),
                    nameof(UserInfo.LastName),
                    nameof(UserInfo.FullName),
                    nameof(UserInfo.Email),
                    nameof(UserInfo.UserLastLogonInfo),
                }
            },
            { "CMS_UserSettings", new string[]
                {
                    nameof(UserSettingsInfo.UserNickName),
                    nameof(UserSettingsInfo.UserSignature),
                    nameof(UserSettingsInfo.UserRegistrationInfo),
                    nameof(UserSettingsInfo.UserSkype),
                    nameof(UserSettingsInfo.UserIM),
                    nameof(UserSettingsInfo.UserPhone),
                    nameof(UserSettingsInfo.UserPosition),
                }
            },
            // COM tables
            { "COM_Address", new string[]
                {
                    nameof(AddressInfo.AddressName),
                    nameof(AddressInfo.AddressLine1),
                    nameof(AddressInfo.AddressLine2),
                    nameof(AddressInfo.AddressCity),
                    nameof(AddressInfo.AddressZip),
                    nameof(AddressInfo.AddressPhone),
                    nameof(AddressInfo.AddressPersonalName),
                }
            },
            { "COM_Customer", new string[]
                {
                    nameof(CustomerInfo.CustomerFirstName),
                    nameof(CustomerInfo.CustomerLastName),
                    nameof(CustomerInfo.CustomerEmail),
                    nameof(CustomerInfo.CustomerPhone),
                    nameof(CustomerInfo.CustomerFax),
                    nameof(CustomerInfo.CustomerCompany),
                }
            },
            { "COM_OrderAddress", new string[]
                {
                    nameof(OrderAddressInfo.AddressLine1),
                    nameof(OrderAddressInfo.AddressLine2),
                    nameof(OrderAddressInfo.AddressCity),
                    nameof(OrderAddressInfo.AddressZip),
                    nameof(OrderAddressInfo.AddressPhone),
                    nameof(OrderAddressInfo.AddressPersonalName),
                }
            },
            // OM tables
            { "OM_Account", new string[]
                {
                    nameof(AccountInfo.AccountName),
                    nameof(AccountInfo.AccountAddress1),
                    nameof(AccountInfo.AccountAddress2),
                    nameof(AccountInfo.AccountCity),
                    nameof(AccountInfo.AccountZIP),
                    nameof(AccountInfo.AccountWebSite),
                    nameof(AccountInfo.AccountPhone),
                    nameof(AccountInfo.AccountEmail),
                    nameof(AccountInfo.AccountFax),
                    nameof(AccountInfo.AccountNotes),
                }
            },
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
