using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace OnlineVotingSystemForCollege.Models.ViewModel
{
    public class ChangePasswordViewModel
    {
        public String UserName { get; set; }
        [Required]
        public String OldPassword { get; set; }
        [Required]
        public String NewPassword { get; set; }
        [Compare("NewPassword", ErrorMessage = "Password Mismatch")]
        public String ConfirmNew { get; set; }
    }
}