using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace SavacFarma.Shared.DTOs
{
    public class UserInfo
    {
        [Required]
        [Display(Name ="User", ShortName = "User")]
        public string Usuario { get; set; }
        [Required]
        public string Password { get; set; }
    }
}
