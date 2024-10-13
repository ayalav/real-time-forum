namespace RealTimeForum.Models;

public class PostRequestDTO
{
    public required string Title { get; set; }
    public required string Content { get; set; }
}

public class PostResponseDTO
{
    public int Id { get; set; }
    public required string Title { get; set; }
    public required string Content { get; set; }
    public required string UserName { get; set; }
}
