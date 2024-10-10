using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace RealTimeForum.Data.Entities;

public class User
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }  // מפתח ראשי

    [Required]
    public string UserName { get; set; }

    [Required]
    public string Password { get; set; }  
}