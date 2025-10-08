namespace Chirp.Razor;

public class Author
{
    public int AuthorID  { get; set; }
    public string name { get; set; }
    public string email { get; set; }
    
    
    
    public ICollection<Cheep>  cheeps { get; set; }

   
    
    
}