using Microsoft.AspNetCore.Mvc.ModelBinding.Metadata;

namespace Chirp.Razor;
using Microsoft.Data.Sqlite;

public class DBFacade
{
    private SqliteConnection connection = null;

    DBFacade()
    {
        connection = new SqliteConnection("Data Source=./chirp.db");
    }

    public SqliteConnection getConnection()
    {
        try
        {
            connection.Open();
            return connection;
        }
        catch (Exception e)
        {
            Console.WriteLine("Could not retrieve the database, error: " + e.GetBaseException());
        }
        return null;
    }
}