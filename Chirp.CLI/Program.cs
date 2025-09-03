// See https://aka.ms/new-console-template for more information
using Microsoft.VisualBasic.FileIO;
using CsvHelper;
using System.Globalization;

string filePath = System.IO.Path.GetFullPath("Data/chirp_cli_db.csv");

try
{ 
    if (args.Length > 0 && args[0] == "chirp")
        {
            string user_message = string.Join(" ", args.Skip(1));
            Console.WriteLine(user_message);
            writeCSV(user_message);
        }
    else
        {
            readCSV();
        } 
} catch (Exception e) { Console.WriteLine(e.Message); }

void writeCSV(String user_message)
{
    var input = new Cheep();
    { 
        input.user_name = Environment.UserName;
        input.user_message = user_message;
        input.unixTimeStamp = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
    };
    
    using (var writer = new StreamWriter((filePath), append:true))
    using (var csv = new CsvWriter(writer, CultureInfo.InvariantCulture))
    {
        if (writer.BaseStream.Length == 0) { csv.WriteHeader<Cheep>(); }
        csv.NextRecord();
        csv.WriteRecord(input);
    }
}

void readCSV()
{
    {
        using (var reader = new StreamReader(filePath))
        {
            using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
            {
                var records = csv.GetRecords<Cheep>();
            } 
        }
    }
}



public record Cheep
{
    public string user_name { get; set; }
    public string user_message { get; set; }
    public long unixTimeStamp { get; set; }
}


