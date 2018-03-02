using System;
using System.Data;
using petapoco.core;

namespace petapoco
{
    public interface IDatabase :IDisposable, IQuery, IAlterPoco, IExecute,
        ITransactionAccessor 
    {
        IMapper DefaultMapper { get; }

        string LastSql { get; }

        object[] LastArgs { get; }

        string LastCommand { get; }

        bool EnableAutoSelect { get; set; }

        bool EnableNamedParameters { get; set; }

        int CommandTimeout { get; set; }

        int OneTimeCommandTimeout { get;set; }

        IProvider Provider { get; }

        string ConnectionString { get; }

        IsolationLevel? IsolationLevel { get; set; }

        ITransaction GetTransaction();

        void BeginTransaction();

        void AbortTransaction();

        void CompleteTransaction();
    }
}
