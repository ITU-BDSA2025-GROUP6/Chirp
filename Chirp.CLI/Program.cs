// See https://aka.ms/new-console-template for more information
using Microsoft.VisualBasic.FileIO;
using System;
using System.Globalization;

try
{
    string filePath = System.IO.Path.GetFullPath("Data/chirp_cli_db.csv");
    using(var parser = new TextFieldParser(filePath))
    {
        parser.SetDelimiters(",");                     
        parser.HasFieldsEnclosedInQuotes = true;  
        
        // remove first line
        String[] header = parser.ReadFields();
        
        while (!parser.EndOfData)
        {
            var line = parser.ReadFields();
            String author = line[0];
            String message = line[1];
            var date  = DateTimeOffset.FromUnixTimeSeconds(long.Parse(line[2])).DateTime;
            
            Console.WriteLine(author + " @ " + date + ": " + message);
        }
    }
    // for writing to the csv
    //using (var writer = new StreamWriter(filePath))
    //{
    //    writer.WriteLine("Test");
    //}
    
} catch (Exception e)
{
    Console.WriteLine(e.Message);
}


