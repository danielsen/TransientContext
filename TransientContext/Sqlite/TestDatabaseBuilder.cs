using System;
using Microsoft.Extensions.Configuration;
using TransientContext.Common;
using TransientContext.Postgresql;

namespace TransientContext.Sqlite
{
    public class TestDatabaseBuilder
    {
        private string _connectionString;
        private string _connectionStringName = "DefaultConnection";
        private string _templateDatabase;
        private IConfiguration _configuration;
        private readonly DatabaseNameGenerator _databaseNameGenerator = new DatabaseNameGenerator();

        public TestDatabaseBuilder WithConnectionString(string value)
        {
            _connectionString = value;
            return this;
        }

        public TestDatabaseBuilder WithConfiguration(IConfiguration value)
        {
            _configuration = value;
            return this;
        }

        public TestDatabaseBuilder WithConnectionStringName(string value)
        {
            _connectionStringName = value;
            return this;
        }

        public TestDatabaseBuilder WithDatabaseNamePrefix(string value)
        {
            _databaseNameGenerator.Prefix = value;
            return this;
        }

        public TestDatabaseBuilder WithTemplateDatabase(string value)
        {
            _templateDatabase = value;
            return this;
        }

        public ITestDatabase Build()
        {
            if (string.IsNullOrWhiteSpace(_connectionString) && _configuration == null)
            {
                throw new ArgumentException("No connection string or configuration was provided");
            }

            if (_configuration == null)
                return new TestDatabase(new ConnectionStringManager(_connectionString), _databaseNameGenerator,
                    new Connection())
                {
                    TemplateDatabase = _templateDatabase
                };
            _connectionString = _configuration.GetConnectionString(_connectionStringName);
            
            if (string.IsNullOrWhiteSpace(_connectionString))
            {
                throw new ArgumentException($"Could not find connection string with name: {_connectionStringName}");
            }

            return new TestDatabase(new ConnectionStringManager(_connectionString), _databaseNameGenerator,
                new Connection())
            {
                TemplateDatabase = _templateDatabase
            };
        }
    }
}