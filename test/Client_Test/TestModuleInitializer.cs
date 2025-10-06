using System;
using System.IO;
using System.Runtime.CompilerServices;

internal static class TestModuleInitializer
{
    [ModuleInitializer]
    public static void Init()
    {
        // go up from bin/Debug/net8.0 to test/Client_Test/, then to test/, then to solution root, then into src/Chirp.SQLite
        var dbPath = Path.GetFullPath(Path.Combine(
            AppContext.BaseDirectory,
            "..", "..", "..", "..", "..",
            "src", "Chirp.SQLite", "chirp.db"));

        Environment.SetEnvironmentVariable("CHIRPDBPATH", dbPath);
        Console.WriteLine($"[MODULE INIT] CHIRPDBPATH = {dbPath}; Exists = {File.Exists(dbPath)}");
    }
}