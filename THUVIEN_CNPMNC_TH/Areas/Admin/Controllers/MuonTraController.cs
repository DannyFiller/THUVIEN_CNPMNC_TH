using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using THUVIEN_CNPMNC_TH.Models;

namespace THUVIEN_CNPMNC_TH.Areas.Admin.Controllers
{
    public class MuonTraController : Controller
    {
        // GET: Admin/MuonTra
        private LIBRARY_FINALEntities1 database = new LIBRARY_FINALEntities1();
        public ActionResult QLMuonTra()
        {
            return View(database.PhieuMuonTras.ToList());
        }

        [HttpGet]
        public ActionResult ThemMuonTra()
        {

            var listBooks = database.Books.Where(b => b.TrungBay == true).ToList();
            ViewBag.TenSach = new SelectList(listBooks, "MaSach", "TenSach");

            var listThuThu = database.ThuThus.ToList();
            ViewBag.TenThuThu = new SelectList(listThuThu, "MaThuThu", "TenThuThu");

            var listDocGia = database.DocGias.ToList();
            ViewBag.TenDocGia = new SelectList(listDocGia, "MaDocGia", "TenDocGia");

            return View();
        }
        [HttpPost]
        public ActionResult ThemMuonTra(PhieuMuonTra phieuMuonTra)
        {
            if (ModelState.IsValid)
            {
                if (phieuMuonTra.NgayMuon.HasValue && phieuMuonTra.NgayTraDuKien.HasValue &&
                    phieuMuonTra.NgayTraDuKien.Value > phieuMuonTra.NgayMuon.Value.AddDays(15))
                {
                    ModelState.AddModelError("NgayTra", "Ngày trả không được lớn hơn 15 ngày so với ngày mượn.");
                }

                if (ModelState.IsValid)
                {

                    phieuMuonTra.MaPhieu = GenerateNextMaPhieu();

                    var selectedBook = database.Books.FirstOrDefault(b => b.MaSach == phieuMuonTra.MaSach);
                    if (selectedBook != null)
                    {
                        phieuMuonTra.MaSach = selectedBook.MaSach;

                        if (selectedBook.SoLuong > 0)
                        {
                            selectedBook.LuotMuonTra++;
                            selectedBook.SoLuong--;
                        }
                        else
                        {
                            ModelState.AddModelError("MaSach", "Số lượng sách đã hết.");
                            return View(phieuMuonTra);
                        }
                    }

                    var selectedThuThu = database.ThuThus.FirstOrDefault(tt => tt.MaThuThu == phieuMuonTra.MaThuThu);
                    if (selectedThuThu != null)
                    {
                        phieuMuonTra.MaThuThu = selectedThuThu.MaThuThu;
                    }

                    var selectedDocGia = database.DocGias.FirstOrDefault(dg => dg.MaDocGia == phieuMuonTra.MaDocGia);
                    if (selectedDocGia != null)
                    {
                        phieuMuonTra.MaDocGia = selectedDocGia.MaDocGia;
                    }
                    phieuMuonTra.TrangThai = "Đang đăng ký";
                    database.PhieuMuonTras.Add(phieuMuonTra);
                    database.SaveChanges();
                    return RedirectToAction("QLMuonTra", "MuonTra");
                }
            }

            var listBooks = database.Books.ToList();
            ViewBag.TenSach = new SelectList(listBooks, "MaSach", "TenSach");
            var listThuThu = database.ThuThus.ToList();
            ViewBag.TenThuThu = new SelectList(listThuThu, "MaThuThu", "TenThuThu");
            var listDocGia = database.DocGias.ToList();
            ViewBag.TenDocGia = new SelectList(listDocGia, "MaDocGia", "TenDocGia");

            return View(phieuMuonTra);
        }

        private string GenerateNextMaPhieu()
        {
            string defaultMaPhieu = "PM001";
            string nextMaPhieu = defaultMaPhieu;

            while (database.PhieuMuonTras.Any(pmt => pmt.MaPhieu == nextMaPhieu))
            {
                string numericPart = nextMaPhieu.Substring(2);

                if (int.TryParse(numericPart, out int numericValue))
                {
                    numericValue++;
                    nextMaPhieu = $"PM{numericValue:D03}";
                }
                else
                {
                    nextMaPhieu = defaultMaPhieu;
                    break;
                }
            }

            return nextMaPhieu;
        }

