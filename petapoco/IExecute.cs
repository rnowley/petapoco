using petapoco.core;

namespace petapoco
{
    public interface IExecute
    {
        int Execute(string sql, params object[] args);

        int Execute(Sql sql);

        T ExecuteScalar<T>(string sql, params object[] args);

        T ExecuteScalar<T>(Sql sql);
    }

}