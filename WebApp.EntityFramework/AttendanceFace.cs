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
    
    public partial class AttendanceFace
    {
        public int id { get; set; }
        public int IdUserJob { get; set; }
        public string Location_In_X { get; set; }
        public string Location_In_Y { get; set; }
        public string Location_Out_X { get; set; }
        public string Location_Out_Y { get; set; }
        public Nullable<int> Check_Location_In { get; set; }
        public Nullable<int> Check_Location_Out { get; set; }
        public System.DateTime Time_In { get; set; }
        public System.DateTime Time_Out { get; set; }
        public string Admin_UserName_Update { get; set; }
        public string Admin_UserName_Confirm { get; set; }
        public string Admin_Messger_Update { get; set; }
        public System.DateTime Admin_time_Update { get; set; }
        public System.DateTime TimeConfirm { get; set; }
        public bool isConfirm { get; set; }
        public bool isStatus { get; set; }
        public string Datetime_ { get; set; }
        public Nullable<double> sumTime { get; set; }
        public Nullable<int> UserJobId { get; set; }
    
        public virtual UserJob UserJob { get; set; }
    }
}
