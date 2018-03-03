using System.Data.Common;
using petapoco.core;

namespace petapoco.providers {

    public class MariaDbDatabaseProvider : DatabaseProvider
    {
        public override DbProviderFactory GetFactory()
        {
            throw new System.NotImplementedException();
        }
    }
}