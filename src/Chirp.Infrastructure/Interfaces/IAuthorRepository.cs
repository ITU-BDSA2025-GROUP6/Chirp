namespace Chirp.Infrastructure.Interfaces;

public interface IAuthorRepository
{
    Task<AuthorDTO> GetAuthorByName(string name);
    Task<AuthorDTO> GetAuthorByEmail(string email);
    Task<int> CreateRecheep(AuthorDTO Author, int cheepID);
}