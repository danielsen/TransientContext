using System.Data.SQLite;
using TransientContext.Common;

namespace TransientContext.Sqlite
{
    public class Connection : IConnection
    {
        public void Execute(string connectionString, string command)
        {
            using (SQLiteConnection connection = new SQLiteConnection(connectionString))
            {
                connection.Open();

                using (var sqliteCommand = connection.CreateCommand())
                {
                    sqliteCommand.CommandText = command;
                    sqliteCommand.ExecuteNonQuery();
                }

                connection.Close();
            }
        }

        public void ExecuteReader(string connectionString, string command, out int rows)
        {
            rows = 0;
            using (SQLiteConnection connection = new SQLiteConnection(connectionString))
            {
                connection.Open();

                using (var sqliteCommand = connection.CreateCommand())
                {
                    sqliteCommand.CommandText = command;
                    using (var sqliteReader = sqliteCommand.ExecuteReader())
                    {
                        if (sqliteReader.HasRows)
                        {
                            while (sqliteReader.Read())
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