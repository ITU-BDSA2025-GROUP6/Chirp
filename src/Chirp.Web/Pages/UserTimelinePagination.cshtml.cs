using Chirp.Infrastructure;
using Chirp.Infrastructure.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Chirp.Razor.Pages;

public class UserTimelinePaginationModel : PageModel
{
    private readonly ICheepService _cheepService;
    
    public Task<List<CheepDTO>> Cheeps { get; set; }
    public bool hasNextPage { get; set; }
    public int currentPage { get; set; }

    public UserTimelinePaginationModel(ICheepService cheepService)
    {
        _cheepService = cheepService;
    }
    
    public ActionResult OnGet(string author, int index)
    {
        int pageSize = 32;
        currentPage = index < 1 ? 1 : index;
        var cheeps = _cheepService.GetCheepsFromAuthor(author, currentPage);
        
        //hasNextPage = cheeps.Count > pageSize;
        //Cheeps = cheeps.Take(pageSize).ToList();
        
        return Page();
    }
}