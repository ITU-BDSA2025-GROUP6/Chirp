using SimpleDB;

var database = CsvDatabase<Cheep>.Instance;
var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

// .../cheeps skal hente alle cheeps fra CSV databasen som en liste af jsonobjekter
// .../cheep skal store det givne JSON object i vores simpleDB

// app.MapGet("/cheeps", () => new Cheep("me", "Hej!", 1684229348));

app.MapPost("/cheep", (Cheep cheep) => {database.Store(cheep);});

app.MapGet("/cheeps", () => database.Read());

app.Run();

public record Cheep
{
    public string user_name { get; set; }
    public string user_message { get; set; }
    public long unixTimeStamp { get; set; }
}