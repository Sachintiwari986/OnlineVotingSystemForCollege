using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace OnlineVotingSystemForCollege.Models.ViewModel
{
    public class CandidateViewModel
    {
        public int CandidateId { get; set; }
        [Required]
        public string CandidateName { get; set; }
        [Required]
        public int? ElectionId { get; set; }
        [Required]
        public String ElectionName { get; set; }

        [Required]
        public string Description { get; set; }
        [Required]
        public string Photo { get; set; }
    }
}