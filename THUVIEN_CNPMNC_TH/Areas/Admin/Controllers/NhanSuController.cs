using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using THUVIEN_CNPMNC_TH.Models;

namespace THUVIEN_CNPMNC_TH.Areas.Admin.Controllers
{
    public class NhanSuController : Controller
    {
        // GET: Admin/NhanSu
        LIBRARY_FINALEntities1 database = new LIBRARY_FINALEntities1();

        public ActionResult QLNhanSu()
        {
            // Lấy danh sách thủ thư từ cơ sở dữ liệu và truyền vào view
            var thuThuList = database.ThuThus.ToList();
            ViewBag.ThuThuList = thuThuList;

            // Lấy danh sách nhân viên kho từ cơ sở dữ liệu và truyền vào view
            var nhanVienKhoList = database.NhanVienKhoes.ToList();
            ViewBag.NhanVienKhoList = nhanVienKhoList;

            // Lấy danh sách nhân viên quản lý độc giả từ cơ sở dữ liệu và truyền vào view
            var nhanVienQLDGList = database.NhanVienQuanLyDocGias.ToList();
            ViewBag.NhanVienQLDGList = nhanVienQLDGList;

            return View();
        }
    }
}