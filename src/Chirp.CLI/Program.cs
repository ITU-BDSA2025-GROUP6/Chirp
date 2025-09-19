using Chirp.CLI;
using SimpleDB;
using DocoptNet;

const string Help = @"Chirp
Usage: 
  chirp cheep <message>...
  chirp list
  chirp (-h | --help)
  chirp --version

Options:
  -h --help             Show help.
  --version             Show version.
  -- chirp <message>... Message to chirp.
";

var database = CsvDatabase<Cheep>.Instance;

try
{
    var arguments = new Docopt().Apply(Help, args, version: "Chirp", exit: true);
    if (arguments["cheep"].IsTrue)
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
            unixTimeStamp = DateTimeOffset.UtcNow.ToUnixTimeSeconds()
        };
        database.Store(cheep);
        Console.WriteLine($"Cheep posted: {userMessage}");
    }
    
    else if (arguments["list"].IsTrue)
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

//test
