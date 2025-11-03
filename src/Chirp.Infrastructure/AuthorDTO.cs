using Chirp.Core;

namespace Chirp.Infrastructure;

public class AuthorDTO
{
    public int Id  { get; set; }
    
    public string Name { get; set; }
    
    public string Email { get; set; }
    
    public ICollection<Cheep> Cheeps { get; set; }
}