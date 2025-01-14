using CMS;
using CMS.Core;
using CMS.DataEngine;

using XperienceCommunity.DatabaseAnonymizer;
using XperienceCommunity.DatabaseAnonymizer.Services;

[assembly: AssemblyDiscoverable]
[assembly: RegisterModule(typeof(AnonymizerModule))]
namespace XperienceCommunity.DatabaseAnonymizer
{
    internal class AnonymizerModule : Module
    {
        public AnonymizerModule() : base(nameof(AnonymizerModule)) { }


        protected override void OnInit()
        {
            base.OnInit();
            InstallSettingsKey();
            Service.Resolve<IAnonymizerService>().Run();
        }


        private void InstallSettingsKey()
        {
            var settingsKey = SettingsKeyInfoProvider.GetSettingsKeyInfo(AnonymizerService.ISANONYMIZED_SETTINGNAME);
            if (settingsKey is not null)
            {
                return;
            }

            new SettingsKeyInfo()
            {
                KeyIsGlobal = true,
                KeyIsHidden = true,
                KeyIsCustom = true,
                KeyValue = "False",
                KeyDefaultValue = "False",
                KeyType = nameof(Boolean),
                KeyName = AnonymizerService.ISANONYMIZED_SETTINGNAME,
                KeyDisplayName = AnonymizerService.ISANONYMIZED_SETTINGNAME
            }.Insert();
        }
    }
}
