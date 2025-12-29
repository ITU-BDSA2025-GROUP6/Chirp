using System.ComponentModel.DataAnnotations;
using Chirp.Core;
using Chirp.Infrastructure;
using Chirp.Infrastructure.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Chirp.Web.Pages;

public class PaginationModel : PageModel
{
    private readonly ICheepService _service;
    private readonly IAuthorService _authorService;

    // materialized list
    public List<CheepDTO> Cheeps { get; set; } = new();

    public bool hasNextPage { get; set; }
    public int currentPage { get; set; }

    public PaginationModel(ICheepService service, IAuthorService authorService)
    {
        _service = service;
        _authorService = authorService;
    }

    public async Task<IActionResult> OnGetAsync(int index = 1)
    {
        currentPage = index < 1 ? 1 : index;

        string? currentUserId = await GetCurrentUserIdAsync();

        Cheeps = await _service.GetCheeps(currentPage, currentUserId) ?? new List<CheepDTO>();

        var nextPageCheeps = await _service.GetCheeps(currentPage + 1, currentUserId);
        hasNextPage = nextPageCheeps != null && nextPageCheeps.Any();

        return Page();
    }

    public async Task<IActionResult> OnPostRecheepAsync(int cheepId)
    {
        if (User.Identity?.IsAuthenticated != true)
            return RedirectToPage();

        var authorName = User.Identity?.Name;
        if (string.IsNullOrEmpty(authorName))
            return RedirectToPage();

        var author = await _authorService.GetAuthorByName(authorName);
        if (author == null)
            return RedirectToPage();

        await _authorService.createRecheep(new AuthorDTO { Id = author.Id }, cheepId);
        return RedirectToPage();
    }

    public async Task<IActionResult> OnPostDeleteAsync(int cheepId)
    {
        if (User.Identity?.IsAuthenticated != true)
            return RedirectToPage();

        var authorName = User.Identity?.Name;
        if (string.IsNullOrEmpty(authorName))
            return RedirectToPage();

        await _service.DeleteCheep(cheepId, authorName);
        return RedirectToPage();
    }

    public async Task<IActionResult> OnGetFollowBtnAsync(string authorName)
    {
        var username = User.Identity?.Name;
        if (string.IsNullOrEmpty(username))
            return RedirectToPage();

        var currentUser = await _authorService.GetAuthorEntityByName(username);
        var followTarget = await _authorService.GetAuthorEntityByName(authorName);

        if (currentUser == null || followTarget == null ||
            currentUser.Id == followTarget.Id)
            return RedirectToPage();

        bool alreadyFollow =
            currentUser.Following.Any(f => f.FollowedById == followTarget.Id);

        if (!alreadyFollow)
        {
            currentUser.Following.Add(new Follows
            {
                FollowsId = currentUser.Id,
                FollowedById = followTarget.Id
            });
        }
        else
        {
            currentUser.Following.RemoveAll(
                f => f.FollowedById == followTarget.Id);
        }

        await _authorService.SaveChangesAsync();
        return RedirectToPage();
    }

    public bool IsFollowing(string authorName)
    {
        var username = User.Identity?.Name;
        if (string.IsNullOrEmpty(username)) return false;

        var currentUser =
            _authorService.GetAuthorEntityByName(username).Result;
        var followTarget =
            _authorService.GetAuthorEntityByName(authorName).Result;

        if (currentUser == null || followTarget == null)
            return false;

        return currentUser.Following
            .Any(f => f.FollowedById == followTarget.Id);
    }

    private async Task<string?> GetCurrentUserIdAsync()
    {
        if (User.Identity?.IsAuthenticated != true)
            return null;

        var username = User.Identity?.Name;
        if (string.IsNullOrEmpty(username))
            return null;

        var author = await _authorService.GetAuthorByName(username);
        return author?.Id;
    }
}