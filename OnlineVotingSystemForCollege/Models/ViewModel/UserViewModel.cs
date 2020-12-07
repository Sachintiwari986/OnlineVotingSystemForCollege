using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace OnlineVotingSystemForCollege.Models.ViewModel
{
    public class UserViewModel
    {
        public int? UserId { get; set; }
        [Required]
        public String UserName { get; set; }
        [Required]
        public String Password { get; set; }
        [Required]
        public String Email { get; set; }
        [Required]
        public String FullName { get; set; }
        [Required]
        public int IdRequestId { get; set; }
    }
}