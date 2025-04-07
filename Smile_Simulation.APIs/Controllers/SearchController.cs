using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Smile_Simulation.Application.Services;

namespace Smile_Simulation.APIs.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
  
    public class SearchController : ControllerBase
    {
        private readonly SearchService _searchService;

        public SearchController(SearchService searchService)
        {
            _searchService = searchService;
        }


        [HttpGet("GlobalSearch")]
        [Authorize(Roles = "Patient, Doctor, Admin")]
        public async Task<IActionResult> GlobalSearch(string searchQuery, int pageNumber = 1, int pageSize = 10)
        {
            if (string.IsNullOrWhiteSpace(searchQuery))
                return BadRequest("يجب إدخال عبارة البحث.");

            var (results, totalCount) = await _searchService.GlobalSearchAsync(searchQuery, pageNumber, pageSize);

            int totalPages = (int)Math.Ceiling((double)totalCount / pageSize);

            var response = new
            {
                Results = results,
                CurrentPage = pageNumber,
                TotalPages = totalPages,
                TotalResults = totalCount
            };

            return Ok(response);
        }
    }
}
