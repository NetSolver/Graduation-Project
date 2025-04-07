using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Smile_Simulation.Domain.Entities;
using Smile_Simulation.Domain.Interfaces.Repositories;
using Smile_Simulation.Infrastructure.Data;

namespace Smile_Simulation.Application.Services
{
    public class LikeService
    {
        private readonly ILikeRepository _likeRepository;
        private readonly IPostRepository _postRepository;
        private readonly SmileDbContext _context;
        private readonly IMapper _mapper;

        public LikeService(ILikeRepository likeRepository, IPostRepository postRepository, SmileDbContext context,
            IMapper mapper)
        {
            _likeRepository = likeRepository;
            _postRepository = postRepository;
            _context = context;
            _mapper = mapper;
        }

        public async Task<string> AddLikeAsync(int postId, string userId)
        {
            if (!await _postRepository.ExistsAsync(postId))
                return "Post not found";

            var userExists = await _context.Users.AnyAsync(u => u.Id == userId);
            if (!userExists)
                return "User not found";

            if (await _likeRepository.IsPostLikedByUserAsync(postId, userId))
                return "Already liked";

            var like = new Like { PostId = postId, UserId = userId };
            await _likeRepository.AddAsync(like);
            await _context.SaveChangesAsync();
            return "Like added successfully";
        }

        public async Task<string> RemoveLikeAsync(int postId, string userId)
        {
          
            var like = await _likeRepository.GetByIdAsync(postId, userId);

            if (like == null)
                return "Like not found";

            await _likeRepository.DeleteAsync(postId, userId);  
            await _context.SaveChangesAsync();  

            return "Like removed successfully";
        }
    }
}
