using System.Data.Common;
using petapoco.core;

namespace petapoco.providers {

    public class PostgresSQLDatabaseProvider : DatabaseProvider
    {
        public override DbProviderFactory GetFactory()
        {
            throw new System.NotImplementedException();
        }
    }
}