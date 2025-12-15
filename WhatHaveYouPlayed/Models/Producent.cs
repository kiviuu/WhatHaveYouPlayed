using Microsoft.Build.Framework;
using System.ComponentModel.DataAnnotations.Schema;

namespace WhatHaveYouPlayed.Models
{
    public class Producent
    {
        public int ProdId { get; set; }
        [System.ComponentModel.DataAnnotations.Required(ErrorMessage = "The name of producent is required.")]
        public string CompanyName { get; set; }
        public virtual List<GameData> GameData { get; set; }
    }
}
