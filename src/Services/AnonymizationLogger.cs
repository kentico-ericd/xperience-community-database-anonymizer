using System.Text;

using CMS;
using CMS.Helpers;

using XperienceCommunity.DatabaseAnonymizer.Services;

[assembly: RegisterImplementation(typeof(IAnonymizationLogger), typeof(AnonymizationLogger))]
namespace XperienceCommunity.DatabaseAnonymizer.Services
{
    public class AnonymizationLogger : IAnonymizationLogger
    {
        private DateTime? mEnd;
        private DateTime? mStart;
        private readonly Dictionary<string, int> mModifications = [];


        public void LogEnd() => mEnd = DateTime.Now;


        public void LogModification(string table, int rowsModified)
        {
            if (rowsModified <= 0)
            {
                return;
            }

            if (!mModifications.ContainsKey(table))
            {
                mModifications.Add(table, rowsModified);

                return;
            }

            int modifications = mModifications[table];
            mModifications[table] = modifications + rowsModified;
        }


        public void LogStart() => mStart = DateTime.Now;


        public string GetLog()
        {
            var msgBuilder = new StringBuilder();
            foreach (string table in mModifications.Keys)
            {
                msgBuilder
                    .Append("- [")
                    .Append(table)
                    .Append("] updated ")
                    .Append(mModifications[table])
                    .AppendLine(" rows");
            }

            return $"Process finished in {GetMinutes()} minutes:{Environment.NewLine}{msgBuilder}";
        }


        private int GetMinutes()
        {
            if (mStart is null || mEnd is null)
            {
                return 0;
            }

            double seconds = (mEnd - mStart).Value.TotalSeconds;

            return ValidationHelper.GetInteger(Math.Ceiling(seconds / 60), 0);
        }
    }
}
