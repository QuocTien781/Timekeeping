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
    
    public partial class PleaseTakeLeave
    {
        public int Id { get; set; }
        public string username { get; set; }
        public string messger { get; set; }
        public string phones { get; set; }
        public System.DateTime timeStart { get; set; }
        public System.DateTime timeEnd { get; set; }
        public int idUserJob { get; set; }
        public bool isComfim { get; set; }
        public System.DateTime DateTime { get; set; }
        public Nullable<int> UserJobId { get; set; }
        public System.DateTime TimeConfim { get; set; }
        public string UserConfim { get; set; }
        public string UserId { get; set; }
    
        public virtual AspNetUser AspNetUser { get; set; }
        public virtual UserJob UserJob { get; set; }
    }
}
