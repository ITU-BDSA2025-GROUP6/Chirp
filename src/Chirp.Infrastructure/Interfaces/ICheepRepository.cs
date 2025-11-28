namespace Chirp.Infrastructure.Interfaces;

public interface ICheepRepository
{
    Task<int> CreateCheep(CheepDTO newMessage);
    //Task<int> CreateAuthor(AuthorDTO author);
    Task<List<CheepDTO>> GetCheeps(int page);
    Task<List<CheepDTO>> GetCheepsFromAuthor(string authorName, int page);
    Task<int> UpdateCheep(CheepDTO alteredMessage);
    Task<bool>  DeleteCheep(int cheepId, string authorName);
    
    //Task<int> InsertAuthor(string username, string email);


}