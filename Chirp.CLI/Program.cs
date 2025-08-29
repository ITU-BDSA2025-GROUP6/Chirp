// See https://aka.ms/new-console-template for more information

List<string> cheeps = new() { "Hello everynyan!", "Welcome to sloppity slop course!", "I miss summer." };

foreach (var cheep in cheeps) {
    Console.WriteLine(cheep);
    Thread.Sleep(1000);
}