using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Chirp.Razor.Pages;

public class PaginationModel : PageModel
{
    private readonly ICheepService _service;

    public List<CheepViewModel> Cheeps { get; set; } = new();

    public PaginationModel(ICheepService service)
    {
        _service = service;
    }

    public ActionResult OnGet(int index)
    {
        var currentPage = index < 1 ? 1 : index;
        Cheeps = _service.GetCheeps(currentPage);
        return Page();
    }
}