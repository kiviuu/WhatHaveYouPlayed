using Microsoft.Build.Framework;
using System.ComponentModel.DataAnnotations.Schema;
namespace WhatHaveYouPlayed.Models
{
    public class UserMessage
    {
        public int MessageId { get; set; }
        public string UserId { get; set; }
        public virtual ApplicationUser User { get; set; }
        [Required]
        public string Topic { get; set; }
        [Required]
        public string Content { get; set; }
        public DateTime CreateTime { get; set; }
    }
}
