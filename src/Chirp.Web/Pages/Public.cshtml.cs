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
    public Task<List<CheepDTO>> Cheeps { get; set; } = Task.FromResult(new List<CheepDTO>());
    
    public Task<List<Author>>? Followers { get; set; }

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
        int currentPage = 1;
        Cheeps = _service.GetCheeps(currentPage);
        return Page();
    }

   public async Task<IActionResult> OnPostRecheepAsync(int CheepID)
   {
   var authorName = User.Identity?.Name;
   var author = await _authorService.GetAuthorByName(authorName);

   if (author == null) {
    return Redirect("/");
   }

    // fallback
   if (author.RecheepIDs == null) { author.RecheepIDs = new List<int>(); }

   author.RecheepIDs.Add(CheepID);
   foreach (var a in author.RecheepIDs) {
   Console.WriteLine("Recheep IDs for Author: " + a);
   }

   return RedirectToPage();
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
        
        CheepDTO newCheep = new CheepDTO // not 100% but should automatically increment the id, check if this is the case.
        {
            AuthorName = authorName,
            Text = Text,
            Timestamp = DateTime.UtcNow
        };
        _service.CreateCheep(newCheep);
        
        
        return Redirect("/");
    }

    public async Task<IActionResult> OnPostDelete(int cheepId)
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


    public async Task<IActionResult> OnGetFollowBtn(string authorName)
    {
        //if (!User.Identity.IsAuthenticated)
           // return Redirect("/");

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

        Console.WriteLine($"Following.Count after add: {currentUser.Following.Count}");

        return RedirectToPage();
    }
}
