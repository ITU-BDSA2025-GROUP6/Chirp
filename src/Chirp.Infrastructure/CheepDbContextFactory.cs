using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Chirp.Infrastructure;

public class CheepDbContextFactory : IDesignTimeDbContextFactory<CheepDBContext>
{
    public CheepDBContext CreateDbContext(string[] args)
    {
        var options = new DbContextOptionsBuilder<CheepDBContext>()
            .UseSqlServer("Server=localhost;Database=ChirpDesignTime;Trusted_Connection=True;TrustServerCertificate=True")
            .Options;
        
        return new CheepDBContext(options);
    }
}