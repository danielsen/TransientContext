using Npgsql;
using TransientContext.Common;

namespace TransientContext.Postgresql
{
    class Connection : IConnection
    {
        public void Execute(string connectionString, string command)
        {
            using (NpgsqlConnection connection = new NpgsqlConnection(connectionString))
            {
                connection.Open();

                using (var npgsqlCommand = connection.CreateCommand())
                {
                    npgsqlCommand.CommandText = command;
                    npgsqlCommand.ExecuteNonQuery();
                }

                connection.Close();
            }
        }

        public void ExecuteReader(string connectionString, string command, out int rows)
        {
            rows = 0;
            using (NpgsqlConnection connection = new NpgsqlConnection(connectionString))
            {
                connection.Open();

                using (var npgsqlCommand = connection.CreateCommand())
                {
                    npgsqlCommand.CommandText = command;
                    using (var npgsqlReader = npgsqlCommand.ExecuteReader())
                    {
                        if (npgsqlReader.HasRows)
                        {
                            while (npgsqlReader.Read())
                            {
                                rows++;
                            }
                        }
                    }
                }
                connection.Close();
            }
        }
    }
}