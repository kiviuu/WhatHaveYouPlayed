using Microsoft.Build.Framework;
using System.ComponentModel.DataAnnotations.Schema;

namespace WhatHaveYouPlayed.Models
{
    // dictonary table for progress states like "Playing", "Completed", "On Hold", etc.
    public class ProgressState
    {
        public int ProgressId { get; set; }
        public string State { get; set; }
        public virtual List<UserGameData> UserGameData { get; set; }
    }
}
