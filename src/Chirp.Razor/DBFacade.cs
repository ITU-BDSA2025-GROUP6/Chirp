using Microsoft.Data.Sqlite;

namespace Chirp.Razor;
public class DBFacade
{
    private SqliteConnection _connection = null;
    
    public SqliteConnection GetConnection()
    {
        try
        {
            if (_connection == null) {
                _connection = new SqliteConnection("Data Source=./chirp.db");
            } 
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