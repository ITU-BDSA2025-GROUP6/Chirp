using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Chirp.Core;

public class Follows
{
    [Required]
    public string FollowsId { get; set; }   
    public Author FollowsAuthor { get; set; }

    [Required]
    public string FollowedById { get; set; } 
    public Author FollowedByAuthor { get; set; }
}