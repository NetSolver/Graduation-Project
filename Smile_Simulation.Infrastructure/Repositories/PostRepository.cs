using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Smile_Simulation.Domain.Entities;
using Smile_Simulation.Domain.Interfaces.Repositories;
using Smile_Simulation.Infrastructure.Data;

namespace Smile_Simulation.Infrastructure.Repositories
{
    public class PostRepository : GenericRepository<Post>, IPostRepository
    {

        private readonly SmileDbContext _context;
        public PostRepository(SmileDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<Post> GetByPostIdAsync(int postId)
        {
            var post = await _context.Posts
                .Include(p => p.Likes)
                .Include(p => p.Comments)
                .FirstOrDefaultAsync(p => p.Id == postId);

            return post;
        }

        public async Task<bool> ExistsAsync(int postId)
        {
            return await _context.Posts.AnyAsync(p => p.Id == postId);
        }


        public async Task<Post> GetPostWithCommentsAndLikesAsync(int postId)
        {
            return await _context.Posts
                .Include(p => p.Comments)
                .Include(p => p.Likes)
                .FirstOrDefaultAsync(p => p.Id == postId);
        }
     
    }
}
