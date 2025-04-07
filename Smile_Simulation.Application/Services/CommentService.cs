using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Smile_Simulation.Application.Mapping;
using Smile_Simulation.Domain.DTOs.Comments;
using Smile_Simulation.Domain.Entities;
using Smile_Simulation.Domain.Interfaces.Repositories;

namespace Smile_Simulation.Application.Services
{
    public class CommentService
    {
        private readonly ICommentRepository _commentRepository;
        private readonly IMapper _mapper;
        private readonly string _BaseUrl;

        public CommentService(ICommentRepository commentRepository, IMapper mapper, IConfiguration configuration)
        {
            _commentRepository = commentRepository;
            _mapper = mapper;
            _BaseUrl= configuration.GetValue<string>("BaseURL");
        }

        public async Task<List<CommentResponseDTO>> GetAllCommentsByPostIdAsync(int postId)
        {
            var comments = await _commentRepository.GetCommentsByPostIdAsync(postId);
            var CommentResponseDtos = comments.Select(comment => new CommentResponseDTO()
            {
                CommentId = comment.Id,
                Content = comment.Content,
                PostId = comment.PostId,
                PublisherId = comment.UserId,
                PublisherName = comment.User.FullName,
                PublisherImage = comment.User?.Image != null ? $"https://localhost:7208{comment.User.Image}" : null,
                CreatedAt = comment.CreatedAt
            }).ToList();

            return CommentResponseDtos;
        }

    
        public async Task<CommentResponseDTO> AddCommentAsync(CommentDTO commentDto, string currentUserId, string currentUserName, string? currentUserImage,int Postid)
        {
            var comment = new Comment()
            {
                Content = commentDto.Content,
                UserId = currentUserId,
                CreatedAt = DateTime.UtcNow,
                PostId = Postid,
                User = new UserApp
                {
                    FullName = currentUserName,
                    Image = currentUserImage
                }
            };

      
            await _commentRepository.AddAsync(comment);



            return new CommentResponseDTO()
            {
                CommentId = comment.Id,
                Content = comment.Content,
                PostId = comment.PostId,
                PublisherId = comment.UserId,
                PublisherName = comment.User.FullName,
                PublisherImage = comment.User?.Image != null ? $"{_BaseUrl}{comment.User.Image}" : null,
                CreatedAt = comment.CreatedAt
            };
          
        
        }


        public async Task<CommentResponseDTO> UpdateCommentAsync(int postId, int commentId, string newContent)
        {
            var comment = await _commentRepository.GetCommentByPostIdAndCommentIdAsync(postId,commentId);

            comment.Content = newContent;

            await _commentRepository.UpdateAsync(comment);

            return new CommentResponseDTO()
            {
                CommentId = comment.Id,
                Content = comment.Content,
                PostId = comment.PostId,
                PublisherId = comment.UserId,
                PublisherName = comment.User.FullName,
                PublisherImage = comment.User?.Image != null ? $"{_BaseUrl}{comment.User.Image}" : null,
                CreatedAt = comment.CreatedAt
            };
        }

        public async Task<CommentDeleteResult> DeleteCommentAsync(int postId, int commentId)
        {
            var comment = await _commentRepository.GetByIdAsync(commentId);


            await _commentRepository.DeleteAsync(commentId); 
            await _commentRepository.SaveChangesAsync();  

            return new CommentDeleteResult { IsSuccess = true };  
        }






    }
}
