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
    
    public partial class ct_phieuNhap_QLK
    {
        public string Machitiet_QLK { get; set; }
        public string Maphieunhap_QLK { get; set; }
        public string Masanpham_QLK { get; set; }
        public Nullable<int> Soluong_QLK { get; set; }
        public Nullable<int> Gianhap_QLK { get; set; }
    
        public virtual phieuNhap_QLK phieuNhap_QLK { get; set; }
    }
}
