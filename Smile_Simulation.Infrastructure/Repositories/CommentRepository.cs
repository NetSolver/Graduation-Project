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
    public class CommentRepository : GenericRepository<Comment>, ICommentRepository
    {
        private readonly SmileDbContext _context;
        public CommentRepository(SmileDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<List<Comment>> GetCommentsByPostIdAsync(int postId) =>
            await _context.Comments
                .Include(c => c.User) 
                .Where(c => c.PostId == postId)
                .ToListAsync();

        public async Task<Comment> GetCommentByPostIdAndCommentIdAsync(int postId, int commentId)
        {
            return await _context.Comments
                .Include(c => c.User) 
                .FirstOrDefaultAsync(c => c.PostId == postId && c.Id == commentId);
        }


    }
}
