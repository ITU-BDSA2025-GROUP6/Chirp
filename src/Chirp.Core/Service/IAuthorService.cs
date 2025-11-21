namespace Chirp.Infrastructure.Interfaces;

public interface IAuthorService
{
    Task<AuthorDTO> GetAuthorByName(string name);
    Task<AuthorDTO> GetAuthorByEmail(string email);
}