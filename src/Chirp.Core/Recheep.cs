using System.ComponentModel.DataAnnotations;

namespace Chirp.Core;

 public class Recheep
 {
     
     [Required] public required string AuthorID { get; set; }
    
     [Required] public required int CheepID { get; set; }
 }