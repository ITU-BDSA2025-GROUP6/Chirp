namespace Chirp.Infrastructure.Interfaces;

public interface ICheepRepository
{
    Task<int> CreateCheep(CheepDto newMessage);
    Task<int> CreateAuthor(AuthorDto author);
    Task<List<CheepDto>> GetCheeps(int page);
    Task<List<CheepDto>> GetCheepsFromAuthor(string authorName, int page);
    Task<int> UpdateCheep(CheepDto alteredMessage);
    Task<AuthorDto> GetAuthorByName(string name);
    Task<AuthorDto> GetAuthorByEmail(string email);

    //Task<int> InsertAuthor(string username, string email);


}