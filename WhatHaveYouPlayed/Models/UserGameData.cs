using Microsoft.Build.Framework;
using System.ComponentModel.DataAnnotations.Schema;

namespace WhatHaveYouPlayed.Models
{
    public class UserGameData
    {
        public int DataId { get; set; }
        public string UserId { get; set; }
        public virtual ApplicationUser User { get; set; }
        public string GameId { get; set; }
        public virtual GameData Game { get; set; }
        public int StateId { get; set; }
        public virtual ProgressState State { get; set; }
        public DateTime AddDate { get; set; }
    }
}
