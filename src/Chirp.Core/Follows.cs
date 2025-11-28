using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Chirp.Core;

public class Follows
{
    [Required]
    public Author FollowsID { get; set; }
    
    [Required]
    public Author FollowedByID { get; set; }
}