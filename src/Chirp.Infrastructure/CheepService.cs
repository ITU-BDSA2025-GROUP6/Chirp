using Chirp.Infrastructure.Interfaces; 
namespace Chirp.Infrastructure;

public class CheepService : ICheepService
{
    private readonly ICheepRepository _repository;

    public CheepService(ICheepRepository repository)
    {
        _repository = repository;
    }

    public async Task<List<CheepDTO>> GetCheeps(int page)
    {
        return await _repository.GetCheeps(page);
    }
    
    public Task<int> CreateCheep(CheepDTO newMessage)
    {
        return _repository.CreateCheep(newMessage);
    }

    public Task<int> CreateAuthor(AuthorDTO newAuthor)
    {
        return _repository.CreateAuthor(newAuthor);
    }

    public Task<List<CheepDTO>> GetCheepsFromAuthor(string authorName, int page)
    {
        return _repository.GetCheepsFromAuthor(authorName, page);
    }

    public Task<int> UpdateCheep(CheepDTO alteredMessage)
    {
        return _repository.UpdateCheep(alteredMessage);
    }

    /*
    public Task<int> InsertAuthor(string username, string email)
    {
        return _repository.InsertAuthor(username, email);
    }
    */

}