using Chirp.Core;

namespace Chirp.Infrastructure;

public class AuthorDto
{ 
    public int AuthorId  { get; set; }

    public string Name { get; set; } = string.Empty;

    public string Email { get; set; } = string.Empty;
    
    public ICollection<Cheep> Cheeps { get; set; }  = new List<Cheep>();
}