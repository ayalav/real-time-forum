using RealTimeForum.Data;
using RealTimeForum.Data.Entities;
using RealTimeForum.Models;

namespace RealTimeForum.Services;

public class PostService
{
    private readonly MyDbContext _dbContext;

    public PostService(MyDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    // Fetch all posts
    public List<Post> GetAllPosts()
    {
        return _dbContext.Posts.ToList();
    }

    // Create a new post
    public Post CreatePost(PostDTO post, int userId)
    {
        var newPost = new Post()
        {
            Title = post.Title,
            Content = post.Content,
            UserId = userId
        };

        _dbContext.Posts.Add(newPost);
        _dbContext.SaveChanges();
        return newPost;
    }

    // Fetch comments for a specific post
    public List<Comment> GetComments(int postId)
    {
        return _dbContext.Comments.Where(c => c.PostId == postId).ToList();
    }

    // Add a comment to a specific post
    public Comment AddComment(int postId, CommentDTO comment)
    {
        var newComment = new Comment()
        {
            PostId = postId,
            Content = comment.Content,
        };

        _dbContext.Comments.Add(newComment);
        _dbContext.SaveChanges();
        return newComment;
    }
}

