using CMS;
using CMS.Core;

using Newtonsoft.Json;

using XperienceCommunity.DatabaseAnonymizer.Models;
using XperienceCommunity.DatabaseAnonymizer.Services;

[assembly: RegisterImplementation(typeof(IAnonymizationTableProvider), typeof(AnonymizationTableProvider), Priority = RegistrationPriority.Fallback)]
namespace XperienceCommunity.DatabaseAnonymizer.Services
{
    internal class AnonymizationTableProvider : IAnonymizationTableProvider
    {
        public async Task<TablesConfiguration> GetTablesConfig()
        {
            if (!File.Exists(Constants.TABLES_FILENAME))
            {
                var newConfig = new TablesConfiguration();
                await WriteTablesConfig(newConfig);

                return newConfig;
            }

            string text = await File.ReadAllTextAsync(Constants.TABLES_FILENAME);
            var config = JsonConvert.DeserializeObject<TablesConfiguration>(text) ??
                throw new JsonReaderException($"The configuration file {Constants.TABLES_FILENAME} cannot be deserialized.");

            return config;
        }


        private static Task WriteTablesConfig(TablesConfiguration config) =>
            File.WriteAllTextAsync(Constants.TABLES_FILENAME, JsonConvert.SerializeObject(config, Formatting.Indented));
    }
}
