using System.Data.Common;

namespace petapoco.core
{
    public interface IProvider {
        
        string GetParameterPrefix(string connectionString);

        DbProviderFactory GetFactory();
    }

}