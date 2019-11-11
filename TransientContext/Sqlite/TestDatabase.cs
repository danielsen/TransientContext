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
            if (Exists())
            {
                File.Delete(_connectionStringManager.DatabaseName);
            }
        }

        public bool Exists()
        {
            var file = File.Exists(_connectionStringManager.DatabaseName);
            return file;
        }
    }
}
