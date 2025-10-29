namespace Chirp.Infrastructure.Interfaces;

public interface ICheepService
{
    Task<List<CheepDTO>> GetCheeps(int page);
    Task<int> CreateCheep(CheepDTO newMessage);
    Task<List<CheepDTO>> GetCheepsFromAuthor(string authorName, int page);
    Task<int> UpdateCheep(CheepDTO alteredMessage);
    Task<AuthorDTO> GetAuthorByName(string name);
    Task<AuthorDTO> GetAuthorByEmail(string email);
    

    //Task<int> InsertAuthor(string username, string email);
}
