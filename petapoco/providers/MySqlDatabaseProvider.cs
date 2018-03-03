using System.Data.Common;
using petapoco.core;

namespace petapoco.providers {

    public class MySqlDatabaseProvider : DatabaseProvider
    {
        public override DbProviderFactory GetFactory()
        {
            throw new System.NotImplementedException();
        }
    }
}