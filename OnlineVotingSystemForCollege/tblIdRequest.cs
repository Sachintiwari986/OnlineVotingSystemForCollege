//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace OnlineVotingSystemForCollege
{
    using System;
    using System.Collections.Generic;
    
    public partial class tblIdRequest
    {
        public int IdRequestId { get; set; }
        public string UserEmail { get; set; }
        public string FullName { get; set; }
        public Nullable<int> UserId { get; set; }
    
        public virtual tblUser tblUser { get; set; }
    }
}
