using Chirp.Infrastructure;
using Chirp.Infrastructure.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Chirp.Web.Pages;

public class PaginationModel : PageModel
{
    private readonly ICheepService _service;

    public Task<List<CheepDTO>> Cheeps { get; set; } = Task.FromResult(new List<CheepDTO>());
    public bool hasNextPage { get; set; }
    
    public int currentPage { get; set; }

    public PaginationModel(ICheepService service)
    {
        _service = service;
    }

    public ActionResult OnGet(int index)
    {
        currentPage = index < 1 ? 1 : index;
        Cheeps = _service.GetCheeps(currentPage);

        if (_service.GetCheeps((currentPage + 1)).Result.Any())
        {
            hasNextPage = true;
        }
            
        return Page();
    }
}