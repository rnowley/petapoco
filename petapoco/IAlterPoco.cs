namespace petapoco
{

    public interface IAlterPoco
    {
        object Insert(string tableName, object poco);

        object Insert(string tableName, string primaryKeyName, object poco);
        
    }
}