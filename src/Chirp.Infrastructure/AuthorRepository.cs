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

    public async Task<AuthorDTO> GetAuthorByName(string? name)
    {

        if (string.IsNullOrWhiteSpace(name))
        {
            throw new InvalidOperationException("Name cannot be null or empty");
        }
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

    public async Task<AuthorDTO> GetAuthorByEmail(string? email)
    {
        if (string.IsNullOrWhiteSpace(email))
        {
            throw new InvalidOperationException("Email cannot be null or empty");
        }
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
}