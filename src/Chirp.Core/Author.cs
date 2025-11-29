using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Chirp.Core
{
    public class Author : IdentityUser
    { 
        public ICollection<Cheep> Cheeps { get; set; } = new List<Cheep>();
        
        public List<Follows> Following { get; set; } = new List<Follows>();
        
        public List<Follows> FollowedBy { get; set; } = new List<Follows>();
    }
}