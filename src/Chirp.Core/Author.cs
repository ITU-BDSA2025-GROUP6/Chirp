using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Chirp.Core
{
    public class Author : IdentityUser
    { 
        public ICollection<Cheep> Cheeps { get; set; } = new List<Cheep>();
        public List<int> RecheepIDs = new List<int>(); // List of cheeps we've recheeped, for printing in MyTimeline
    }
}