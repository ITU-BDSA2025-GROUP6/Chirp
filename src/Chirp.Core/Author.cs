using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Chirp.Core
{
    public class Author : IdentityUser
    { 
        public ICollection<Cheep> Cheeps { get; set; } = new List<Cheep>();
        
        public List<Author> Authors { get; set; } = new List<Author>();
    }
}