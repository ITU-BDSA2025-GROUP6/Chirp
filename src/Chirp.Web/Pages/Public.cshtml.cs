using System.ComponentModel.DataAnnotations;
using System.Net.Mime;
using Chirp.Core;
using Chirp.Infrastructure;
using Chirp.Infrastructure.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Chirp.Web.Pages;

public class PublicModel : PageModel
{
    private readonly ICheepService _service;
    private readonly IAuthorService _authorService;
    public Task<List<CheepDTO>> Cheeps { get; set; } = Task.FromResult(new List<CheepDTO>());
    
    public Task<List<Author>> Followers { get; set; }
    
    [BindProperty]
    [StringLength(160, ErrorMessage = "The {0} must be at max {1} characters long.")]
    public string Text { get; set; } = string.Empty;
    public PublicModel(ICheepService service,  IAuthorService authorService)
    {
        _service = service;
        _authorService = authorService;
    }

    public ActionResult OnGet()
    {
        int currentpage = 1;
        Cheeps = _service.GetCheeps(currentpage);
        return Page();
    }

    public IActionResult OnPost()
    {
        if (!ModelState.IsValid)
        {
            Cheeps = _service.GetCheeps(1);
            return Page();
        }
        if (!(User.Identity?.IsAuthenticated ?? false ))
        {
            //return Redirect("/login");
            return Redirect("/");
        }
        // Lille fail safe ift. nullable names
        var authorName = User.Identity?.Name;
        if (string.IsNullOrEmpty(authorName)) return Redirect("/");
        
        CheepDTO newcheep = new CheepDTO // not 100% but should automaticlly incremnt the id, check if this is the case.
        {
            AuthorName = authorName,
            Text = Text,
            Timestamp = DateTime.UtcNow
        };
        _service.CreateCheep(newcheep);
        
        
        return Redirect("/");
    }

    
    public async Task<IActionResult> OnGetFollowBtn(string authorName)
    {
        if (!User.Identity.IsAuthenticated)
        {
            return Redirect("/login"); 
        }

        AuthorDTO user = await _authorService.GetAuthorByName(User.Identity.Name);
        AuthorDTO follower = await _authorService.GetAuthorByName(authorName);

        if (user == null || follower == null)
        {
            return NotFound();
        }

        if (user.Id != follower.Id && !user.Following.Contains(follower))
        {
            user.Following.Add(follower);
        }
        
        return RedirectToPage();
    }
    
}
