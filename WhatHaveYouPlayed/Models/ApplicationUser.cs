using Microsoft.AspNetCore.Identity;

namespace WhatHaveYouPlayed.Models
{
    public class ApplicationUser : IdentityUser
    {
        public DateTime CreateDate { get; set; }
        public virtual List<BlogPost> BlogPosts { get; set; }
        public virtual List<UserGameData> UserGameDatas { get; set; }
        public virtual List<UserMessage> UserMessages { get; set; }
    }
}