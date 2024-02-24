using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace THUVIEN_CNPMNC_TH.Models
{
    public class ViewModel
    {
        public List<Book> Books { get; set; }
        public List<DocGia> DocGias { get; set; }

        public List<TheDocGia> TheDocGias { get; set; }
        public List<TheLoai> TheLoai { get; set; }
        public List<NhaXuatBan> NXB { get; set; }
        public List<Book> Books2 { get; set; }
        public int SoSachMuons { get; set; }

        public List<PhieuMuonTra> BorrowingHistory { get; set; }
    }
    public class ThemPhieuPhatViewModel
    {
        public string MaPhieu { get; set; }
        public PhieuPhat PhieuPhat { get; set; }
    }
}