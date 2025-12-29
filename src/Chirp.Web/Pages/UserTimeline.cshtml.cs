using Chirp.Infrastructure;
using Chirp.Infrastructure.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Chirp.Web.Pages;

public class UserTimelineModel : PageModel
{
    private readonly ICheepService _cheepService;
    private readonly IAuthorService _authorService;

    public List<CheepDTO> Cheeps { get; set; } = new();
    public bool IsOwnTimeline { get; set; }

    public int CurrentPage { get; set; } = 1;
    public bool HasNextPage { get; set; }

    public UserTimelineModel(ICheepService service, IAuthorService authorService)
    {
        _cheepService = service;
        _authorService = authorService;
    }

    public async Task<IActionResult> OnGetAsync(string author)
    {
        CurrentPage = 1;

        var currentUserName = User.Identity?.Name;
        IsOwnTimeline = currentUserName != null && currentUserName == author;

        if (IsOwnTimeline)
        {
            var currentUser = await _authorService.GetAuthorEntityByName(currentUserName!);
            if (currentUser != null)
            {
                Cheeps = await _cheepService.GetCheepsFromFollowedAuthor(currentUser.Id, CurrentPage);

                // Determine if there is a next page
                HasNextPage = (await _cheepService
                    .GetCheepsFromFollowedAuthor(currentUser.Id, CurrentPage + 1)).Any();
            }
        }
        else
        {
            Cheeps = await _cheepService.GetCheepsFromAuthor(author, CurrentPage);

            HasNextPage = (await _cheepService
                .GetCheepsFromAuthor(author, CurrentPage + 1)).Any();
        }

        return Page();
    }
}