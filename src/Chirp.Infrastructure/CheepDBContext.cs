using Chirp.Core;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;


namespace Chirp.Infrastructure;


public class CheepDBContext : IdentityDbContext<Author>
{
    public DbSet<Cheep> Cheeps { get; set; }
    public DbSet<Author> Authors { get; set; }
    
    public DbSet<Follows> Follows { get; set; }
    
    public CheepDBContext(DbContextOptions<CheepDBContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        //many to many relation
    }
}
