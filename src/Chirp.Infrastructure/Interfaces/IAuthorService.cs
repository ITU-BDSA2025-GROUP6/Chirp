using Chirp.Core;

namespace Chirp.Infrastructure.Interfaces;

public interface IAuthorService
{
    Task<AuthorDTO> GetAuthorByName(string name);
    Task SaveChangesAsync();
    Task<AuthorDTO> GetAuthorByEmail(string email);
    Task<int> createRecheep(AuthorDTO Author, int cheepID);

    public Task<Author?> GetAuthorEntityByName(string name);
    
}