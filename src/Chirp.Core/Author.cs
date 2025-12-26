using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Chirp.Core
{
    public class Author : IdentityUser
    { 
        public ICollection<Cheep> Cheeps { get; set; } = new List<Cheep>();

        public List<Recheep> Recheeps { get; set; } = new List<Recheep>(); // List of cheeps we've recheeped, for printing in MyTimeline

        public List<Follows> Following { get; set; } = new List<Follows>();

        public List<Follows> FollowedBy { get; set; } = new List<Follows>();
        
        public string ProfilePicturePath { get; set; } = "/images/default.png";
    }
}