using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Smile_Simulation.Domain.DTOs.Comments;
using Smile_Simulation.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Smile_Simulation.Domain.Entities
{
    public class UserApp : IdentityUser
    {
        public string FullName { get; set; }
        public string? Image { get; set; }
        public Gender gender { get; set; }
        public string? Address {  get; set; }
        public DateOnly? BirthDay { get; set; }
        public ICollection<Post> Posts { get; set; } = new List<Post>();
        public ICollection<Like> Likes { get; set; } = new List<Like>();
        public ICollection<Comment> Comments { get; set; } = new List<Comment>();


    }
}
