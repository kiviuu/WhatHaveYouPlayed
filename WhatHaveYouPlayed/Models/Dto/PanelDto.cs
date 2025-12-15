using System.ComponentModel.DataAnnotations;
using System.Web;
namespace WhatHaveYouPlayed.Models.Dto
{
    public class PanelDto
    {
        public ApplicationUser User { get; set; }
        public BlogPost BlogPost { get; set; }
        public GameData GameData { get; set; }
        public List<UserMessage> userMessages { get; set; }
        public List<string> StatusMessage { get; set; } = null;
        public IFormFile PostImage { get; set; }
        [Required(ErrorMessage = "The file is required.")]
        public IFormFile GameImage { get; set; }

        public List<BlogPost> blogPosts { get; set; }
        public List<Producent> Producents { get; set; }
        public List<GameData> games { get; set; }
    }
}
