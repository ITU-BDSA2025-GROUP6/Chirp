using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Chirp.Razor.Pages
{
    public class LoginModel : PageModel
    {
    private readonly ICheepService _cheepService;

    [BindProperty]
    public string rec_Username { get; set; }

    [BindProperty]
    public string rec_Email { get; set; }

    [BindProperty]
    public string rec_Password { get; set; }

    {

        public void OnPost() {
        AuthorDTO author = new AuthorDTO{Username = rec_Username, Email = rec_Email, Password = rec_Password}

        // Creates the author using the dto
        await _cheepService.CreateAuthor(author);
        }


        public void OnGet() {

        }
    }
    }
}