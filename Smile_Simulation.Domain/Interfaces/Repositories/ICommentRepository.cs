using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Smile_Simulation.Domain.Entities;

namespace Smile_Simulation.Domain.Interfaces.Repositories
{
    public interface ICommentRepository : IGenericRepository<Comment>
    {
        Task<List<Comment>> GetCommentsByPostIdAsync(int postId);
        Task<Comment?> GetCommentByPostIdAndCommentIdAsync(int postId, int commentId);
    }
}
