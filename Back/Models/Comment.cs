namespace RealTimeForum.Models;

public class CommentRequestDTO
{
    public required string Content { get; set; }
}

public class CommentResponseDTO
{
    public required string Content { get; set; }
    public required string UserName { get; set; }
}
