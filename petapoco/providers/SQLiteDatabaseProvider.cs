using System.Data.Common;
using petapoco.core;

namespace petapoco.providers {

    public class SQLiteDatabaseProvider : DatabaseProvider
    {
        public override DbProviderFactory GetFactory()
        {
            throw new System.NotImplementedException();
        }
    }
}