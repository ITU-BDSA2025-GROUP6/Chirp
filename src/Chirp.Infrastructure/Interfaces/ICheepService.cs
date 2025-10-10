namespace Chirp.Infrastructure;

public interface ICheepService
{
    public List<CheepViewModel> GetCheeps();
    public List<CheepViewModel> GetCheeps(int page, int pageSize);
    public List<CheepViewModel> GetCheepsFromAuthor(string author);

    public List<CheepViewModel> GetCheepsFromAuthor(string author, int page, int pageSize);
}
