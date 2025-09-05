namespace SimpleDB;
using CsvHelper;
using System.Globalization;

public sealed class CsvDatabase<T> : IDatabaseRepository<T>
{
    string filePath = "../SimpleDB/Data/Cheep_DB.csv";
    public void Store(T input) {
        using (var writer = new StreamWriter((filePath), append:true))
        using (var csv = new CsvWriter(writer, CultureInfo.InvariantCulture))
        {
            if (writer.BaseStream.Length == 0) { csv.WriteHeader<T>(); }
            csv.NextRecord();
            csv.WriteRecord(input);
        }
    }
    
        
      
    public IEnumerable<T> Read(int? limit = null)
    {
        using var reader = new StreamReader(filePath );
        using var csv = new CsvReader(reader, CultureInfo.InvariantCulture);
        
        var records = csv.GetRecords<T>();
        return records.ToList();
    }
}




