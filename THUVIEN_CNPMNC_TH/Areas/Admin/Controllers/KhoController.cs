using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using THUVIEN_CNPMNC_TH.Models;

namespace THUVIEN_CNPMNC_TH.Areas.Admin.Controllers
{
    public class KhoController : Controller
    {
        LIBRARY_FINALEntities1 database = new LIBRARY_FINALEntities1();

        // GET: Admin/Kho
        public ActionResult QLKho()
        {
            return View(database.PhieuNhaps.ToList());
        }

        // GET: Admin/Kho/NhapSach
        public ActionResult NhapSach()
        {
            var listBooks = database.NhanVienKhoes.ToList();
            ViewBag.TenNhanVienKho = new SelectList(listBooks, "MaNhanVienKho", "TenNhanVienKho");
            var nextMaPN = GenerateNextMaPN();
            var phieuNhap = new PhieuNhap { MaPN = nextMaPN };
            return View(phieuNhap);
        }

        // POST: Admin/Kho/NhapSach
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult NhapSach(PhieuNhap phieuNhap)
        {
            if (ModelState.IsValid)
            {
                // Kiểm tra xem có sách nào có tên giống với phieuNhap.TenSach không
                var existingBook = database.PhieuNhaps.FirstOrDefault(p => p.TenSach == phieuNhap.TenSach);

                if (existingBook != null)
                {
                    // Sách đã tồn tại, cộng thêm số lượng vào sách có sẵn
                    existingBook.SoLuong += phieuNhap.SoLuong;
                }
                else
                {
                    // Sách không tồn tại, tạo phiếu nhập mới
                    database.PhieuNhaps.Add(phieuNhap);
                }

                // Lưu thay đổi vào cơ sở dữ liệu
                database.SaveChanges();

                // Redirect to the inventory list page
                return RedirectToAction("QLKho");
            }

            var listBooks = database.NhanVienKhoes.ToList();
            ViewBag.TenNhanVienKho = new SelectList(listBooks, "MaNhanVienKho", "TenNhanVienKho");
            return View(phieuNhap);
        }



        // Helper method to generate the next MaPN
        private string GenerateNextMaPN()
        {
            string defaultMaPN = "PN001";
            string nextMaPN = defaultMaPN;

            while (database.PhieuNhaps.Any(p => p.MaPN == nextMaPN))
            {
                string numericPart = nextMaPN.Substring(2);

                if (int.TryParse(numericPart, out int numericValue))
                {
                    numericValue++;
                    nextMaPN = $"PN{numericValue:D3}";
                }
                else
                {
                    nextMaPN = defaultMaPN;
                    break;
                }
            }

            return nextMaPN;
        }
        [HttpPost]
        public ActionResult DeletePhieuNhap(string maPN)
        {
            var phieuNhap = database.PhieuNhaps.FirstOrDefault(d => d.MaPN == maPN);
            if (phieuNhap != null)
            {
                database.PhieuNhaps.Remove(phieuNhap);
                database.SaveChanges();
                return Json(new { success = true });
            }
            else
            {
                return Json(new { success = false });
            }
        }
    }
}