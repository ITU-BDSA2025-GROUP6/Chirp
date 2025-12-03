using System.ComponentModel.DataAnnotations;
using System.Net.Mime;
using Chirp.Infrastructure;
using Chirp.Infrastructure.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Chirp.Web.Pages;

public class PublicModel : PageModel
{
    private readonly ICheepService _service;
    private readonly IAuthorService _authservice;
    public Task<List<CheepDTO>> Cheeps { get; set; } = Task.FromResult(new List<CheepDTO>());
    
    [BindProperty]
    [StringLength(160, ErrorMessage = "The {0} must be at max {1} characters long.")]
    public string Text { get; set; } = string.Empty;
    public PublicModel(ICheepService service, IAuthorService authservice)
    {
        _service = service;
        _authservice = authservice;
    }

    public ActionResult OnGet()
    {
        int currentpage = 1;
        Cheeps = _service.GetCheeps(currentpage);
        return Page();
    }

   public async Task<IActionResult> OnPostRecheepAsync(int CheepID)
   {
   var authorName = User.Identity?.Name;
   var author = await _authservice.GetAuthorByName(authorName);

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
        
        CheepDTO newcheep = new CheepDTO // not 100% but should automaticlly incremnt the id, check if this is the case.
        {
            AuthorName = authorName,
            Text = Text,
            Timestamp = DateTime.UtcNow
        };
        _service.CreateCheep(newcheep);
        
        
        return Redirect("/");
    }
}
