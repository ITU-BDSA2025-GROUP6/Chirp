namespace Chirp.Infrastructure.Interfaces;

public interface ICheepService
{
    Task<List<CheepDTO>> GetCheeps(int page);
    Task<List<CheepDTO>> GetCheeps(int page, string? currentUserId);
    Task<int> CreateCheep(CheepDTO newMessage);
    Task<List<CheepDTO>> GetCheepsFromAuthor(string authorName, int page);
    Task<int> UpdateCheep(CheepDTO alteredMessage);
    Task<bool>  DeleteCheep(int cheepId, string authorName);
    Task<List<CheepDTO>> GetCheepsFromFollowedAuthor(string userId, int page);
    
    

    //Task<int> InsertAuthor(string username, string email);
}
