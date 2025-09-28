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
                var basePath = AppContext.BaseDirectory;
                var dbPath = Path.Combine(basePath, "chirp.db");
                _connection = new SqliteConnection($"Data Source={dbPath}");
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