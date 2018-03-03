using System;
using System.Configuration;
using System.Collections.Generic;
using System.Data;
using petapoco.core;
using Microsoft.Extensions.Configuration;
using System.IO;
using System.Data.Common;
using System.Linq;

namespace petapoco {

    public class Database : IDatabase
    {
        private IMapper _defaultMapper;
        private string _connectionString;
        private IProvider _provider;
        private IDbConnection _sharedConnection;
        private IDbTransaction _transaction;
        private int _sharedConnectionDepth;
        private int _transactionDepth;
        private string _lastSql;
        private object[] _lastArgs;
        private string _paramPrefix;
        private DbProviderFactory _factory;
        private IsolationLevel? _isolationLevel;

        public Database() {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json");

            var config = builder.Build();

            var appConfig = new AppSettings();
            appConfig = (AppSettings)config.GetSection("AppSettings");

            if (appConfig.ConnectionStrings.Count == 0) 
            {
                throw new InvalidOperationException("One or more connection strings must be registered to use the no parameter constructor");
            }

            var entry = appConfig.ConnectionStrings[0];
            _connectionString = entry.ConnectionString;
            string providerName = !string.IsNullOrEmpty(entry.ProviderName) ? entry.ProviderName : "System.Data.SqlClient";
            Initialise(DatabaseProvider.Resolve(providerName, false, _connectionString), null);
        }

        public Database(string connectionStringName) {

            if (string.IsNullOrEmpty(connectionStringName))
            {
                throw new ArgumentException("Connection string name must not be null or empty", nameof(connectionStringName));
            }

            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json");

            var config = builder.Build();

            var appConfig = new AppSettings();
            appConfig = (AppSettings)config.GetSection("AppSettings");
            var entry = appConfig.ConnectionStrings.Single(cs => cs.ConnectionString == connectionStringName);
            _connectionString = entry.ConnectionString;
            var providerName = !string.IsNullOrEmpty(entry.ProviderName) ? entry.ProviderName : "System.Data.SqlClient";
            Initialise(DatabaseProvider.Resolve(providerName, false, _connectionString), null);
        }

        public static IConfiguration Configuration { get; set; }

        public IMapper DefaultMapper => throw new NotImplementedException();

        public string LastSql => throw new NotImplementedException();

        public object[] LastArgs => throw new NotImplementedException();

        public string LastCommand => throw new NotImplementedException();

        public bool EnableAutoSelect { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public bool EnableNamedParameters { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public int CommandTimeout { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public int OneTimeCommandTimeout { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public IProvider Provider => throw new NotImplementedException();

        public string ConnectionString => throw new NotImplementedException();

        public IsolationLevel? IsolationLevel { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public void AbortTransaction()
        {
            throw new NotImplementedException();
        }

        public void BeginTransaction()
        {
            throw new NotImplementedException();
        }

        public void CompleteTransaction()
        {
            throw new NotImplementedException();
        }

        public virtual void OnExecutedCommand(IDbCommand command) {}

        public virtual void OnConnectionClosing(IDbConnection connection) {}

        public void Dispose()
        {
            CloseSharedConnection();
        }

        public int Execute(string sql, params object[] args)
        {
            throw new NotImplementedException();
        }

        public int Execute(Sql sql)
        {
            throw new NotImplementedException();
        }

        public T ExecuteScalar<T>(string sql, params object[] args)
        {
            throw new NotImplementedException();
        }

        public T ExecuteScalar<T>(Sql sql)
        {
            throw new NotImplementedException();
        }

        public List<T> Fetch<T>(string sql, params object[] args)
        {
            throw new NotImplementedException();
        }

        public List<T> Fetch<T>(Sql sql)
        {
            throw new NotImplementedException();
        }

        public ITransaction GetTransaction()
        {
            throw new NotImplementedException();
        }

        public object Insert(string tableName, object poco)
        {
            throw new NotImplementedException();
        }

        public object Insert(string tableName, string primaryKeyName, object poco)
        {
            throw new NotImplementedException();
        }

        public Page<T> Page<T>(long page, long itemsPerPage, string sqlCount, object[] countArgs, string sqlPage, object[] pageArgs)
        {
            throw new NotImplementedException();
        }

        public Page<T> Page<T>(long page, long itemsPerPage, string sql, params object[] args)
        {
            throw new NotImplementedException();
        }

        public Page<T> Page<T>(long page, long itemsPerPage, Sql sql)
        {
            throw new NotImplementedException();
        }

        public Page<T> Page<T>(long page, long itemsPerPage, Sql sqlCount, Sql sqlPage)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<T> Query<T>(string sql, params object[] args)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<T> Query<T>(Sql sql)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<TRet> Query<T1, T2, TRet>(Func<T1, T2, TRet> cb, string sql, params object[] args)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<TRet> Query<T1, T2, T3, TRet>(Func<T1, T2, T3, TRet> cb, string sql, params object[] args)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<TRet> Query<T1, T2, T3, T4, TRet>(Func<T1, T2, T3, T4, TRet> cb, string sql, params object[] args)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<TRet> Query<T1, T2, T3, T4, T5, TRet>(Func<T1, T2, T3, T4, T5, TRet> cb, string sql, params object[] args)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<T1> Query<T1, T2>(Sql sql)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<T1> Query<T1, T2, T3>(Sql sql)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<T1> Query<T1, T2, T3, T4>(Sql sql)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<T1> Query<T1, T2, T3, T4, T5>(Sql sql)
        {
            throw new NotImplementedException();
        }

        public void CloseSharedConnection() {

            if (_sharedConnectionDepth > 0)
            {
                --_sharedConnectionDepth;

                if (_sharedConnectionDepth == 0)
                {
                    OnConnectionClosing(_sharedConnection);
                    _sharedConnection.Dispose();
                    _sharedConnection = null;
                }

            }

        }

        public IEnumerable<TRet> Query<TRet>(Type[] types, object cb, string sql, params object[] args)
        {
            throw new NotImplementedException();
        }

        private void Initialise(IProvider provider, IMapper mapper) {
            _transactionDepth = 0;
            EnableAutoSelect = true;
            EnableNamedParameters = true;

            _provider = provider;
            _paramPrefix = _provider.GetParameterPrefix(_connectionString);
            _factory = _provider.GetFactory();

            _defaultMapper = mapper ?? new ConventionMapper();
        }

        internal void DoPreExecute(IDbCommand command) {

        }
    }
}