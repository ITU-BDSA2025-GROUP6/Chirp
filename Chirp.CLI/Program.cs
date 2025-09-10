using System.Text.RegularExpressions;
using Chirp.CLI;
using SimpleDB;
using DocoptNet;

try
{
    var database = new CsvDatabase<Cheep>();
    if (args.Length > 0 && args[0] == "chirp")
    {
        string user_message = "";
        for (int i = 1; i < args.Length; i++)
        {
            user_message += args[i] + " ";
        }

        var cheep = new Cheep();
        {
            cheep.user_name = Environment.UserName;
            cheep.user_message = user_message.Trim();
            cheep.unixTimeStamp = DateTimeOffset.UtcNow.ToLocalTime().AddHours(2).ToUnixTimeSeconds();
        }
        ;
        database.Store(cheep);
    }
    else
    {
        UserInterface.PrintCheeps(database.Read());
    }


}
catch (Exception e)
{
    Console.WriteLine(e.Message);
}

return 0;


public record Cheep
{
    public string user_name { get; set; }
    public string user_message { get; set; }
    public long unixTimeStamp { get; set; }
}

