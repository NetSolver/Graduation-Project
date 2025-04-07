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
    public class LikeRepository : GenericRepository<Like>, ILikeRepository
    {
        private readonly SmileDbContext _context;

        public LikeRepository(SmileDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<bool> IsPostLikedByUserAsync(int postId, string userId) =>
            await _context.Likes.AnyAsync(l => l.PostId == postId && l.UserId == userId);
      

    }
}
