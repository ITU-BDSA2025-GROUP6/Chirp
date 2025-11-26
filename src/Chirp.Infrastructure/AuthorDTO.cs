using Chirp.Core;

namespace Chirp.Infrastructure;

public class AuthorDTO
{
    public string Id  { get; set; }
    
    public string Name { get; set; }
    
    public string Email { get; set; }
    
    public ICollection<Cheep> Cheeps { get; set; }

    public List<AuthorDTO> Following { get; set; } = new();
}