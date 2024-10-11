using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RealTimeForum.Data.Entities;

public class Post
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    [Required]
    public int UserId { get; set; }

    [Required]
    public required string Title { get; set; }

    [Required]
    public required string Content { get; set; }

    [ForeignKey(nameof(UserId))]
    public User User { get; set; } = null!;
}
