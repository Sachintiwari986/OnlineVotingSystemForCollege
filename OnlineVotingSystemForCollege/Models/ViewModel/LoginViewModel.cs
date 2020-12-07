using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace OnlineVotingSystemForCollege.Models.ViewModel
{
    public class LoginViewModel
    {
        public int UserId { get; set; }
        [Required]
        public String UserName { get; set; }
        [Required]
        public String Password { get; set; }

        public bool RememberMe { get; set; }
    
    }
}
