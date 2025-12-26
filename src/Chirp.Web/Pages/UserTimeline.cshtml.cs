using Chirp.Infrastructure;
using Chirp.Infrastructure.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Chirp.Web.Pages;

public class UserTimelineModel : PageModel
{
    private readonly ICheepService _service;
    public Task<List<CheepDTO>> Cheeps { get; set; } = Task.FromResult(new List<CheepDTO>());

    public UserTimelineModel(ICheepService service)
    {
        _service = service;
    }

    public ActionResult OnGet(string author)
    {
        int currentPage = 1;
        Cheeps = _service.GetCheepsFromAuthor(author, currentPage);
        
        return Page();
    }
}
