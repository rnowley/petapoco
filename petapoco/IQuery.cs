using System;
using System.Collections.Generic;
using petapoco.core;

namespace petapoco
{

    public interface IQuery
    {
        IEnumerable<T> Query<T>(string sql, params object[] args);

        IEnumerable<T> Query<T>(Sql sql);

        IEnumerable<TRet> Query<T1, T2, TRet>(Func<T1, T2, TRet> cb, string sql,
                params object[] args);

        IEnumerable<TRet> Query<T1, T2, T3, TRet>(Func<T1, T2, T3, TRet> cb, string sql,
                params object[] args);

        IEnumerable<TRet> Query<T1, T2, T3, T4, TRet>(Func<T1, T2, T3, T4, TRet> cb, string sql,
                params object[] args);

        IEnumerable<TRet> Query<T1, T2, T3, T4, T5, TRet>(Func<T1, T2, T3, T4, T5, TRet> cb, string sql,
                params object[] args);

        IEnumerable<T1> Query<T1, T2>(Sql sql);

        IEnumerable<T1> Query<T1, T2, T3>(Sql sql);

        IEnumerable<T1> Query<T1, T2, T3, T4>(Sql sql);

        IEnumerable<T1> Query<T1, T2, T3, T4, T5>(Sql sql);

        IEnumerable<TRet> Query<TRet>(Type[] types, object cb, string sql, 
                params object[] args);

        List<T> Fetch<T>(string sql, params object[] args);

        List<T> Fetch<T>(Sql sql);

        Page<T> Page<T>(long page, long itemsPerPage, string sqlCount, object[] countArgs,
                        string sqlPage, object[] pageArgs);

        Page<T> Page<T>(long page, long itemsPerPage, string sql, params object[] args);

        Page<T> Page<T>(long page, long itemsPerPage, Sql sql);

        Page<T> Page<T>(long page, long itemsPerPage, Sql sqlCount, Sql sqlPage);
    }   
}