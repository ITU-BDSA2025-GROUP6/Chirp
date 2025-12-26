using Chirp.Infrastructure;
using Chirp.Infrastructure.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Chirp.Web.Pages;

public class UserTimelineModel : PageModel
{
    private readonly ICheepService _cheepService;
    private readonly IAuthorService _authorService;
    public List<CheepDTO> Cheeps { get; set; } = new List<CheepDTO>();
    public bool IsOwnTimeline { get; set; }

    public UserTimelineModel(ICheepService service,  IAuthorService authorService)
    {
        _cheepService = service;
        _authorService = authorService;
    }

    public async Task<ActionResult> OnGetAsync(string author)
    {
        int currentPage = 1;
        var currentUserName = User.Identity?.Name;
        
        IsOwnTimeline = currentUserName != null && currentUserName == author;
        if (IsOwnTimeline)
        {
            var currentUser = await _authorService.GetAuthorEntityByName(currentUserName!);
            if (currentUser != null)
            {
                Cheeps = await _cheepService.GetCheepsFromFollowedAuthor(currentUser.Id, currentPage);
            }
        }
        else
        {
            Cheeps = await _cheepService.GetCheepsFromAuthor(author, currentPage);
        }
        return Page();
    }
}
