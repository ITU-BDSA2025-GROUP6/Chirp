using Chirp.Infrastructure;
//using Microsoft.Data.Sqlite;
public record CheepViewModel(string Author, string Message, string Timestamp);

public interface ICheepService
{
    public List<CheepViewModel> GetCheeps();
    public List<CheepViewModel> GetCheeps(int page, int pageSize);
    public List<CheepViewModel> GetCheepsFromAuthor(string author);

    public List<CheepViewModel> GetCheepsFromAuthor(string author, int page, int pageSize);
}

public class CheepService : ICheepService
{
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
    }
    
    public void InsertMessage(int authorid, string text)
    {
        var facade = new DBFacade();
        using SqliteConnection connection = facade.GetConnection();
        long unixtime = DateTimeOffset.UtcNow.ToUnixTimeSeconds();

        const string sqlInsert = @"INSERT INTO message (author_id, text, pub_date)
                               VALUES (@author_id, @text, @pub_date);";
        using var command = connection.CreateCommand();
        command.CommandText = sqlInsert;
        command.Parameters.AddWithValue("@author_id", authorid);
        command.Parameters.AddWithValue("@text", text);
        command.Parameters.AddWithValue("@pub_date", unixtime);
        command.ExecuteNonQuery();
    }
    
    public void InsertUser(string username, string email)
    {
        var facade = new DBFacade();
        using SqliteConnection connection = facade.GetConnection();

        const string sqlInsert = "INSERT INTO user (username, email) VALUES (@username, @email);";
        using var command = connection.CreateCommand();
        command.CommandText = sqlInsert;
        command.Parameters.AddWithValue("@username", username);
        command.Parameters.AddWithValue("@email", email);
        command.ExecuteNonQuery();
    }


    public int getIDfromEmail(string email)
    {
        DBFacade facade = new DBFacade();
        using SqliteConnection connection = facade.GetConnection();
        var sqlQuery = @"SELECT u.user_id from user u where u.email = @email;";

        using var command = connection.CreateCommand();
        command.CommandText = sqlQuery;
        command.Parameters.AddWithValue("@email", email);

        using var reader = command.ExecuteReader();
        while (reader.Read())
        {
            {
                return reader.GetInt32(0);
            }
        }
        return 0;
    }




    private static string UnixTimeStampToDateTimeString(double unixTimeStamp)
    {
        // Unix timestamp is seconds past epoch
        DateTime dateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
        dateTime = dateTime.AddSeconds(unixTimeStamp);
        return dateTime.ToString("MM/dd/yy H:mm:ss");
    }
    
    public List<CheepViewModel> GetCheeps(int page, int pageSize){
        {
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
    
    public List<CheepViewModel> GetCheepsFromAuthor(string author, int page, int pageSize)
    {
        int offset = (page - 1) * pageSize;
        
        // filter by the provided author name
        DBFacade facade = new DBFacade();
        using SqliteConnection connection = facade.GetConnection();
        var sqlQuery = @"
        SELECT u.username, m.text, m.pub_date
        FROM user u inner join message m
        ON u.user_id = m.author_id
        WHERE u.username = @author
        ORDER by m.pub_date desc
        limit $limit offset $offset";
        
   
        
        using var command = connection.CreateCommand();
        command.CommandText = sqlQuery;
        
        command.Parameters.AddWithValue("@author", author);
        command.Parameters.AddWithValue("$limit", pageSize);
        command.Parameters.AddWithValue("$offset", offset);

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
    }
}


