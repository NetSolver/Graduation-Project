using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Smile_Simulation.Domain.Entities
{
    public class Like
    {
        public int PostId { get; set; }
        public Post Post { get; set; }
        public string UserId { get; set; }
        public UserApp User { get; set; }
    }
}
