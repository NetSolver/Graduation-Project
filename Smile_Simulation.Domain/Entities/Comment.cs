using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Smile_Simulation.Domain.Entities
{
    public class Comment : BaseEntity<int>
    {
        public string Content { get; set; }
        public int PostId { get; set; }
        public Post Post { get; set; }
        public string UserId { get; set; }
        public UserApp User { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
