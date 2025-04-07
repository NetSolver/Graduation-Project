using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Smile_Simulation.Domain.Interfaces.Repositories;

namespace Smile_Simulation.Application.Services
{
    public class SearchService
    {
        private readonly IPostRepository _postRepository;

        public SearchService(IPostRepository postRepository)
        {
            _postRepository = postRepository;
        }
        public async Task<(List<dynamic> Results, int TotalCount)> GlobalSearchAsync(string searchQuery, int pageNumber, int pageSize)
        {
            searchQuery = searchQuery.ToLower();


            var posts = (await _postRepository.GetAllAsync())
                .Where(p => p.Content.ToLower().Contains(searchQuery) )
                .Select(p => new { Type = "Post", p.Content ,p.Image});


            var combinedResults = posts.Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .Cast<dynamic>()
                .ToList();

            int totalCount = posts.Count();

            return (combinedResults, totalCount);
        }

    }
}
