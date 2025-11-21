using Chirp.Infrastructure.Interfaces;

namespace Chirp.Infrastructure;

public class AuthorService : IAuthorService
{
    private readonly IAuthorRepository _repository;

    
    public Task<AuthorDTO> GetAuthorByName(string name)
    {
        return _repository.GetAuthorByName(name);
    }
    public Task<AuthorDTO> GetAuthorByEmail(string email)
    {
        return _repository.GetAuthorByEmail(email);
    }
}