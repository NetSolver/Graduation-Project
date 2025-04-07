using System.Security.Claims;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Smile_Simulation.Application.Services;
using Smile_Simulation.Domain.DTOs.Comments;
using Smile_Simulation.Domain.Entities;
using Smile_Simulation.Infrastructure.Repositories;

namespace Smile_Simulation.APIs.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Patient,Doctor,Admin")]
    public class CommentController : ControllerBase
    {
        private readonly CommentService _commentService;
        private readonly IMapper _mapper;
        private readonly UserManager<UserApp> _userManager;


        public CommentController(CommentService commentService, IMapper mapper, UserManager<UserApp> userApp)
        {
            _commentService = commentService;
            _mapper = mapper;
            _userManager = userApp;

        }

        [HttpGet("postId/{postId}")]

        public async Task<ActionResult<List<CommentResponseDTO>>> GetCommentsByPostId(int postId)
        {
            var comments = await _commentService.GetAllCommentsByPostIdAsync(postId);
            return Ok(comments);
        }



        [HttpPost("postId/{postId}")]
        public async Task<ActionResult<CommentResponseDTO>> AddComment([FromRoute] int postId,
            [FromBody] CommentDTO request)
        {
            if (string.IsNullOrEmpty(request.Content))
                return BadRequest("Comment content cannot be empty.");

            var currentUserId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(currentUserId))
            {
                return Unauthorized("User is not authenticated.");
            }

            var user = await _userManager.FindByIdAsync(currentUserId);
            if (user == null)
                return NotFound("User not found");

            var currentUserName = user.FullName;

            string? currentUserImage = null;
            if (!string.IsNullOrEmpty(user.Image))
            {
                currentUserImage = user.Image;
            }



            var createdComment =
                await _commentService.AddCommentAsync(request, currentUserId, currentUserName, currentUserImage,
                    postId);

            return CreatedAtAction(nameof(GetCommentsByPostId), new { postId = createdComment.PostId }, createdComment);

        }

        [HttpPut("postId/{postId}/commentId/{commentId}")]

        public async Task<ActionResult<CommentResponseDTO>> UpdateComment([FromRoute] int postId,
            [FromRoute] int commentId, [FromBody] CommentDTO request)
        {
            if (string.IsNullOrEmpty(request.Content))
                return BadRequest("Invalid content data.");


            var updatedComment =
                await _commentService.UpdateCommentAsync(postId, commentId, request.Content);

            if (updatedComment == null)
                return NotFound("Comment not found or does not belong to the specified post.");
            return Ok(updatedComment);

        }


        [HttpDelete("postid/{postId}/commentid/{commentId}")]
        public async Task<IActionResult> DeleteComment([FromRoute] int postId, [FromRoute] int commentId)
        {
           

            var result = await _commentService.DeleteCommentAsync(postId, commentId);

            return Ok("Comment Deleted Successfully");
        }

    }
}