        [HttpPost]
        public ActionResult XoaPhieuMuonTra(string id)
        {
            var phieuMuonTra = database.PhieuMuonTras.Find(id);

            if (phieuMuonTra == null)
            {
                return Json(new { success = false, message = "Không tìm thấy phiếu mượn trả." });
            }

            // Kiểm tra xem phiếu mượn trả có phiếu phạt liên quan hay không
            var phieuPhat = database.PhieuPhats.FirstOrDefault(pp => pp.MaPhieuMuon == id);
            if (phieuPhat != null)
            {
                return Json(new { success = false, message = "Không thể xóa phiếu mượn trả có phiếu phạt liên quan." });
            }

            database.PhieuMuonTras.Remove(phieuMuonTra);
            database.SaveChanges();
            return Json(new { success = true, message = "Xóa phiếu mượn trả thành công." });
        }


        [HttpGet]
        public ActionResult TraSach(string id)
        {
            var phieuMuonTra = database.PhieuMuonTras.Find(id);

            if (phieuMuonTra == null)
            {
                return HttpNotFound();
            }

            var listBooks = database.Books.Where(b => b.TrungBay == true).ToList();
            ViewBag.TenSach = new SelectList(listBooks, "MaSach", "TenSach");

            var listThuThu = database.ThuThus.ToList();
            ViewBag.TenThuThu = new SelectList(listThuThu, "MaThuThu", "TenThuThu");

            var listDocGia = database.DocGias.ToList();
            ViewBag.TenDocGia = new SelectList(listDocGia, "MaDocGia", "TenDocGia");

            return View(phieuMuonTra);
        }


        [HttpPost]
        public ActionResult TraSach(PhieuMuonTra phieuMuonTra)
        {
            if (ModelState.IsValid)
            {
                if (phieuMuonTra.NgayMuon.HasValue && phieuMuonTra.NgayTraDuKien.HasValue &&
                    phieuMuonTra.NgayTraDuKien.Value > phieuMuonTra.NgayMuon.Value.AddDays(15))
                {
                    ModelState.AddModelError("NgayTra", "Ngày trả không được lớn hơn 15 ngày so với ngày mượn.");
                }

                if (ModelState.IsValid)
                {
                    var existingPhieuMuonTra = database.PhieuMuonTras.Find(phieuMuonTra.MaPhieu);

                    if (existingPhieuMuonTra == null)
                    {
                        return HttpNotFound();
                    }
                    existingPhieuMuonTra.MaSach = phieuMuonTra.MaSach;
                    existingPhieuMuonTra.MaThuThu = phieuMuonTra.MaThuThu;
                    existingPhieuMuonTra.MaDocGia = phieuMuonTra.MaDocGia;
                    existingPhieuMuonTra.NgayMuon = phieuMuonTra.NgayMuon;
                    existingPhieuMuonTra.TrangThai = "Đã Trả";
                    existingPhieuMuonTra.NgayTraDuKien = phieuMuonTra.NgayTraDuKien;
                    existingPhieuMuonTra.NgayTraThucTe = phieuMuonTra.NgayTraThucTe;

                    database.SaveChanges();

                    return RedirectToAction("QLMuonTra");
                }
            }
            var listBooks = database.Books.Where(b => b.TrungBay == true).ToList();
            ViewBag.TenSach = new SelectList(listBooks, "MaSach", "TenSach");

            var listThuThu = database.ThuThus.ToList();
            ViewBag.TenThuThu = new SelectList(listThuThu, "MaThuThu", "TenThuThu");

            var listDocGia = database.DocGias.ToList();
            ViewBag.TenDocGia = new SelectList(listDocGia, "MaDocGia", "TenDocGia");

            return View(phieuMuonTra);
        }

        [HttpGet]
        public ActionResult ThemPhieuPhat(string id)
        {
            var phieuMuonTra = database.PhieuMuonTras.Find(id);

            if (phieuMuonTra == null)
            {
                return HttpNotFound();
            }

            PhieuPhat phieuPhat = new PhieuPhat();
            phieuPhat.MaPhieuMuon = id;

            DateTime ngayTraThucTe = phieuMuonTra.NgayTraThucTe ?? DateTime.Now;
            DateTime ngayTraDuKien = phieuMuonTra.NgayTraDuKien ?? DateTime.Now;
            TimeSpan soNgayTre = ngayTraThucTe - ngayTraDuKien;
            int soNgayTrePositive = Math.Max(0, soNgayTre.Days); 
            phieuPhat.SoNgayTre = soNgayTrePositive;

            if (soNgayTrePositive <= 3)
            {
                phieuPhat.SoTienPhat = soNgayTrePositive * 10000;
            }
            else if (soNgayTrePositive <= 5)
            {
                phieuPhat.SoTienPhat = soNgayTrePositive * 15000;
            }
            else
            {
                phieuPhat.SoTienPhat = soNgayTrePositive * 20000;
            }

            return View(phieuPhat);
        }

