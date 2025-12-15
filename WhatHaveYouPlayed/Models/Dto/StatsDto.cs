namespace WhatHaveYouPlayed.Models.Dto
{
    public class StatsDto
    {
        public int Playing { get; set; }
        public int Complete { get; set; }
        public int Planning { get; set; }
        public int Awaiting { get; set; }
        public int Dropped { get; set; }
        public int Added { get; set; }
    }
}
