using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Chirp.Razor.Pages;

public class UserTimelineModel : PageModel
{
    private readonly ICheepService _service;
    public List<CheepViewModel> Cheeps { get; set; }

    public UserTimelineModel(ICheepService service)
    {
        _service = service;
    }

    public ActionResult OnGet(string author)
    {
        // 👇 Insert a specific cheep before fetching
        // You might map author → userId in your service, here I'll just hardcode 1
        _service.InsertCheep(100,"Automatic cheep: page was loaded!");

        // then fetch as normal
        Cheeps = _service.GetCheepsFromAuthor(author);

        return Page();
    }
}