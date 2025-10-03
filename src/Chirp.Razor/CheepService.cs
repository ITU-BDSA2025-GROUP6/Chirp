using System.Collections.Generic;
using Chirp.Razor;
using Microsoft.Data.Sqlite;
public record CheepViewModel(string Author, string Message, string Timestamp);

public interface ICheepService
{
    public List<CheepViewModel> GetCheeps();
    public List<CheepViewModel> GetCheeps(int page);
    public List<CheepViewModel> GetCheepsFromAuthor(string author);
}

public class CheepService : ICheepService
{
    // These would normally be loaded from a database for example
    private static readonly List<CheepViewModel> _cheeps = new()
        {
            new CheepViewModel("Helge", "Hello, BDSA students!", UnixTimeStampToDateTimeString(1690892208)),
            new CheepViewModel("Adrian", "Hej, velkommen til kurset.", UnixTimeStampToDateTimeString(1690895308)),
        };
    
    
    public List<CheepViewModel> GetCheeps()
    {
        DBFacade facade = new DBFacade();
        using SqliteConnection connection = facade.GetConnection();

        var sqlQuery = @"
        SELECT u.username, m.text, m.pub_date
        FROM user u inner join message m
        ON u.user_id = m.author_id
        ORDER by m.pub_date desc;";

        using var command = connection.CreateCommand();
        command.CommandText = sqlQuery;

        var cheeps = new List<CheepViewModel>();
        using var reader = command.ExecuteReader();
        while (reader.Read())
        {
            {
                string author = reader.GetString(0);
                string message = reader.GetString(1);
                double unixTime = reader.GetDouble(2);
                string timestamp = UnixTimeStampToDateTimeString(unixTime);

                cheeps.Add(new CheepViewModel(author, message, timestamp));
            }
        }

        return cheeps;
    }

    public List<CheepViewModel> GetCheepsFromAuthor(string author)
    {
        // filter by the provided author name
        DBFacade facade = new DBFacade();
        using SqliteConnection connection = facade.GetConnection();
        var sqlQuery = @"
        SELECT u.username, m.text, m.pub_date
        FROM user u inner join message m
        ON u.user_id = m.author_id
        WHERE u.username = @author
        ORDER by m.pub_date desc;";

        using var command = connection.CreateCommand();
        command.CommandText = sqlQuery;
        command.Parameters.AddWithValue("@author", author);

        var cheeps = new List<CheepViewModel>();
        using var reader = command.ExecuteReader();
        while (reader.Read())
        {
            {
                string auth = reader.GetString(0);
                string message = reader.GetString(1);
                double unixTime = reader.GetDouble(2);
                string timestamp = UnixTimeStampToDateTimeString(unixTime);

                cheeps.Add(new CheepViewModel(auth, message, timestamp));
            }
        }

        return cheeps;

        return _cheeps.Where(x => x.Author == author).ToList();
    }

    private static string UnixTimeStampToDateTimeString(double unixTimeStamp)
    {
        // Unix timestamp is seconds past epoch
        DateTime dateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
        dateTime = dateTime.AddSeconds(unixTimeStamp);
        return dateTime.ToString("MM/dd/yy H:mm:ss");
    }
    
    public List<CheepViewModel> GetCheeps(int page){
        {
            int pageSize = 10;
            int offset = (page - 1) * pageSize;
            var cheeps = new List<CheepViewModel>();
        
            DBFacade facade = new DBFacade();
            using SqliteConnection connection = facade.GetConnection();
            var command = connection.CreateCommand();

            command.CommandText = @"
            select u.username, m.text, m.pub_date
            from user u inner join message m
            on u.user_id = m.author_id
            order by m.pub_date desc
            limit $limit offset $offset
        ";
        
            command.Parameters.AddWithValue("$limit", pageSize);
            command.Parameters.AddWithValue("$offset", offset);
        
            using var reader = command.ExecuteReader();
            while (reader.Read()){
                string author = reader.GetString(0);
                string message = reader.GetString(1);
                double unixTime = reader.GetDouble(2);
                string timestamp = UnixTimeStampToDateTimeString(unixTime);

                cheeps.Add(new CheepViewModel(author, message, timestamp));
            }
            return cheeps;
        }
    }
}


