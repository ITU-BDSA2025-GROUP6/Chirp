using SimpleDB;

var database = CsvDatabase<Cheep>.Instance;
var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

// .../cheeps skal hente alle cheeps fra CSV databasen som en liste af jsonobjekter
// .../cheep skal store det givne JSON object i vores simpleDB

app.MapGet("/cheeps", () => new Cheep("me", "Hej!", 1684229348));

// app.MapGet("/cheeps", () => database.Read().GetEnumerator().ToString());

app.Run();

public record Cheep(string Author, string Message, long Timestamp);