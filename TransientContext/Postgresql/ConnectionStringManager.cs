﻿using Npgsql;
using TransientContext.Common;

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
            var connectionStringBuilder = new NpgsqlConnectionStringBuilder(Default)
            {
                Database = value
            };
            CreatedDatabase = connectionStringBuilder.ToString();
        }

        public string Default { get; }
        public string CreatedDatabase { get; private set; }
    }
}
