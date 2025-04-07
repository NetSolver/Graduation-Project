using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.Extensions.Configuration;
using Smile_Simulation.Domain.DTOs.PostsDto;
using Smile_Simulation.Domain.Entities;
using Smile_Simulation.Domain.Interfaces.Repositories;
using Smile_Simulation.Infrastructure.Repositories;

namespace Smile_Simulation.Application.Services
{
    public class PostService
    {
        private readonly IPostRepository _postRepository;
        private readonly ILikeRepository _likeRepository;
        private readonly IMapper _mapper;
        private readonly string _BaseUrl;

        public PostService(IPostRepository postRepository, ILikeRepository likeRepository, IMapper mapper, IConfiguration configuration)
        {
            _postRepository = postRepository;
            _likeRepository = likeRepository;
            _mapper = mapper;
            _BaseUrl = configuration.GetValue<string>("BaseURL");
        }


        public async Task<PostDTO> GetPostByIdAsync(int postId, string currentUserId)
        {
            var post = await _postRepository.GetByPostIdAsync(postId);
            if (post == null) return null;

            return new PostDTO()
            {
                Id = postId,
                Content = post.Content,
                PublisherId = post.PublisherId,
                ImageUrl = post.Image != null ? $"{_BaseUrl}{post.Image}" : null,
                CreatedAt = post.CreatedAt,
                CommentsCount = post.Comments.Count(),
                IsLikedByCurrentUser = await _likeRepository.IsPostLikedByUserAsync(postId, currentUserId),
                LikesCount = post.Likes.Count,
            };

        }


        public async Task<PostDTO> AddPostAsync(CreatePostDTO createPostDto, string publisherId)
        {
            if (createPostDto == null)
            {
                throw new ArgumentNullException(nameof(createPostDto), "CreatePostDTO is null.");
            }

            var post = new Post
            {
                Content = createPostDto.Content,
                PublisherId = publisherId,
                CreatedAt = DateTime.UtcNow
            };

            if (createPostDto.Image != null && createPostDto.Image.Length > 0)
            {
                var fileName = $"{Guid.NewGuid()}{Path.GetExtension(createPostDto.Image.FileName)}";
                var imagePath = Path.Combine("wwwroot/posts", fileName);

                try
                {
                    using (var stream = new FileStream(imagePath, FileMode.Create))
                    {
                        await createPostDto.Image.CopyToAsync(stream);
                    }
                    post.Image = $"/posts/{fileName}";
                }
                catch (Exception ex)
                {
                    throw new Exception("فشل في حفظ الصورة", ex);
                }
            }

            Post savedPost = await _postRepository.AddAsync(post);
         
            return new PostDTO()
            {
                Id = post.Id,
                Content = post.Content,
                PublisherId = post.PublisherId,
                ImageUrl = post.Image != null ? $"{_BaseUrl}{post.Image}" : null,
                CreatedAt = post.CreatedAt,
                LikesCount = post.Likes.Count(),
                CommentsCount = post.Comments.Count(),

            };

       
        }


        public async Task<bool> DeletePostAsync(int postId)
        {
            var post = await _postRepository.GetByIdAsync(postId);
            if (post == null) return false;

            _postRepository.DeleteAsync(postId);
            return true;
        }




        public async Task<PostDTO> UpdatePostAsync(int postId, CreatePostDTO updatePostDto)
        {
            var post = await _postRepository.GetByPostIdAsync(postId);
            if (post == null) return null;

            if(!string.IsNullOrEmpty(updatePostDto.Content))
            post.Content = updatePostDto.Content;

            if (updatePostDto.Image != null && updatePostDto.Image.Length > 0)
            {
                var fileName = $"{Guid.NewGuid()}{Path.GetExtension(updatePostDto.Image.FileName)}";
                var imagePath = Path.Combine("wwwroot/posts", fileName);

                try
                {
                    using (var stream = new FileStream(imagePath, FileMode.Create))
                    {
                        await updatePostDto.Image.CopyToAsync(stream);
                    }
                    post.Image = $"/posts/{fileName}";
                }
                catch (Exception ex)
                {
                    throw new Exception("فشل في حفظ الصورة", ex);
                }
            }

           await _postRepository.UpdateAsync(post);
           return new PostDTO()
           {
               Id = postId,
               Content = post.Content,
               PublisherId = post.PublisherId,
               ImageUrl = post.Image != null ? $"{_BaseUrl}{post.Image}" : null,
               CreatedAt = post.CreatedAt,
               CommentsCount = post.Comments.Count(),
               IsLikedByCurrentUser = await _likeRepository.IsPostLikedByUserAsync(postId, post.PublisherId),
               LikesCount = post.Likes.Count,
           };

        }

    }
}

