using Chirp.Infrastructure;
using Chirp.Infrastructure.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Chirp.Web.Pages;

public class UserTimelinePaginationModel : PageModel
{
    private readonly ICheepService _cheepService;
    
    public Task<List<CheepDTO>> Cheeps { get; set; } = Task.FromResult(new List<CheepDTO>());
    public bool hasNextPage { get; set; }
    public int currentPage { get; set; }

    public UserTimelinePaginationModel(ICheepService cheepService)
    {
        _cheepService = cheepService;
    }
    
    public ActionResult OnGet(string author, int index)
    {
        currentPage = index < 1 ? 1 : index;
        Cheeps = _cheepService.GetCheeps(currentPage); 
        
        return Page();
    }
}