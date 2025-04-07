using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Smile_Simulation.Domain.Entities;

namespace Smile_Simulation.Domain.Interfaces.Repositories
{
    public interface ILikeRepository : IGenericRepository<Like>
    {
        Task<bool> IsPostLikedByUserAsync(int postId, string userId);
      
    }
}
