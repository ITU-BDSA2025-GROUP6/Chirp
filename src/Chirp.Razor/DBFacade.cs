using Microsoft.AspNetCore.Mvc.ModelBinding.Metadata;

using Microsoft.Data.Sqlite;

namespace Chirp.Razor;
public class DBFacade
{
    private SqliteConnection _connection = null;
    
    public DBFacade()
    {
        _connection = new SqliteConnection("Data Source=./chirp.db");
    }
    public SqliteConnection getConnection()
    {
        try
        {
            _connection.Open();
            return _connection;
        }
        catch (Exception e)
        {
            Console.WriteLine("Could not retrieve the database, error: " + e.GetBaseException());
        }
        return null;
    }
}