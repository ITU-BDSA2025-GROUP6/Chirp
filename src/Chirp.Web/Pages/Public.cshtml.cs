using System.Net.Mime;
using Chirp.Infrastructure;
using Chirp.Infrastructure.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Chirp.Razor.Pages;

public class PublicModel : PageModel
{
    private readonly ICheepService _service;
    public Task<List<CheepDTO>> Cheeps { get; set; }
    
    [BindProperty]
    public string Text { get; set; }
    public PublicModel(ICheepService service)
    {
        _service = service;
    }

    public ActionResult OnGet()
    {
        int currentpage = 1;
        Cheeps = _service.GetCheeps(currentpage);
        return Page();
    }

    public IActionResult OnPost()
    {
        if (!(User.Identity?.IsAuthenticated ?? false ))
        {
            return Redirect("/login");
        }
        
        CheepDTO newcheep = new CheepDTO // not 100% but should automaticlly incremnt the id, check if this is the case.
        {
            AuthorName = User.Identity.Name,
            Text = Text,
            Timestamp = DateTime.UtcNow
        };
        _service.CreateCheep(newcheep);
        
        
        return Redirect("/");
    }
}
