using TransientContext.Common;
using System.Data.SQLite;

namespace TransientContext.Sqlite
{
    public class ConnectionStringManager : IConnectionStringManager
    {
        public ConnectionStringManager(string defaultConnectionString)
        {
            Default = defaultConnectionString;
        }
        
        public void SetCreatedDatabaseName(string value)
        {
            var connectionStringBuilder = new SQLiteConnectionStringBuilder(Default)
            {
                DataSource = value
            };
            CreatedDatabase = connectionStringBuilder.ToString();
        }

        public string Default { get; }
        public string CreatedDatabase { get; private set; }
    }
}