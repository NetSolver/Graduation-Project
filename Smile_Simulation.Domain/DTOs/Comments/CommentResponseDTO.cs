using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Smile_Simulation.Domain.DTOs.Comments
{
    public class CommentResponseDTO
    {
        public int CommentId { get; set; }

       
        public string Content { get; set; }

       
        public int PostId { get; set; }

        public string PublisherId { get; set; }

        public string PublisherName { get; set; }
        public string PublisherImage { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
