using System.ComponentModel.DataAnnotations;

namespace Chirp.Core;

public class Follows
{
    [Required]
    [MaxLength(36)]
    public string? FollowsId { get; set; }
    public Author? FollowsAuthor { get; set; }

    [Required]
    [MaxLength(36)]
    public string? FollowedById { get; set; } 
    public Author? FollowedByAuthor { get; set; }
}