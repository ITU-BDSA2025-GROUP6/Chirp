using Chirp.Core;

namespace Chirp.Infrastructure;

public class AuthorDTO
{ 
    public int AuthorID  { get; set; }

    public string Name { get; set; } = string.Empty;

    public string Email { get; set; } = string.Empty;
    
    public ICollection<Cheep> Cheeps { get; set; }  = new List<Cheep>();
}