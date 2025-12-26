using Chirp.Infrastructure;
using Chirp.Infrastructure.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Chirp.Web.Pages;

public class UserTimelinePaginationModel : PageModel
{
    private readonly ICheepService _cheepService;
    private readonly IAuthorService _authorService;
    
    public List<CheepDTO> Cheeps { get; set; } = new List<CheepDTO>();
    public bool hasNextPage { get; set; }
    public int currentPage { get; set; }
    public bool IsOwnTimeline { get; set; }

    public UserTimelinePaginationModel(ICheepService cheepService,  IAuthorService authorService)
    {
        _cheepService = cheepService;
        _authorService = authorService;
    }
    
    public async Task<ActionResult> OnGetAsync(string author, int index)
    {
        currentPage = index < 1 ? 1 : index;
        var currentUserName = User.Identity?.Name;
        
        IsOwnTimeline = currentUserName != null && currentUserName == author;

        if (IsOwnTimeline)
        {
            var currentUser = await _authorService.GetAuthorEntityByName(currentUserName!);
            if (currentUser != null)
            {
                Cheeps = await _cheepService.GetCheepsFromFollowedAuthor(currentUser.Id, currentPage);
            }
            else
            {
                Cheeps = await _cheepService.GetCheepsFromFollowedAuthor(author, currentPage);
            }
            
        }
        return Page();
    }
}