using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using RealTimeForum.Hubs;
using RealTimeForum.Models;
using RealTimeForum.Services;

namespace RealTimeForum.Controllers;

[Authorize]
[ApiController]
[Route("[controller]")]
public class PostController(IPostService postService, IHubContext<ForumHub> hubContext) : ControllerBase
{
    // Fetch all posts
    [HttpGet]
    public async Task<IActionResult> GetAllPosts()
    {
        var posts = await postService.GetAllPosts();
        return Ok(posts);
    }

    // Create a new post
    [HttpPost]
    public async Task<IActionResult> CreatePost([FromBody] PostRequestDTO post)
    {
        var userId = GetUserId();

        if (userId == 0)
            return Unauthorized("User ID not found in token.");

        var createdPost = await postService.CreatePost(post, userId);
        await hubContext.Clients.All.SendAsync("ReceivePostUpdate", post.Title);

        return Ok(createdPost);
    }

    // Fetch comments for a specific post
    [HttpGet("{postId}/comments")]
    public async Task<IActionResult> GetComments(int postId)
    {
        var comments = await postService.GetComments(postId);
        return Ok(comments);
    }

    // Add a comment to a specific post
    [HttpPost("{postId}/comments")]
    public async Task<IActionResult> AddComment(int postId, [FromBody] CommentRequestDTO comment)
    {
        var userId = GetUserId();

        if (userId == 0)
            return Unauthorized("User ID not found in token.");

        var createdComment = await postService.AddComment(postId, comment, userId);
        await hubContext.Clients.All.SendAsync("ReceiveCommentUpdate", comment.Content);

        return Ok(createdComment);
    }

    // Get the user ID from the JWT token claim
    private int GetUserId()
    {
        var claim = User.Claims.FirstOrDefault(c => c.Type == "UserId");
        if (claim == null)
            return 0;

        var userId = int.Parse(claim.Value);
        return userId;
    }
}

