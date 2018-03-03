namespace petapoco.utilities {

    public class PagingHelper : IPagingHelper
    {
        public static IPagingHelper Instance { get; private set; }
        public bool SplitSql(string sql, out SQLParts parts)
        {
            throw new System.NotImplementedException();
        }
    }

}