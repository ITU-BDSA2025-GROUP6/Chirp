namespace Chirp.Infrastructure.Interfaces;

public interface ICheepService
{
    Task<List<CheepDTO>> GetCheeps(int page);
    Task<int> CreateCheep(CheepDTO newMessage);
    Task<List<CheepDTO>> GetCheepsFromAuthor(string authorName, int page);
    Task<int> UpdateCheep(CheepDTO alteredMessage);
    Task<bool>  DeleteCheep(int cheepId, string authorName);

    //Task<int> InsertAuthor(string username, string email);
}
