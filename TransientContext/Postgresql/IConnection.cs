namespace TransientContext.Postgresql
{
    public interface IConnection
    {
        void Execute(string connectionString, string command);
    }
}
