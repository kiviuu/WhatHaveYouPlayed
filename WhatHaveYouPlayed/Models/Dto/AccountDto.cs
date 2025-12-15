using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace WhatHaveYouPlayed.Models.Dto
{
    public class AccountDto
    {
        public string StatusMessage { get; set; } = null;
        public string Username { get; set; }
        public string Role { get; set; }
        [EmailAddress]
        public string Email { get; set; }
        [DisplayName("Join Date")]
        public DateTime CreateDate { get; set; }
        public ApplicationUser User { get; set; }
        public StatsDto Statistics { get; set; }
        public ChangeDataDto ChangeData { get; set; }
        public UserMessage UserMessage { get; set; }
    }
}
