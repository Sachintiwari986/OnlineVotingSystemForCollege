using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace OnlineVotingSystemForCollege.Models.ViewModel
{
    public class ResultViewModel
    {
       
        [Required]
        public String  CandidateName { get; set; }
        [Required]
        public String Photo { get; set; }
        [Required]

        public String ElectionName { get; set; }
        [Required]


        public int? VoteObtained { get; set; }

    }
}