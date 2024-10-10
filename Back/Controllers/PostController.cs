using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using RealTimeForum.Data.Entities;
using RealTimeForum.Hubs;
using RealTimeForum.Models;
using RealTimeForum.Services;

namespace RealTimeForum.Controllers;

[Authorize]
[ApiController]
[Route("[controller]")]
public class PostController : ControllerBase
{
    private readonly IPostService _postService;
    private readonly IHubContext<ForumHub> _hubContext;

    public PostController(IPostService postService, IHubContext<ForumHub> hubContext)
    {
        _postService = postService;
        _hubContext = hubContext;
    }

    // Fetch all posts
    [HttpGet]
    public async Task<IActionResult> GetAllPosts()
    {
        var posts = await _postService.GetAllPosts();
        return Ok(posts);
    }

    // Create a new post
    [HttpPost]
    public async Task<IActionResult> CreatePost([FromBody] PostDTO post)
    {
        // Get the user ID from the JWT token (claims)
        var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == "UserId");

        if (userIdClaim == null)
        {
            return Unauthorized("User ID not found in token.");
        }

        // Parse the user ID
        var userId = int.Parse(userIdClaim.Value);

        var createdPost = _postService.CreatePost(post, userId);
        await _hubContext.Clients.All.SendAsync("ReceivePostUpdate", post.Title);

        return Ok(createdPost);
    }

    // Fetch comments for a specific post
    [HttpGet("{postId}/comments")]
    public async Task<IActionResult> GetComments(int postId)
    {
        var comments = await  _postService.GetComments(postId);
        return Ok(comments);
    }

    // Add a comment to a specific post
    [HttpPost("{postId}/comments")]
    public async Task<IActionResult> AddComment(int postId, [FromBody] CommentDTO comment)
    {
        var createdComment = _postService.AddComment(postId, comment);
        await _hubContext.Clients.All.SendAsync("ReceiveCommentUpdate", comment.Content);

        return Ok(createdComment);
    }
}

