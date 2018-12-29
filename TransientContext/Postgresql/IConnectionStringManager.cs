namespace TransientContext.Postgresql
{
    interface IConnectionStringManager
    {
        void SetCreatedDatabaseName(string value);
        string Default { get; }
        string CreatedDatabase { get; }
    }
}
