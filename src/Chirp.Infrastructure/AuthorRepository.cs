using Chirp.Core;
using Chirp.Infrastructure.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Chirp.Infrastructure;

public class AuthorRepository : IAuthorRepository
{
    private readonly CheepDbContext _dbContext;

    public AuthorRepository(CheepDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<AuthorDTO> GetAuthorByName(string name)
    {
        try
        {
            var author = await _dbContext.Authors
                .Include(a => a.Cheeps)
                .FirstAsync(a => a.UserName == name);

            return new AuthorDTO
            {
                Id = author.Id,
                Name = author.UserName ?? string.Empty,
                Email = author.Email ?? string.Empty,
                Cheeps = author.Cheeps
            };
        }
        catch (InvalidOperationException)
        {
            throw new InvalidOperationException("No such author with name: " + name);
        }
    }

    public async Task<AuthorDTO> GetAuthorByEmail(string email)
    {
        try
        {
            var author = await _dbContext.Authors
                .Include(a => a.Cheeps)
                .FirstAsync(a => a.Email == email);

            return new AuthorDTO
            {
                Id = author.Id,
                Name = author.UserName ?? string.Empty,
                Email = author.Email ?? string.Empty,
                Cheeps = author.Cheeps
            };
        }
        
        catch (InvalidOperationException)
        {
            throw new InvalidOperationException("No such author with email: " + email);
        }
    }
    public async Task<int> CreateRecheep(AuthorDTO author, int cheepID)
    {
        if (author == null)
            throw new InvalidOperationException("No such author");

        var existing = await _dbContext.Recheeps
            .FirstOrDefaultAsync(r => r.AuthorID == author.Id && r.CheepID == cheepID);

        if (existing != null)
        {
            _dbContext.Recheeps.Remove(existing);
            await _dbContext.SaveChangesAsync();
            return cheepID;
        }

        var newRecheep = new Recheep
        {
            AuthorID = author.Id,
            CheepID = cheepID
        };

        await _dbContext.Recheeps.AddAsync(newRecheep);
        await _dbContext.SaveChangesAsync();
        return cheepID;
    }
}