# Usage Guide

## Supported tables

See the default implementation [`AnonymizationTableProvider`](/src/Services/AnonymizationTableProvider.cs) for a full list of supported tables and columns. This integration currently anonymizes the data contained in the following tables:

- OM_Contact

## Adding your tables

If the list of [supported tables](#supported-tables) doesn't meet your needs, you use a custom `IAnonymizationTableProvider` to generate a list of your own. You can copy tables from [`AnonymizationTableProvider`](/src/Services/AnonymizationTableProvider.cs), add your tables and columns, and use the `RegisterImplementation` to register it. For example, you may want to anonymize data entered in your forms:

```cs
[assembly: RegisterImplementation(typeof(IAnonymizationTableProvider), typeof(MyTableProvider))]
//...
public class MyTableProvider : IAnonymizationTableProvider
{
    private readonly Dictionary<string, string[]> myTables = new Dictionary<string, string[]>
    {
        { "OM_Contact", new string[]
            {
                nameof(ContactInfo.ContactFirstName),
                // Other columns from default implementation...
            }
        },
        { "DancingGoat_ContactForm", new string[]
            {
                "Name",
                "Email"
            }
        }
    };

    public IEnumerable<string> GetTables() => myTables.Keys;

    public IEnumerable<string> GetColumns(string tableName) =>
        myTables.FirstOrDefault(x => x.Key.Equals(tableName, StringComparison.OrdinalIgnoreCase)).Value;
}
```
