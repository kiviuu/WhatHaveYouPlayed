//using Microsoft.Build.Framework;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace WhatHaveYouPlayed.Models.Dto
{
    public class ChangeDataDto
    {
        [Required]
        [DisplayName("Password")]
        [DataType(DataType.Password)]
        public string OldPassword { get; set; }
        [Required]
        [DisplayName("New Password")]
        [DataType(DataType.Password)]
        public string NewPassword { get; set; }
        [Required]
        [DisplayName("Confirm Password")]
        [DataType(DataType.Password)]
        [Compare("NewPassword", ErrorMessage = "The new password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }

        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string OldEmail { get; set; }
        [Required]
        [EmailAddress]
        [Display(Name = "New Email")]
        public string NewEmail { get; set; }

        [Required]
        [Display(Name = "Confirm Code")]
        public string ConfirmDeleteId { get; set; }
    }
}
