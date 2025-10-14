using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Chirp.Core
{
    public class Author
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)] // Auto-increment
        public int AuthorID { get; set; }

        [Required]
        public required string Name { get; set; }

        [Required]
        public required string Email { get; set; }

        public ICollection<Cheep> Cheeps { get; set; } = new List<Cheep>();
    }
}