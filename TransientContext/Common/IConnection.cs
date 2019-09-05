namespace TransientContext.Common
{
    public interface IConnection
    {
        void Execute(string connectionString, string command);
        void ExecuteReader(string connectionString, string command, out int rows);
    }
}
