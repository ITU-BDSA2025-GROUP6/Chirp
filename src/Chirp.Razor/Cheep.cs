namespace Chirp.Razor;
using System.ComponentModel.DataAnnotations;

public class Cheep
{
    [Required]
    public required int CheepID { get; set; }
    
    [Required] 
    [StringLength(500)]
    public required string Text { get; set; }
    
    [Required]
    public required DateTime Timestamp { get; set; }
    
    [Required]
    public required Author Author { get; set; }
    
    
}