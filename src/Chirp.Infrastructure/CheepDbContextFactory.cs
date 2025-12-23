using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Chirp.Infrastructure;

public class CheepDbContextFactory : IDesignTimeDbContextFactory<CheepDBContext>
{
    public CheepDBContext CreateDbContext(string[] args)
    {

        var provider = "sqlite"; //default

        for (int i = 0; i < args.Length; i++)
        {
            if (args[i] == "--provider" && i + 1 < args.Length)
            {
                provider = args[i + 1].ToLower();
                break;
            }
        }

        var envProvider = Environment.GetEnvironmentVariable("EF_PROVIDER");
        if (!string.IsNullOrEmpty(envProvider))
        {
            provider = envProvider.ToLower();
        }

        var optionsBuilder = new DbContextOptionsBuilder<CheepDBContext>();

        if (provider == "sqlserver")
        {
            // For migration generation, we just need a valid connection string format
            // The actual connection doesn't need to work
            optionsBuilder.UseSqlServer(
                "Server=. ;Database=Chirp;Trusted_Connection=True;",
                x => x.MigrationsAssembly("Chirp.Infrastructure")
                    .MigrationsHistoryTable("__EFMigrationsHistory", "dbo"));
        }
        else
        {
            optionsBuilder.UseSqlite(
                "Data Source=Chirp.db",
                x => x.MigrationsAssembly("Chirp.Infrastructure"));
        }

        return new CheepDBContext(optionsBuilder.Options);
    }
}