namespace TransientContext.Common
{
    interface IConnectionStringManager
    {
        void SetCreatedDatabaseName(string value);
        string Default { get; }
        string CreatedDatabase { get; }
        string DatabaseName { get; }
    }
}
