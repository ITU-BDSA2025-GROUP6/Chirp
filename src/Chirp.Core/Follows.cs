using System.ComponentModel.DataAnnotations;

namespace Chirp.Core;

public class Follows
{
    [Required] public string FollowsId { get; set; } = string.Empty;
    public Author? FollowsAuthor { get; set; }

    [Required] public string FollowedById { get; set; } = string.Empty;
    public Author? FollowedByAuthor { get; set; }
}