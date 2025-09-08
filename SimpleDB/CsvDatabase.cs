namespace SimpleDB;
using CsvHelper;
using System.Globalization;

/// <summary>
/// Represents a simple CSV-backed database for storing and retrieving records of type <typeparamref name="T"/>.
/// </summary>
/// <typeparam name="T">
/// The type of the objects to be stored and retrieved from the CSV database.
/// </typeparam>
public sealed class CsvDatabase<T> : IDatabaseRepository<T>
{
    string filePath = "../SimpleDB/Data/Cheep_DB.csv";
    /// <summary>
    /// Stores a record of type <typeparamref name="T"/> in the underlying CSV database.
    /// </summary>
    /// <param name="input">The record of type <typeparamref name="T"/> to be stored in the database.</param>
    public void Store(T input)
    {
        using (var writer = new StreamWriter((filePath), append:true))
        using (var csv = new CsvWriter(writer, CultureInfo.InvariantCulture))
        {
            if (writer.BaseStream.Length == 0) { csv.WriteHeader<T>(); }
            csv.NextRecord();
            csv.WriteRecord(input);
        }
    }
    
    
    /// <summary>
    /// Inumerates through the records  in the CSV database and returns them as a 
    /// </summary>
    /// <param name="limit"></param>
    /// <returns></returns>
    public IEnumerable<T> Read(int? limit = null)
    {
        using var reader = new StreamReader(filePath );
        using var csv = new CsvReader(reader, CultureInfo.InvariantCulture);
        var records = csv.GetRecords<T>();
        return records.ToList();
    }
}




