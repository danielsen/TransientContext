using Npgsql;

namespace TransientContext.Postgresql
{
    class ConnectionStringManager : IConnectionStringManager
    {
        public ConnectionStringManager(string defaultConnectionString)
        {
            Default = defaultConnectionString;
        }

        public void SetCreatedDatabaseName(string value)
        {
            var connectionStringBuilder = new NpgsqlConnectionStringBuilder(Default);
            connectionStringBuilder.Database = value;
            CreatedDatabase = connectionStringBuilder.ToString();
        }

        public string Default { get; private set; }
        public string CreatedDatabase { get; private set; }
    }
}
