using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Chirp.Razor.Pages;

public class PaginationModel : PageModel
{
    private readonly ICheepService _service;

    public List<CheepViewModel> Cheeps { get; set; } = new();
    public bool hasNextPage { get; set; }
    
    public int currentPage { get; set; }

    public PaginationModel(ICheepService service)
    {
        _service = service;
    }

    public ActionResult OnGet(int index)
    {
        int pageSize = 32;
        currentPage = index < 1 ? 1 : index;
        var cheeps = _service.GetCheeps(currentPage, pageSize+1);
        
        hasNextPage = cheeps.Count > pageSize;
        Cheeps = cheeps.Take(pageSize).ToList();
        return Page();
    }
}