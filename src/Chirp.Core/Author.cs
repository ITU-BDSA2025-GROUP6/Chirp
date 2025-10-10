namespace Chirp.Core;
using System.ComponentModel.DataAnnotations;

public class Author
{
    [Required]
    public required int AuthorID  { get; set; }
    
    [Required]
    public required string name { get; set; }
    
    [Required]
    public required string email { get; set; }
    
    [Required]
    public required ICollection<Cheep>  cheeps { get; set; }

   
    
    
}