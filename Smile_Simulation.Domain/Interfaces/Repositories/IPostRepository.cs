using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Smile_Simulation.Domain.Entities;

namespace Smile_Simulation.Domain.Interfaces.Repositories
{
    public interface IPostRepository : IGenericRepository<Post>
    {
        Task<Post> GetByPostIdAsync(int postId);
        Task<bool> ExistsAsync(int postId);
    }
}
