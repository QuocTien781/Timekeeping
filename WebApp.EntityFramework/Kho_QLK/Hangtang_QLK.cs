//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace ToolsApp.EntityFramework.Kho_QLK
{
    using System;
    using System.Collections.Generic;
    
    public partial class Hangtang_QLK
    {
        public int Id { get; set; }
        public string Matang_QLK { get; set; }
        public string Masp_QLK { get; set; }
    
        public virtual sanpham_QLK sanpham_QLK { get; set; }
        public virtual Tangkekho_QLK Tangkekho_QLK { get; set; }
    }
}
