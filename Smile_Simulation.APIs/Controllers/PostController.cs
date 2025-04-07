using System.Security.Claims;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Smile_Simulation.Application.Services;
using Smile_Simulation.Domain.DTOs.PostsDto;
using Smile_Simulation.Domain.Entities;

namespace Smile_Simulation.APIs.Controllers
{
  

        [Route("api/[controller]")]
        [ApiController]
        [Authorize(Roles = "Admin,Doctor")] 
        public class PostController : ControllerBase
            { 
               private readonly PostService _postService; 
          

      
            public PostController(PostService postService)
            {
                _postService = postService;  
               
            }


      
                [HttpGet("{postId}")]
                public async Task<ActionResult<PostDTO>> GetPostById([FromRoute]int postId)
                {   
                    var currentUserId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                    var post = await _postService.GetPostByIdAsync(postId, currentUserId);
                    if (post == null) return NotFound("Post not found.");
                    return Ok(post);

                }



                [HttpPost] 
                public async Task<ActionResult<PostDTO>> CreatePost([FromForm] CreatePostDTO postDto)
                {
                    if (!ModelState.IsValid)
                    {
                        var errors = ModelState.Values.SelectMany(v => v.Errors)
                            .Select(e => e.ErrorMessage)
                            .ToList();
                        return BadRequest(new { message = "Validation failed", errors });
                    }

                    if (User?.Identity == null || !User.Identity.IsAuthenticated)
                    {
                        return Unauthorized(new { message = "User is not authenticated" });
                    }

                    try
                    {
                        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

                        if (string.IsNullOrEmpty(userId))
                        {
                            return Unauthorized(new { message = "User ID not found" });
                        }

                        var createdPostDto = await _postService.AddPostAsync(postDto, userId);
                    return CreatedAtAction(nameof(GetPostById), new { postId = createdPostDto.Id }, createdPostDto);
                    }
                    catch (Exception ex)
                    {
                        return StatusCode(500, new
                        {
                            message = "Failed to create the post",
                            error = ex.Message,
                            inner = ex.InnerException?.Message
                        });
                    }
                }


            [HttpPut("PostId/{postId}")]
                public async Task<ActionResult<PostDTO>> UpdatePost([FromRoute] int postId, [FromForm] CreatePostDTO updatePostDto)
                {
                    try
                    {
                        var updatedPostDTO = await _postService.UpdatePostAsync(postId, updatePostDto);
                        if (updatedPostDTO == null)
                            return NotFound("Post not found.");

                        return Ok(updatedPostDTO);
                    }
                    catch (Exception ex)
                    {
                        return StatusCode(500, new { message = "Failed to update the post", error = ex.Message });
                    }
                }


            [HttpDelete("PostId/{postId}")]
                public async Task<ActionResult> DeletePost([FromRoute]int postId)
                {
                    var deleted = await _postService.DeletePostAsync(postId);
                    if (!deleted) return NotFound("Post not found.");
                    return Ok("Post deleted successfully.");
                }

            }

        }

