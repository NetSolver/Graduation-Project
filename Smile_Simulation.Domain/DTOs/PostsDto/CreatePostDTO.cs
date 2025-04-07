using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace Smile_Simulation.Domain.DTOs.PostsDto
{
    public class CreatePostDTO
    {
       
       
        public string? Content { get; set; }

        public IFormFile? Image { get; set; }

    }
}