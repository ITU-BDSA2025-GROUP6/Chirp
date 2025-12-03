using Chirp.Infrastructure;
using Chirp.Infrastructure.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Chirp.Web.Pages;

public class PaginationModel : PageModel
{
    private readonly ICheepService _service;

    // materialized list — do not keep this as Task<T>
    public List<CheepDTO> Cheeps { get; set; } = new();
    public bool hasNextPage { get; set; }
    
    public int currentPage { get; set; }

    public PaginationModel(ICheepService service)
    {
        _service = service;
    }

    // Async handler — await the service calls to avoid sync-over-async and concurrent DbContext usage
    public async Task<IActionResult> OnGetAsync(int index = 1)
    {
        currentPage = index < 1 ? 1 : index;

        // Materialize the current page cheeps inside the request scope
        Cheeps = await _service.GetCheeps(currentPage) ?? new List<CheepDTO>();

        // Check if the next page has any items — await instead of blocking.
        var nextPageCheeps = await _service.GetCheeps(currentPage + 1);
        hasNextPage = (nextPageCheeps != null && nextPageCheeps.Any());

        return Page();
    }
}