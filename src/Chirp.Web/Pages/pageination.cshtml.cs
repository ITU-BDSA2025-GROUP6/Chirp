using Chirp.Infrastructure;
using Chirp.Infrastructure.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Chirp.Web.Pages;

public class PaginationModel : PageModel
{
    private readonly ICheepService _service;

    public List<CheepDTO> Cheeps { get; set; } = new List<CheepDTO>();
    public bool hasNextPage { get; set; }
    
    public int currentPage { get; set; }

    public PaginationModel(ICheepService service)
    {
        _service = service;
    }

    public async Task<IActionResult> OnGetAsync(int index)
    {
        currentPage = index < 1 ? 1 : index;
        
        Cheeps = await _service.GetCheeps(currentPage);
        
        var nextPageCheeps = await _service.GetCheeps(currentPage + 1);
        hasNextPage = nextPageCheeps.Any();
            
        return Page();
    }
}