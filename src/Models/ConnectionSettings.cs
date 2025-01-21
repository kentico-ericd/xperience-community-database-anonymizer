using System.Data.SqlClient;

namespace XperienceCommunity.DatabaseAnonymizer.Models
{
    internal class ConnectionSettings
    {
        public string? DataSource { get; set; }


        public string? UserID { get; set; }


        public string? Password { get; set; }


        public string? DatabaseName { get; set; }



        public string ToConnectionString()
        {
            var builder = new SqlConnectionStringBuilder();
            if (!string.IsNullOrEmpty(DataSource))
            {
                builder.DataSource = DataSource;
            }

            if (!string.IsNullOrEmpty(UserID))
            {
                builder.UserID = UserID;
            }

            if (!string.IsNullOrEmpty(Password))
            {
                builder.Password = Password;
            }

            if (!string.IsNullOrEmpty(DatabaseName))
            {
                builder.InitialCatalog = DatabaseName;
            }

            return builder.ConnectionString;
        }
    }
}
