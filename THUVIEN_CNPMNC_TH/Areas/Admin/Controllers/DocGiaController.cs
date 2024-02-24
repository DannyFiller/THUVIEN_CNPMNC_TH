using THUVIEN_CNPMNC_TH.Models;
using System;
using System.Linq;
using System.Web.Mvc;

namespace THUVIEN_CNPMNC_TH.Areas.Admin.Controllers
{
    public class DocGiaController : Controller
    {
        LIBRARY_FINALEntities1 database = new LIBRARY_FINALEntities1();

        // GET: Admin/DocGia
        public ActionResult QLDocGia()
        {
            return View(database.DocGias.ToList());
        }

        public ActionResult EditDocGia(string maDocGia)
        {
            var docGia = database.DocGias.FirstOrDefault(d => d.MaDocGia == maDocGia);
            if (docGia == null)
            {
                return HttpNotFound();
            }
            return View(docGia);
        }

        [HttpPost]
        public ActionResult SaveEdit(DocGia editedDocGia)
        {
            var existingDocGia = database.DocGias.FirstOrDefault(d => d.MaDocGia == editedDocGia.MaDocGia);
            if (existingDocGia == null)
            {
                return HttpNotFound();
            }

            existingDocGia.TenDocGia = editedDocGia.TenDocGia;
            existingDocGia.NgaySinh = editedDocGia.NgaySinh;
            existingDocGia.DiaChi = editedDocGia.DiaChi;
            existingDocGia.GioiTinh = editedDocGia.GioiTinh;
            existingDocGia.SoDienThoai = editedDocGia.SoDienThoai;
            database.SaveChanges();
            return RedirectToAction("QLDocGia");
        }

        [HttpPost]
        public ActionResult DeleteDocGia(string maDocGia)
        {
            var docGia = database.DocGias.FirstOrDefault(d => d.MaDocGia == maDocGia);
            if (docGia != null)
            {
                database.DocGias.Remove(docGia);
                database.SaveChanges();
                return Json(new { success = true });
            }
            else
            {
                return Json(new { success = false });
            }
        }

        public ActionResult AddDocGia()
        {
            return View();
        }

        // POST: Admin/DocGia/SaveAdd
        [HttpPost]
        public ActionResult SaveAdd(DocGia newDocGia)
        {
            if (ModelState.IsValid)
            {
                if (newDocGia.NgaySinh.HasValue)
                {
                    
                    int currentYear = DateTime.Now.Year;

                    // Lấy năm sinh từ ngày sinh
                    int birthYear = newDocGia.NgaySinh.Value.Year;

                    // Tính tuổi bằng cách lấy hiện tại trừ đi năm sinh
                    int age = currentYear - birthYear;

                    // Kiểm tra tuổi phải trên 12 tuổi so với hiện tại
                    if (age < 12)
                    {
                        ModelState.AddModelError("NgaySinh", "Tuổi của độc giả phải trên 12 tuổi. Vui lòng nhập lại ngày sinh.");
                        return View("AddDocGia", newDocGia);
                    }
                }
                else
                {
                    ModelState.AddModelError("NgaySinh", "Ngày sinh không hợp lệ. Vui lòng nhập lại ngày sinh.");
                    return View("AddDocGia", newDocGia);
                }
                newDocGia.MaDocGia = GenerateNextMaDocGia();

                database.DocGias.Add(newDocGia);
                database.SaveChanges();
                return RedirectToAction("QLDocGia");
            }
            return View("AddDocGia", newDocGia);
        }



        // Helper method to generate the next MaDocGia
        private string GenerateNextMaDocGia()
        {
            string defaultMaDocGia = "DG001";
            string nextMaDocGia = defaultMaDocGia;

            // Kiểm tra xem MaDocGia đã tồn tại trong cơ sở dữ liệu hay chưa
            while (database.DocGias.Any(d => d.MaDocGia == nextMaDocGia))
            {
                // Tách phần số hậu tố ra khỏi MaDocGia
                string numericPart = nextMaDocGia.Substring(2);

                // Tăng giá trị số lên 1 và cập nhật MaDocGia mới
                if (int.TryParse(numericPart, out int numericValue))
                {
                    numericValue++;
                    nextMaDocGia = string.Format("DG{0:D3}", numericValue);
                }
                else
                {
                    // Nếu không thể phân tích phần số hậu tố, trở lại MaDocGia mặc định "DG001"
                    nextMaDocGia = defaultMaDocGia;
                    break;
                }
            }

            return nextMaDocGia;
        }
    }
}
