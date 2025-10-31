using Chirp.Core;

namespace Chirp.Infrastructure;

public class AuthorDTO
{
    public int AuthorID  { get; set; }
    
    public string Username { get; set; }

    public string Password { get; set; }
    
    public string Email { get; set; }
    
    public ICollection<Cheep> Cheeps { get; set; }
}