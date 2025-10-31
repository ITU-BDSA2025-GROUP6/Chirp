namespace Chirp.Infrastructure.Interfaces;

public interface ICheepService
{
    Task<List<CheepDto>> GetCheeps(int page);
    Task<int> CreateCheep(CheepDto newMessage);
    Task<List<CheepDto>> GetCheepsFromAuthor(string authorName, int page);
    Task<int> UpdateCheep(CheepDto alteredMessage);
    Task<AuthorDto> GetAuthorByName(string name);
    Task<AuthorDto> GetAuthorByEmail(string email);
    

    //Task<int> InsertAuthor(string username, string email);
}
