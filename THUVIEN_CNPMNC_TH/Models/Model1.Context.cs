﻿//------------------------------------------------------------------------------
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
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class LIBRARY_FINALEntities1 : DbContext
    {
        public LIBRARY_FINALEntities1()
            : base("name=LIBRARY_FINALEntities1")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<Book> Books { get; set; }
        public virtual DbSet<DocGia> DocGias { get; set; }
        public virtual DbSet<NhanVienKho> NhanVienKhoes { get; set; }
        public virtual DbSet<NhanVienQuanLyDocGia> NhanVienQuanLyDocGias { get; set; }
        public virtual DbSet<NhaXuatBan> NhaXuatBans { get; set; }
        public virtual DbSet<PhieuMuonTra> PhieuMuonTras { get; set; }
        public virtual DbSet<PhieuNhap> PhieuNhaps { get; set; }
        public virtual DbSet<PhieuPhat> PhieuPhats { get; set; }
        public virtual DbSet<TheDocGia> TheDocGias { get; set; }
        public virtual DbSet<TheLoai> TheLoais { get; set; }
        public virtual DbSet<ThuThu> ThuThus { get; set; }
        public virtual DbSet<UserGroup> UserGroups { get; set; }
        public virtual DbSet<User> Users { get; set; }
    }
}