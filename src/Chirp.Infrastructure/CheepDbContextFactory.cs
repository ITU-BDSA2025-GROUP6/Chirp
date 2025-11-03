using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Chirp.Infrastructure;

public class CheepDbContextFactory : IDesignTimeDbContextFactory<CheepDBContext>
{
    public CheepDBContext CreateDbContext(string[] args)
    {
        var options = new DbContextOptionsBuilder<CheepDBContext>()
            .UseSqlite("Data Source=Chirp.db")
            .Options;
        
        return new CheepDBContext(options);
    }
}