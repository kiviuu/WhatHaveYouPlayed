using Microsoft.Build.Framework;
using System.ComponentModel.DataAnnotations.Schema;

namespace WhatHaveYouPlayed.Models
{
    public class BlogPost
    {
        public string PostId { get; set; }
        [System.ComponentModel.DataAnnotations.Required(ErrorMessage="The Title field is required.")]
        public string Title { get; set; }
        public DateTime PostDate { get; set; }
        public string AuthorId { get; set; }
        public virtual ApplicationUser Author { get; set; }
        [System.ComponentModel.DataAnnotations.Required(ErrorMessage = "The Content field is required.")]
        public string Content { get; set; }
#nullable enable
        public string? PostImgSrc { get; set; }
    }
}
