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
    
    public partial class kho_QLK
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public kho_QLK()
        {
            this.Kekho_QLK = new HashSet<Kekho_QLK>();
            this.Tangkekho_QLK = new HashSet<Tangkekho_QLK>();
        }
    
        public string Makho_QLK { get; set; }
        public string Tenkho_QLK { get; set; }
        public string Diachi_QLK { get; set; }
        public string Thanhpho_QLK { get; set; }
        public string Quanhuyen_QLK { get; set; }
        public string Phuongxa_QLK { get; set; }
        public Nullable<bool> Hieuluc_QLK { get; set; }
        public string Nguoitao_QLK { get; set; }
        public Nullable<System.DateTime> Ngaytao_QLK { get; set; }
        public string Nguoicapnhat_QLK { get; set; }
        public Nullable<System.DateTime> Ngaycapnhat_QLK { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Kekho_QLK> Kekho_QLK { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Tangkekho_QLK> Tangkekho_QLK { get; set; }
    }
}
