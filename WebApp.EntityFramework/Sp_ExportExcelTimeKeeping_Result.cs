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
    
    public partial class Sp_ExportExcelTimeKeeping_Result
    {
        public int id { get; set; }
        public string Fullname { get; set; }
        public string AccoutType { get; set; }
        public string NameLocation { get; set; }
        public string job_Detail_Name { get; set; }
        public string JobDescription { get; set; }
        public string Username { get; set; }
        public bool isConfirm { get; set; }
        public bool isStatus { get; set; }
        public Nullable<double> sumTime { get; set; }
        public System.DateTime Time_In { get; set; }
        public System.DateTime Time_Out { get; set; }
        public int IDUserJob { get; set; }
    }
}
