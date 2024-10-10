using Microsoft.EntityFrameworkCore;
using RealTimeForum.Data;
using RealTimeForum.Data.Entities;
using RealTimeForum.Models;

namespace RealTimeForum.Services;

public interface IPostService
{
    Task<List<Post>> GetAllPosts();
    Task<Post> CreatePost(PostDTO post, int userId);
    Task<List<Comment>> GetComments(int postId);
    Task<Comment> AddComment(int postId, CommentDTO comment);
}

public class PostService : IPostService
{
    private readonly MyDbContext _dbContext;

    public PostService(MyDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    // Fetch all posts
    public async Task<List<Post>> GetAllPosts()
    {
        return await _dbContext.Posts.ToListAsync();
    }

    // Create a new post
    public async Task<Post> CreatePost(PostDTO post, int userId)
    {
        var newPost = new Post()
        {
            Title = post.Title,
            Content = post.Content,
            UserId = userId
        };

        _dbContext.Posts.Add(newPost);
        await _dbContext.SaveChangesAsync();
        return newPost;
    }

    // Fetch comments for a specific post
    public async Task<List<Comment>> GetComments(int postId)
    {
        return await _dbContext.Comments.Where(c => c.PostId == postId).ToListAsync();
    }

    // Add a comment to a specific post
    public async Task<Comment> AddComment(int postId, CommentDTO comment)
    {
        var newComment = new Comment()
        {
            PostId = postId,
            Content = comment.Content,
        };

        _dbContext.Comments.Add(newComment);
        await _dbContext.SaveChangesAsync();
        return newComment;
    }
}

