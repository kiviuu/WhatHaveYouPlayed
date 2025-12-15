using Microsoft.AspNetCore.Mvc;
using Microsoft.Build.Framework;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WhatHaveYouPlayed.Models
{
    public class GameData
    {
        public string GameId { get; set; }
        [DisplayName("Game Name")]
        [System.ComponentModel.DataAnnotations.Required]
        public string Name { get; set; }
        public int ProducentId { get; set; }
        public virtual Producent Producent { get; set; }
        [DisplayName("Release Date")]
        [System.ComponentModel.DataAnnotations.Required]
        public DateTime ReleaseDate { get; set; }
        [DisplayName("Age")]
        [System.ComponentModel.DataAnnotations.Required]
        public int PegiAge { get; set; }
        //Image name in storage
        [DisplayName("Image")]
        public string ImgSrc { get; set; }

        public virtual List<UserGameData> UserGameData { get; set; }
    }
}
