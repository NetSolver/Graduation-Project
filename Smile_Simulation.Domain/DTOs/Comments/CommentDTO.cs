using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Smile_Simulation.Domain.DTOs.Comments
{
    public class CommentDTO
    {
        [Required(ErrorMessage = "Content is required.")]
        public string Content { get; set; }

    }
}
