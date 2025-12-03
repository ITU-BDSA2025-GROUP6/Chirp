using System.ComponentModel.DataAnnotations;

namespace Chirp.Core;

 public class Recheep
 {
     [Required] public string AuthorID { get; set; }

     [Required] public int CheepID { get; set; }
 }