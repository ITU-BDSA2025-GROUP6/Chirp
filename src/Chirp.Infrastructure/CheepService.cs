using Chirp.Infrastructure.Interfaces; 
namespace Chirp.Infrastructure;

public class CheepService : ICheepService
{
    private readonly ICheepRepository _repository;

    public CheepService(ICheepRepository repository)
    {
        _repository = repository;
    }

    public async Task<List<CheepDto>> GetCheeps(int page)
    {
        return await _repository.GetCheeps(page);
    }
    
    public Task<int> CreateCheep(CheepDto newMessage)
    {
        return _repository.CreateCheep(newMessage);
    }

    public Task<int> CreateAuthor(AuthorDto newAuthor)
    {
        return _repository.CreateAuthor(newAuthor);
    }

    public Task<List<CheepDto>> GetCheepsFromAuthor(string authorName, int page)
    {
        return _repository.GetCheepsFromAuthor(authorName, page);
    }

    public Task<int> UpdateCheep(CheepDto alteredMessage)
    {
        return _repository.UpdateCheep(alteredMessage);
    }

    public Task<AuthorDto> GetAuthorByName(string name)
    {
        return _repository.GetAuthorByName(name);
    }
    public Task<AuthorDto> GetAuthorByEmail(string email)
    {
        return _repository.GetAuthorByEmail(email);
    }
    

    /*
    public Task<int> InsertAuthor(string username, string email)
    {
        return _repository.InsertAuthor(username, email);
    }
    */

}