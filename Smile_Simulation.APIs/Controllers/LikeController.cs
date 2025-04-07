using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Smile_Simulation.Application.Services;

namespace Smile_Simulation.APIs.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Patient,Doctor,Admin")]
    public class LikeController : ControllerBase
    {
        private readonly LikeService _likeService;

        public LikeController(LikeService likeService)
        {
            _likeService = likeService;
        }


        [HttpPost("postid/{postId}")]
        public async Task<IActionResult> ToggleLike(int postId)
        {

            var currentUserId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (currentUserId == null)
                return Unauthorized(new { message = "User not authenticated." });


            var likeResult = await _likeService.AddLikeAsync(postId, currentUserId);

            if (likeResult == "Post not found")
                return NotFound(new { message = likeResult });

            if (likeResult == "User not found")
                return NotFound(new { message = likeResult });

            if (likeResult == "Already liked")
            {
                var removeResult = await _likeService.RemoveLikeAsync(postId, currentUserId);
                return Ok(new { message = removeResult });
            }


            return Ok(new { message = likeResult });
        }

    }
}
