using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RealTimeForum.Data.Entities;
using RealTimeForum.Models;
using RealTimeForum.Services;

namespace RealTimeForum.Controllers;

[Authorize] 
[ApiController]
[Route("[controller]")]
public class PostController : ControllerBase
{
    private readonly PostService _postService;

    public PostController(PostService postService)
    {
        _postService = postService;
    }

    // Fetch all posts
    [HttpGet]
    public IActionResult GetAllPosts()
    {
        var posts = _postService.GetAllPosts();
        return Ok(posts);
    }

    // Create a new post
    [HttpPost]
  [HttpPost]
public IActionResult CreatePost([FromBody] PostDTO post)
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
    return Ok(createdPost);
}

    // Fetch comments for a specific post
    [HttpGet("{postId}/comments")]
    public IActionResult GetComments(int postId)
    {
        var comments = _postService.GetComments(postId);
        return Ok(comments);
    }

    // Add a comment to a specific post
    [HttpPost("{postId}/comments")]
    public IActionResult AddComment(int postId, [FromBody] CommentDTO comment)
    {
        var createdComment = _postService.AddComment(postId, comment);
        return Ok(createdComment);
    }
}