        [HttpPost]
        public ActionResult ThemPhieuPhat(PhieuPhat phieuPhat)
        {
            if (ModelState.IsValid)
            {
                if (phieuPhat.NgayPhat > DateTime.Now)
                {
                    ModelState.AddModelError("NgayPhat", "Ngày trả không hợp lệ, ngày phạt phải bé hơn ngày hôm nay");
                }

                if (ModelState.IsValid)
                {
                    phieuPhat.NgayPhat = DateTime.Now;

                    phieuPhat.MaPhieuPhat = GenerateNextMaPhieuPhat();
                    database.PhieuPhats.Add(phieuPhat);
                    database.SaveChanges();

                    var phieuMuonTra = database.PhieuMuonTras.Find(phieuPhat.MaPhieuMuon);
                    if (phieuMuonTra != null)
                    {
                        // Cập nhật TrangThai thành "Đã Phạt"
                        phieuMuonTra.TrangThai = "Đã Phạt";
                        database.SaveChanges();
                    }

                    return RedirectToAction("QLMuonTra");
                }
            }

            return View(phieuPhat);
        }




        private string GenerateNextMaPhieuPhat()
        {
            string defaultMaPhieuPhat = "PP001";
            string nextMaPhieuPhat = defaultMaPhieuPhat;

            while (database.PhieuPhats.Any(pp => pp.MaPhieuPhat == nextMaPhieuPhat))
            {
                string numericPart = nextMaPhieuPhat.Substring(2);
                if (int.TryParse(numericPart, out int numericValue))
                {
                    numericValue++;
                    nextMaPhieuPhat = string.Format("PP{0:D3}", numericValue);
                }
                else
                {
                    nextMaPhieuPhat = defaultMaPhieuPhat;
                    break;
                }
            }

            return nextMaPhieuPhat;
        }


        [HttpGet]
        public ActionResult DuyetPhieu(string id)
        {
            var phieuMuonTra = database.PhieuMuonTras.Find(id);

            if (phieuMuonTra == null)
            {
                return HttpNotFound();
            }

            //phieuMuonTra.TrangThai = "Đang Mượn";

            var listBooks = database.Books.Where(b => b.TrungBay == true).ToList();
            ViewBag.TenSach = new SelectList(listBooks, "MaSach", "TenSach");

            var listThuThu = database.ThuThus.ToList();
            ViewBag.TenThuThu = new SelectList(listThuThu, "MaThuThu", "TenThuThu");

            var listDocGia = database.DocGias.ToList();
            ViewBag.TenDocGia = new SelectList(listDocGia, "MaDocGia", "TenDocGia");

            return View(phieuMuonTra);
        }


        [HttpPost]
        public ActionResult DuyetPhieu(PhieuMuonTra phieuMuonTra)
        {
            if (ModelState.IsValid)
            {
                if (phieuMuonTra.NgayMuon.HasValue && phieuMuonTra.NgayTraDuKien.HasValue &&
                    phieuMuonTra.NgayTraDuKien.Value > phieuMuonTra.NgayMuon.Value.AddDays(15))
                {
                    ModelState.AddModelError("NgayTra", "Ngày trả không được lớn hơn 15 ngày so với ngày mượn.");
                }

                if (ModelState.IsValid)
                {
                    var existingPhieuMuonTra = database.PhieuMuonTras.Find(phieuMuonTra.MaPhieu);

                    if (existingPhieuMuonTra == null)
                    {
                        return HttpNotFound();
                    }
                    existingPhieuMuonTra.MaSach = phieuMuonTra.MaSach;
                    existingPhieuMuonTra.MaThuThu = phieuMuonTra.MaThuThu;
                    existingPhieuMuonTra.MaDocGia = phieuMuonTra.MaDocGia;
                    existingPhieuMuonTra.NgayMuon = phieuMuonTra.NgayMuon;
                    existingPhieuMuonTra.TrangThai = "Đang Mượn";
                    existingPhieuMuonTra.NgayTraDuKien = phieuMuonTra.NgayTraDuKien;
                    existingPhieuMuonTra.NgayTraThucTe = phieuMuonTra.NgayTraThucTe;

                    database.SaveChanges();

                    return RedirectToAction("QLMuonTra");
                }
            }
            var listBooks = database.Books.Where(b => b.TrungBay == true).ToList();
            ViewBag.TenSach = new SelectList(listBooks, "MaSach", "TenSach");

            var listThuThu = database.ThuThus.ToList();
            ViewBag.TenThuThu = new SelectList(listThuThu, "MaThuThu", "TenThuThu");

            var listDocGia = database.DocGias.ToList();
            ViewBag.TenDocGia = new SelectList(listDocGia, "MaDocGia", "TenDocGia");

            return View(phieuMuonTra);
        }
    }
}