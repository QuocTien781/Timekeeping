//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace ToolsApp.EntityFramework
{
    using System;
    using System.Collections.Generic;
    
    public partial class UserJob
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public UserJob()
        {
            this.AttendanceFaces = new HashSet<AttendanceFace>();
            this.PleaseTakeLeaves = new HashSet<PleaseTakeLeave>();
        }
    
        public int Id { get; set; }
        public string Username { get; set; }
        public int Id_Detail_Job { get; set; }
        public Nullable<int> JobDetailId { get; set; }
        public bool Status { get; set; }
        public string UserId { get; set; }
        public string UsernameDelete { get; set; }
        public System.DateTime timeCreate { get; set; }
        public System.DateTime timeDelete { get; set; }
    
        public virtual AspNetUser AspNetUser { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<AttendanceFace> AttendanceFaces { get; set; }
        public virtual JobDetail JobDetail { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<PleaseTakeLeave> PleaseTakeLeaves { get; set; }
    }
}
