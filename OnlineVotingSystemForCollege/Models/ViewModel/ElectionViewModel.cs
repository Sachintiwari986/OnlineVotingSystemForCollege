using Foolproof;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace OnlineVotingSystemForCollege.Models.ViewModel
{
    public class ElectionViewModel
    {
        public int ElectionId { get; set; }
        [Required(ErrorMessage = "Enter Election Name")]
        public string ElectionName { get; set; }
        [Required(ErrorMessage = "Select Start Date")]
        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy}")]
        public string ElectionStartDate { get; set; }
        [Required (ErrorMessage ="Select End Date")]
        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy}")]
        [GreaterThan("ElectionStartDate", ErrorMessage = "Election End Date is before than Start Date")]

        public string ElectionEndDate { get; set; }
        [Required]
        public int? NoOfCandidate { get; set; }
    }
}