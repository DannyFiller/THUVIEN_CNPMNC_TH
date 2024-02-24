

using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using THUVIEN_CNPMNC_TH.Models;

namespace THUVIEN_CNPMNC_TH.Areas.Admin.Controllers
{
    public class PhieuPhatController : Controller
    {
        // GET: Admin/PhieuPhat
        LIBRARY_FINALEntities1 database = new LIBRARY_FINALEntities1();
        public ActionResult QLPhieuPhat()
        {
            return View(database.PhieuPhats.ToList());
        }

        public ActionResult ThemPhieuPhat(string MaPhieuMuon, int soNgayTre)
        {
            if (string.IsNullOrEmpty(MaPhieuMuon))
            {
                return RedirectToAction("QLPhieuPhat");
            }

            var phieuPhat = new PhieuPhat { MaPhieuMuon = MaPhieuMuon, SoNgayTre = soNgayTre, MaPhieuPhat = GenerateNextMaPhieuPhat() };
            return View(phieuPhat);
        }

        [HttpPost]
        public ActionResult ThemPhieuPhat(PhieuPhat phieuPhat)
        {
            if (ModelState.IsValid)
            {
                if (phieuPhat.NgayPhat > DateTime.Now)
                {
                    ModelState.AddModelError("NgayPhat", "Ngày phạt không được lớn hơn ngày hiện tại.");
                }

                if (phieuPhat.SoTienPhat <= 0)
                {
                    ModelState.AddModelError("SoTienPhat", "Số tiền phạt không được nhỏ hơn 0.");
                }

                if (ModelState.IsValid)
                {
                    try
                    {
                        database.PhieuPhats.Add(phieuPhat);
                        database.SaveChanges();
                        return RedirectToAction("QLPhieuPhat");
                    }
                    catch (Exception ex)
                    {
                        ModelState.AddModelError("", "Đã xảy ra lỗi khi thêm phiếu phạt: " + ex.Message);
                    }
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


        [HttpPost]
        public ActionResult XoaPhieuPhat(string id)
        {
            var phieuPhat = database.PhieuPhats.Find(id);

            if (phieuPhat == null)
            {
                return Json(new { success = false });
            }

            database.PhieuPhats.Remove(phieuPhat);
            database.SaveChanges();

            return Json(new { success = true });
        }


        public ActionResult QLTaiChinh()
        {
            var totalPhieuPhatByMonth = new List<int>();

            for (int i = 1; i <= 12; i++)
            {
                int totalAmount = database.PhieuPhats
                    .Where(pp => pp.NgayPhat != null && pp.NgayPhat.Value.Month == i)
                    .Sum(pp => pp.SoTienPhat) ?? 0;
                totalPhieuPhatByMonth.Add(totalAmount);
            }

            ViewBag.TotalPhieuPhatByMonth = totalPhieuPhatByMonth;
            return View();
        }

    }
}