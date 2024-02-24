using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using THUVIEN_CNPMNC_TH.Models;

namespace THUVIEN_CNPMNC_TH.Controllers
{
    public class RegisterController : Controller
    {
        private LIBRARY_FINALEntities1 db = new LIBRARY_FINALEntities1();

        public ActionResult DangKy()
        {
            return View();
        }

        [HttpPost]
        public ActionResult DangKy(DocGia docGia)
        {
            if (ModelState.IsValid)
            {
                // Kiểm tra xem người dùng đã tồn tại hay chưa (bằng email hoặc tên đăng nhập, tùy vào cơ sở dữ liệu của bạn)
                var existingUser = db.DocGias.FirstOrDefault(u => u.Email == docGia.Email);

                if (existingUser == null)
                {
                    // Gán Roles mặc định (3) cho người dùng mới
                    docGia.Roles = 3;

                    // Thêm người dùng mới vào cơ sở dữ liệu
                    db.DocGias.Add(docGia);
                    db.SaveChanges();

                    // Chuyển hướng đến trang chính sau khi đăng ký thành công
                    return RedirectToAction("Index", "HomeClient");
                }
                else
                {
                    // Người dùng đã tồn tại, hiển thị thông báo lỗi
                    ModelState.AddModelError("", "Email đã tồn tại, vui lòng chọn email khác.");
                }
            }

            // Nếu có lỗi xảy ra hoặc dữ liệu không hợp lệ, hiển thị lại trang đăng ký
            return View(docGia);
        }

    }
}