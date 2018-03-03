namespace petapoco.utilities {

    public interface IPagingHelper 
    {
        bool SplitSql(string sql, out SQLParts parts);
    }
}