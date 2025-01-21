using CMS.ContactManagement;
using CMS.Ecommerce;
using CMS.EmailEngine;
using CMS.Globalization;
using CMS.Membership;

namespace XperienceCommunity.DatabaseAnonymizer.Models
{
    internal class TablesConfiguration
    {
        public IEnumerable<TableConfiguration> Tables { get; set; } = [
            new TableConfiguration() {
                TableName = "CMS_Country",
                AnonymizeColumns =
                [
                    nameof(CountryInfo.CountryName),
                    nameof(CountryInfo.CountryDisplayName),
                    nameof(CountryInfo.CountryTwoLetterCode),
                    nameof(CountryInfo.CountryThreeLetterCode),
                ]
            },
            new TableConfiguration() {
                TableName = "CMS_Email",
                AnonymizeColumns =
                [
                    nameof(EmailInfo.EmailFrom),
                    nameof(EmailInfo.EmailTo),
                    nameof(EmailInfo.EmailCc),
                    nameof(EmailInfo.EmailBcc),
                    nameof(EmailInfo.EmailSubject),
                    nameof(EmailInfo.EmailBody),
                    nameof(EmailInfo.EmailPlainTextBody),
                ]
            },
            new TableConfiguration() {
                TableName = "CMS_State",
                AnonymizeColumns =
                [
                    nameof(StateInfo.StateName),
                    nameof(StateInfo.StateDisplayName),
                    nameof(StateInfo.StateCode),
                ]
            },
            new TableConfiguration() {
                TableName = "CMS_User",
                AnonymizeColumns =
                [
                    nameof(UserInfo.FirstName),
                    nameof(UserInfo.MiddleName),
                    nameof(UserInfo.LastName),
                    nameof(UserInfo.FullName),
                    nameof(UserInfo.Email),
                    nameof(UserInfo.UserLastLogonInfo),
                ]
            },
            new TableConfiguration() {
                TableName = "CMS_UserSettings",
                AnonymizeColumns =
                [
                    nameof(UserSettingsInfo.UserNickName),
                    nameof(UserSettingsInfo.UserSignature),
                    nameof(UserSettingsInfo.UserRegistrationInfo),
                    nameof(UserSettingsInfo.UserSkype),
                    nameof(UserSettingsInfo.UserIM),
                    nameof(UserSettingsInfo.UserPhone),
                    nameof(UserSettingsInfo.UserPosition),
                ]
            },
            new TableConfiguration() {
                TableName = "COM_Address",
                AnonymizeColumns =
                [
                    nameof(AddressInfo.AddressName),
                    nameof(AddressInfo.AddressLine1),
                    nameof(AddressInfo.AddressLine2),
                    nameof(AddressInfo.AddressCity),
                    nameof(AddressInfo.AddressZip),
                    nameof(AddressInfo.AddressPhone),
                    nameof(AddressInfo.AddressPersonalName),
                ]
            },
            new TableConfiguration() {
                TableName = "COM_Customer",
                AnonymizeColumns =
                [
                    nameof(CustomerInfo.CustomerFirstName),
                    nameof(CustomerInfo.CustomerLastName),
                    nameof(CustomerInfo.CustomerEmail),
                    nameof(CustomerInfo.CustomerPhone),
                    nameof(CustomerInfo.CustomerFax),
                    nameof(CustomerInfo.CustomerCompany),
                ]
            },
            new TableConfiguration() {
                TableName = "COM_OrderAddress",
                AnonymizeColumns =
                [
                    nameof(OrderAddressInfo.AddressLine1),
                    nameof(OrderAddressInfo.AddressLine2),
                    nameof(OrderAddressInfo.AddressCity),
                    nameof(OrderAddressInfo.AddressZip),
                    nameof(OrderAddressInfo.AddressPhone),
                    nameof(OrderAddressInfo.AddressPersonalName),
                ]
            },
            new TableConfiguration() {
                TableName = "OM_Account",
                AnonymizeColumns =
                [
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
                ]
            },
            new TableConfiguration() {
                TableName = "OM_Contact",
                AnonymizeColumns =
                [
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
                ]
            },
        ];
    }
}
