using System;
using System.Collections.Generic;
using THUVIEN_CNPMNC_TH.Models;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;


namespace THUVIEN_CNPMNC_TH.Areas.Admin.Controllers
{
    public class HomeController : Controller
    {
        // GET: Admin/Home
        LIBRARY_FINALEntities1 database = new LIBRARY_FINALEntities1();
        public ActionResult Index()
        {
            if (TempData["SuccessMessage"] != null)
            {
                ViewBag.SuccessMessage = TempData["SuccessMessage"];
            }
            if (TempData["ErrorMessage"] != null)
            {
                ViewBag.ErrorMessage = TempData["ErrorMessage"];
            }
            var books = database.Books.ToList();
            var docgia = database.DocGias.ToList();
            var thedocgia = database.TheDocGias.ToList();
            int sosachmuon = database.PhieuMuonTras.Count();

            // Tạo ViewModel chứa danh sách sách và danh sách độc giả
            var viewModel = new ViewModel
            {
                Books = books,
                DocGias = docgia,
                TheDocGias = thedocgia,
                SoSachMuons = sosachmuon
            };
            return View(viewModel);
        }
    }
}