// See https://aka.ms/new-console-template for more information
using Microsoft.VisualBasic.FileIO;
using CsvHelper;
using System.Globalization;
using SimpleDB;

var database = new CsvDatabase<Cheep>();

try
{ 
    if (args.Length > 0 && args[0] == "chirp")
    {
        string user_message = "";
            for (int i = 1; i < args.Length; i++)
            {
                user_message += args[i] +" ";
            }
            var cheep = new Cheep();
            { 
                cheep.user_name = Environment.UserName;
                cheep.user_message = user_message.Trim();
                cheep.unixTimeStamp = DateTimeOffset.Now.ToUnixTimeSeconds();
                //cheep.unixTimeStamp = DateTimeOffset.UtcNow.ToLocalTime().AddHours(2).ToUnixTimeSeconds();
            };
            database.Store(cheep);
    }
    else
    {
        foreach (var r in database.Read())
        {
            Console.WriteLine($"{r.user_name} @ {DateTimeOffset.FromUnixTimeSeconds(r.unixTimeStamp).DateTime} : {r.user_message}  ");
        }
    }
    
    
} catch (Exception e) { Console.WriteLine(e.Message); }

public record Cheep
{
    public string user_name { get; set; }
    public string user_message { get; set; }
    public long unixTimeStamp { get; set; }
}