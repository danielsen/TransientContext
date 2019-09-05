namespace TransientContext.Common
{
    public interface ITestDatabase
    {
        void Create();
        void RunScripts(string scriptFolderPath);
        string ConnectionString { get; }
        void Drop();
        bool Exists();
    }
}
