using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Chirp.Razor.Pages;

public class UserTimelinePaginationModel : PageModel
{
    private readonly ICheepService _cheepService;
    
    public List<CheepViewModel> Cheeps { get; set; } = new();

    public UserTimelinePaginationModel(ICheepService cheepService)
    {
        _cheepService = cheepService;
    }
    
    public ActionResult OnGet(string author, int index)
    {
        var currentPage = index < 1 ? 1 : index;
        Cheeps = _cheepService.GetCheepsFromAuthor(author, currentPage);
        return Page();
    }
}