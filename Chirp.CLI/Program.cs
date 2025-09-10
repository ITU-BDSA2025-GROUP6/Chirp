using System.Text.RegularExpressions;
using Chirp.CLI;
using SimpleDB;
using DocoptNet;

const string Help = @"Chirp
Usage:
  cheep chirp <message>...
  cheep list
  cheep (-h | --help)
  cheep --version

Options:
  -h --help             Show help.
  --version             Show version.
  -- chirp <message>... Message to chirp.
";

var database = new CsvDatabase<Cheep>();

try
{
    var arguments = new Docopt().Apply(Help, args, version: "Chirp", exit: true);
    if (arguments["chirp"].IsTrue)
    {
        var messageValue = arguments["<message>"]?.Value;

        string userMessage;
        if (messageValue is System.Collections.IEnumerable messageWords)
        {
            var words = new List<string>();
            foreach (var word in messageWords)
            {
                words.Add(word.ToString()!);
            }

            userMessage = string.Join(" ", words);
        }
        else
        {
            userMessage = messageValue.ToString()!;
        }

        var cheep = new Cheep()
        {
            user_name = Environment.UserName,
            user_message = userMessage.Trim(),
            unixTimeStamp = DateTimeOffset.UtcNow.ToLocalTime().AddHours(2).ToUnixTimeSeconds(),
        } ;
        database.Store(cheep);
        Console.WriteLine($"Cheep posted: {userMessage}");
    }
    
    else if (arguments["list"].IsTrue)
    {
        foreach (var cheep in database.Read())
        {
            Console.WriteLine(
                $"{cheep.user_name} @ {DateTimeOffset.FromUnixTimeSeconds(cheep.unixTimeStamp).DateTime}: {cheep.user_message}");
        }
    }
}

catch (Exception e)
{
    Console.WriteLine(e.Message);
}


public record Cheep
{
    public string user_name { get; set; }
    public string user_message { get; set; }
    public long unixTimeStamp { get; set; }
}

