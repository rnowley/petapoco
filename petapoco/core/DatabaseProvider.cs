using System;
using System.Data;
using System.Data.Common;
using System.Linq;
using petapoco.providers;
using petapoco.utilities;

namespace petapoco.core {

    public abstract class DatabaseProvider : IProvider {

        public abstract DbProviderFactory GetFactory();

        public virtual bool HasNativeGuidSupport {
            get { return false; }
        }

        public virtual IPagingHelper PagingUtility {
            get { return PagingHelper.Instance; }
        }

        public virtual string EscapeTableName(string tableName) {
            return tableName.IndexOf('.') >= 0 ? tableName : EscapeSqlIdentifier(tableName);
        }

        public virtual string EscapeSqlIdentifier(string sqlIdentifier) {
            return string.Format("[{0}]", sqlIdentifier);
        }

        public virtual string GetParameterPrefix(string connectionString) {
            return "@";
        }

        public virtual object MapParameterValue(object value) {

        
            if (value is bool)
            {
                return ((bool)value) ? 1 : 0;
            }
        
            return value;
        }

        public virtual string BuildPageQuery(long skip, long take, SQLParts parts, ref object[] args) {
            var sql = string.Format("{0}\nLIMIT @{1} OFFSET @{2}", parts.Sql, args.Length, args.Length + 1);
            args = args.Concat(new object[] { take, skip }).ToArray();
            return sql;
        }

        public virtual string GetExistsSql() {
            return "SELECT COUN(*) FROM {0} WHERE {1}";
        }

        public virtual string GetAutoIncrementExpression(TableInfo tableInfo) {
            return null;
        }

        public virtual string GetInsertOutputClause(string primaryKeyName) {
            return string.Empty;
        }

        public virtual object ExecuteInsert(Database database, IDbCommand command, string primaryKeyName) {
            command.CommandText += ";\nSELECT @@IDENTITY AS NewID;";
            return ExecuteScalarHelper(database, command);
        }

        protected DbProviderFactory GetFactory(params string[] assemblyQualifiedNames) {
            Type ft = null;

            foreach (var assemblyName in assemblyQualifiedNames)
            {
                ft = Type.GetType(assemblyName);

                if (ft != null) 
                {
                    break;
                }

            }

            if (ft == null)
            {
                throw new ArgumentException("Could not load the " + GetType().Name + "DbProviderFactory.");
            }

            return (DbProviderFactory)ft.GetField("Instance").GetValue(null);
        }

        internal static IProvider Resolve(Type type, bool allowDefault, string connectionString) {
            var typeName = type.Name;

            if (typeName.StartsWith("MySql")) {
                return Singleton<MySqlDatabaseProvider>.Instance;
            } else if (typeName.StartsWith("MariaDb")) {
                return Singleton<MariaDbDatabaseProvider>.Instance;
            } else if (typeName.StartsWith("Npgsql") || typeName.StartsWith("PgSql")) {
                return Singleton<PostgresSQLDatabaseProvider>.Instance;
            } else {
                // Default to SQLite
                return Singleton<SQLiteDatabaseProvider>.Instance;
            }
        }

        internal static IProvider Resolve(string providerName, bool allowDefault, string connectionString) {

            if (providerName.IndexOf("MySql", StringComparison.InvariantCultureIgnoreCase) >= 0)
            {
                return Singleton<MySqlDatabaseProvider>.Instance;
            } else if (providerName.IndexOf("MariaDb", StringComparison.InvariantCultureIgnoreCase) >= 0)
            {
                return Singleton<MariaDbDatabaseProvider>.Instance;
            } else if (providerName.IndexOf("Npgsql", StringComparison.InvariantCultureIgnoreCase) >= 0 || providerName.IndexOf("pgsql", StringComparison.InvariantCultureIgnoreCase) >= 0)
            {
                return Singleton<PostgresSQLDatabaseProvider>.Instance;
            } else if (providerName.IndexOf("SQLite", StringComparison.InvariantCultureIgnoreCase) >= 0)
            {
                return Singleton<SQLiteDatabaseProvider>.Instance;
            }

            if (!allowDefault)
            {
                throw new ArgumentException("Could not match '" + providerName + "' to a provider.", nameof(providerName));
            }

            // Assume SQLite
            return Singleton<SQLiteDatabaseProvider>.Instance;
        }

        internal static DbProviderFactory Unwrap(DbProviderFactory factory) {
            var sp = factory as IServiceProvider;

            if (sp == null)
            {
                return factory;
            }

            var unwrapped = sp.GetService(factory.GetType()) as DbProviderFactory;
            return unwrapped == null ? factory : Unwrap(unwrapped);
        }

        protected void ExecuteNonQueryHelper(Database database, IDbCommand command) {
            database.DoPreExecute(command);
            command.ExecuteNonQuery();
            database.OnExecutedCommand(command);
        }

        protected object ExecuteScalarHelper(Database database, IDbCommand command) {
            database.DoPreExecute(command);
            object r = command.ExecuteScalar();
            database.OnExecutedCommand(command);
            return r;
        }
    }
}