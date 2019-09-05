using System;
using System.IO;
using System.Linq;
using System.Text;
using TransientContext.Common;

namespace TransientContext.Sqlite
{
    class TestDatabase : ITestDatabase
    {
        public string ConnectionString { get; private set; }
        public string TemplateDatabase { get; set; }

        private readonly IConnectionStringManager _connectionStringManager;
        private readonly IDatabaseNameGenerator _databaseNameGenerator;
        private readonly IConnection _connection;
        private string _createdDatabaseName;

        public TestDatabase(IConnectionStringManager connectionStringManager,
            IDatabaseNameGenerator databaseNameGenerator, IConnection connection)
        {
            _connectionStringManager = connectionStringManager;
            _databaseNameGenerator = databaseNameGenerator;
            _connection = connection;
        }

        public void Create()
        {
            _createdDatabaseName = _databaseNameGenerator.Generate();
            _connectionStringManager.SetCreatedDatabaseName(_createdDatabaseName);
            ConnectionString = _connectionStringManager.CreatedDatabase;
            _connection.Execute(_connectionStringManager.Default, BuildCreateStatement());
        }

        private string BuildCreateStatement()
        {
            StringBuilder stringBuilder = new StringBuilder($"create database {_createdDatabaseName}");

            if (!string.IsNullOrWhiteSpace(TemplateDatabase))
            {
                stringBuilder.Append($" template {TemplateDatabase}");
            }

            stringBuilder.Append(";");
            return stringBuilder.ToString();
        }

        public void RunScripts(string scriptFolderPath)
        {
            if (!Directory.Exists(scriptFolderPath))
            {
                throw new ArgumentException($"Directory does not exist: {scriptFolderPath}");
            }

            Directory.GetFiles(scriptFolderPath)
                .OrderBy(s => s)
                .Select(File.ReadAllText)
                .ToList()
                .ForEach(command => _connection.Execute(_connectionStringManager.CreatedDatabase, command));
        }

        public void Drop()
        {
            _connection.Execute(_connectionStringManager.Default, 
                $"select pid, pg_terminate_backend(pid) from pg_stat_activity where datname = '{_createdDatabaseName}' and pid <> pg_backend_pid();");
            _connection.Execute(_connectionStringManager.Default,
                $"drop database {_createdDatabaseName}");
        }

        public bool Exists()
        {
            _connection.ExecuteReader(_connectionStringManager.Default,
                $"select 1 as result from pg_database where datname = '{_createdDatabaseName}'", 
                out int rows);

            return rows > 0;
        }
    }
}
