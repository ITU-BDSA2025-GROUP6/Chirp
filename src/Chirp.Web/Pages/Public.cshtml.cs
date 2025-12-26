using System.ComponentModel.DataAnnotations;
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
    public List<CheepDTO> Cheeps { get; set; } = new List<CheepDTO>();
    
    public Task<List<Author>>? Followers { get; set; }

    [BindProperty]
    [StringLength(160, ErrorMessage = "The {0} must be at max {1} characters long.")]
    public string Text { get; set; } = string.Empty;
    public PublicModel(ICheepService service,  IAuthorService authorService)
    {
        _service = service;
        _authorService = authorService;
    }

    public async Task<IActionResult> OnGetAsync()
    {
        int currentPage = 1;
        Cheeps = await _service.GetCheeps(currentPage);
        return Page();
    }

   public async Task<IActionResult> OnPostRecheepAsync(int CheepID)
   {
   var authorName = User.Identity?.Name;
   var author = await _authorService.GetAuthorByName(authorName);

   if (author == null) {
    return Redirect("/");
   }

    AuthorDTO authorDTO = new AuthorDTO { Id = author.Id };
   await _authorService.createRecheep(authorDTO, CheepID);
   return RedirectToPage();
   }

    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid)
        {
            Cheeps = await _service.GetCheeps(1);
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
        
        CheepDTO newCheep = new CheepDTO // not 100% but should automatically increment the id, check if this is the case.
        {
            AuthorName = authorName,
            Text = Text,
            Timestamp = DateTime.UtcNow
        };
        await _service.CreateCheep(newCheep);
        
        
        return Redirect("/");
    }

    public async Task<IActionResult> OnPostDeleteAsync(int cheepId)
    {
        if (! User.Identity?.IsAuthenticated ?? true)
        {
            return  Redirect("/");
        }
        var authorName = User.Identity?.Name;
        if (string.IsNullOrEmpty(authorName))
        {
            return Redirect("/");
        }
        await _service.DeleteCheep(cheepId, authorName);
        return Redirect("/");
    }


    public async Task<IActionResult> OnGetFollowBtnAsync(string authorName)
    {
        var username = User.Identity?.Name;
        if (string.IsNullOrEmpty(username)) return Redirect("/");

        var currentUser = await _authorService.GetAuthorEntityByName(username);
        var followTarget = await _authorService.GetAuthorEntityByName(authorName);

        if (currentUser == null || followTarget == null || currentUser.Id == followTarget.Id)
            return Redirect("/");
        
        Console.WriteLine($"CurrentUser: {currentUser.UserName}");
        Console.WriteLine($"FollowTarget: {followTarget.UserName}");
        Console.WriteLine($"Following.Count before add: {currentUser.Following.Count}");


        bool alreadyFollow = currentUser.Following
            .Any(f => f.FollowedById == followTarget.Id);

        Console.WriteLine($"AlreadyFollowing: {alreadyFollow}");
        
        if (!alreadyFollow)
        {
            currentUser.Following.Add(new Follows
            {
                FollowsId = currentUser.Id,
                FollowedById = followTarget.Id
            });

            await _authorService.SaveChangesAsync();
        }
        else
        {
            currentUser.Following.RemoveAll(f => f.FollowedById == followTarget.Id);
            await _authorService.SaveChangesAsync();
        }

        Console.WriteLine($"Following.Count after add: {currentUser.Following.Count}");
        
        return RedirectToPage();
    }

    public bool IsFollowing(string authorName)
    {
        var username = User.Identity?.Name;
        if (string.IsNullOrEmpty(username)) return false;
        
        
        var currentUser = _authorService.GetAuthorEntityByName(username).Result;
        var followTarget = _authorService.GetAuthorEntityByName(authorName).Result;
        if (currentUser == null || followTarget == null)
            return false;


        bool alreadyFollow = currentUser.Following
            .Any(f => f.FollowedById == followTarget.Id);
        return alreadyFollow;
    }
    
    public async Task<bool> IsFollowingAsync(string authorName)
    {
        var username = User.Identity?.Name;
        if (string.IsNullOrEmpty(username)) return false;

        var currentUser = await _authorService.GetAuthorEntityByName(username);
        var followTarget = await _authorService.GetAuthorEntityByName(authorName);

        if (currentUser == null || followTarget == null)
            return false;

        return currentUser.Following.Any(f => f.FollowedById == followTarget.Id);
    }


}
