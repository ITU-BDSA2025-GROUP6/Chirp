using Chirp.Core;
using Chirp.Infrastructure.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Chirp.Infrastructure;

public class AuthorService : IAuthorService
{
    private readonly IAuthorRepository _repository;
    private readonly CheepDBContext _context;

    public AuthorService(IAuthorRepository repository, CheepDBContext context)
    {
        _repository = repository;
        _context = context;
    }

    public Task<AuthorDTO> GetAuthorByName(string name)
    {
        return _repository.GetAuthorByName(name);
    }
    public Task<AuthorDTO> GetAuthorByEmail(string email)
    {
        return _repository.GetAuthorByEmail(email);
    }
    
    
    public Task<Author?> GetAuthorEntityByName(string name)
    {
        return _context.Authors
            .Include(a => a.Following)
            .ThenInclude(f => f.FollowedByAuthor)
            .FirstOrDefaultAsync(a => a.UserName == name);
    }
    
    public Task SaveChangesAsync()
    {
        return _context.SaveChangesAsync();
    }

    public async Task<int> CreateRecheep(AuthorDTO Author, int cheepID) {
        return _repository.CreateRecheep(Author, cheepID);
    }

}