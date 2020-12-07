using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace OnlineVotingSystemForCollege.Models.ViewModel
{
    public class RegistrationViewModel
    {
        [Required(AllowEmptyStrings = false, ErrorMessage = "Enter Your User Name")]
        public String UserName { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Enter Your Email")]
        public String Email { get; set; }
        [Required(AllowEmptyStrings = false, ErrorMessage = "Enter Your Full Name")]
        public String FullName { get; set; }
        [Required(AllowEmptyStrings = false, ErrorMessage = "Enter Your Password")]

        [DataType(DataType.Password)]
        [MinLength(6, ErrorMessage = "Need Min 6 character")]


        public String Password { get; set; }
        [Required(AllowEmptyStrings = false, ErrorMessage = "Confirm Your Password")]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Confirm Password should Match With Password")]
        public String ConfirmPassword { get; set; }
    }
}