using Microsoft.EntityFrameworkCore;
using RealTimeForum.Data;
using RealTimeForum.Data.Entities;
using RealTimeForum.Models;

namespace RealTimeForum.Services;

public interface IPostService
{
    Task<List<PostResponseDTO>> GetAllPosts();
    Task<PostResponseDTO> CreatePost(PostRequestDTO post, int userId);
    Task<List<CommentResponseDTO>> GetComments(int postId);
    Task<CommentResponseDTO> AddComment(int postId, CommentRequestDTO comment, int userId);
}

public class PostService(MyDbContext dbContext) : IPostService
{
    // Fetch all posts
    public async Task<List<PostResponseDTO>> GetAllPosts()
    {
        return await dbContext
            .Posts
            .Include(post => post.User)
            .Select(post => PostToDTO(post))
            .ToListAsync();
    }

    // Create a new post
    public async Task<PostResponseDTO> CreatePost(PostRequestDTO post, int userId)
    {
        var newPost = new Post()
        {
            Title = post.Title,
            Content = post.Content,
            UserId = userId
        };

        dbContext.Posts.Add(newPost);
        await dbContext.SaveChangesAsync();

        var postWithUser = await dbContext
                        .Posts
                        .Include(c => c.User)
                        .FirstOrDefaultAsync(c => c.Id == newPost.Id);

        return PostToDTO(newPost);
    }

    // Fetch comments for a specific post
    public async Task<List<CommentResponseDTO>> GetComments(int postId)
    {
        return await dbContext
            .Comments
            .Include(comment => comment.User)
            .Where(c => c.PostId == postId)
            .Select(comment => CommentToDTO(comment))
            .ToListAsync();
    }

    // Add a comment to a specific post
    public async Task<CommentResponseDTO> AddComment(int postId, CommentRequestDTO comment, int userId)
    {
        var newComment = new Comment()
        {
            PostId = postId,
            Content = comment.Content,
            UserId = userId
        };

        dbContext.Comments.Add(newComment);
        await dbContext.SaveChangesAsync();

        var commentWithUser = await dbContext
                     .Comments
                     .Include(c => c.User)
                     .FirstOrDefaultAsync(c => c.Id == newComment.Id);

        return CommentToDTO(newComment);
    }

    private static PostResponseDTO PostToDTO(Post post)
    {
        return new PostResponseDTO
        {
            Id = post.Id,
            Title = post.Title,
            Content = post.Content,
            UserName = post.User.UserName
        };
    }

    private static CommentResponseDTO CommentToDTO(Comment comment)
    {
        return new CommentResponseDTO
        {
            Content = comment.Content,
            UserName = comment.User.UserName
        };
    }
}

