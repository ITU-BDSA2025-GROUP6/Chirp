namespace Chirp.Infrastructure.Interfaces;

public interface ICheepRepository
{
    Task<int> CreateCheep(CheepDTO newMessage);
    Task<int> CreateAuthor(AuthorDTO author);
    Task<List<CheepDTO>> GetCheeps(int page);
    Task<List<CheepDTO>> GetCheepsFromAuthor(string authorName, int page);
    Task<int> UpdateCheep(CheepDTO alteredMessage);
    Task<AuthorDTO> GetAuthorByName(string name);
    Task<AuthorDTO> GetAuthorByEmail(string email);

    //Task<int> InsertAuthor(string username, string email);


}