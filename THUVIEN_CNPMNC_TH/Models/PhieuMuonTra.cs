//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace THUVIEN_CNPMNC_TH.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class PhieuMuonTra
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public PhieuMuonTra()
        {
            this.PhieuPhats = new HashSet<PhieuPhat>();
        }
    
        public string MaPhieu { get; set; }
        public string MaSach { get; set; }
        public string MaDocGia { get; set; }
        public string MaThuThu { get; set; }
        public Nullable<System.DateTime> NgayMuon { get; set; }
        public Nullable<System.DateTime> NgayTraDuKien { get; set; }
        public Nullable<System.DateTime> NgayTraThucTe { get; set; }
        public string TrangThai { get; set; }
    
        public virtual Book Book { get; set; }
        public virtual DocGia DocGia { get; set; }
        public virtual ThuThu ThuThu { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<PhieuPhat> PhieuPhats { get; set; }
    }
}
