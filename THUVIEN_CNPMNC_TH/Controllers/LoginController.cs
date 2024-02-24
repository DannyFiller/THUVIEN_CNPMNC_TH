using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Security.Principal;
using System.Web;
using System.Web.Mvc;
using THUVIEN_CNPMNC_TH.Models;



namespace THUVIEN_CNPMNC_TH.Controllers
{
    public class LoginController : Controller
    {
        LIBRARY_FINALEntities1 db = new LIBRARY_FINALEntities1();

        public ActionResult LoginAndRegistration()
        {
            if (TempData["SuccessMessage"] != null)
            {
                ViewBag.SuccessMessage = TempData["SuccessMessage"];
            }
            if (TempData["ErrorMessage"] != null)
            {
                ViewBag.ErrorMessage = TempData["ErrorMessage"];
            }
            return View();
        }

        [HttpPost]
        public ActionResult KiemTraDangNhap(string username, string password)
        {
            // Kiểm tra đăng nhập bằng tài khoản User
            var user = db.Users.FirstOrDefault(u => u.UserName == username && u.Passwords == password);

            // Kiểm tra đăng nhập bằng tài khoản DocGia
            var docGia = db.DocGias.FirstOrDefault(d => d.Email == username && d.Passwords == password);

            if (user != null)
            {
                if (user.Roles == 3)
                {

                    Session["UserRoles"] = user.Roles;
                    return RedirectToAction("Index", "HomeClient");
                }
                else if (user.Roles == 1 || user.Roles == 2)
                {
                    Session["UserRoles"] = user.Roles;
                    TempData["SuccessMessage"] = "Đăng nhập thành công.";
                    return RedirectToAction("Index", "Home", new { area = "Admin" });
                }
            }
            else if (docGia != null)
            {
                if (docGia.Roles == 3)
                {

                    Session["DocGiaRoles"] = docGia.Roles;
                    Session["DocGiaID"] = docGia.MaDocGia;
                    TempData["SuccessMessage"] = "Đăng nhập thành công.";
                    return RedirectToAction("Index", "HomeClient");
                }
            }
            TempData["ErrorMessage"] = "Đăng nhập không thành công. Vui lòng kiểm tra lại thông tin đăng nhập.";
            return RedirectToAction("LoginAndRegistration");
        }

        public ActionResult Logout()
        {

            Session["DocGiaRoles"] = null;
            Session["UserRoles"] = null;
            TempData["SuccessMessage"] = "Đăng xuất  thành công.";
            return RedirectToAction("Index", "HomeClient"); 
        }


        [HttpPost]
        public ActionResult SaveRegistration(DocGia newDocGia)
        {
            if (ModelState.IsValid)
            {
                //string hashedPassword = BCrypt.Net.BCrypt.HashPassword(newDocGia.Passwords);

                //// Ensure the hashed password is not longer than 50 characters
                //if (hashedPassword.Length > 50)
                //{
                //    // Truncate the hashed password to 50 characters
                //    hashedPassword = hashedPassword.Substring(0, 50);
                //}

                //newDocGia.Passwords = hashedPassword;
                newDocGia.Roles = 3;
                newDocGia.MaDocGia = GenerateNextMaDocGia();
                db.DocGias.Add(newDocGia);
                db.SaveChanges();

                // Redirect to a success page or login page
                return RedirectToAction("LoginAndRegistration");
            }
            TempData["SuccessMessage"] = "Đăng ký tài khoản thành công.";
            return View("LoginAndRegistration", newDocGia);
        }


        private string GenerateNextMaDocGia()
        {
            string defaultMaDocGia = "DG001";
            string nextMaDocGia = defaultMaDocGia;

            // Kiểm tra xem MaDocGia đã tồn tại trong cơ sở dữ liệu hay chưa
            while (db.DocGias.Any(d => d.MaDocGia == nextMaDocGia))
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