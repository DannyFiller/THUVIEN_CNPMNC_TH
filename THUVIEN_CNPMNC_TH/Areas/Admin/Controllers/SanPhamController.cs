using System;
using System.Collections.Generic;
using THUVIEN_CNPMNC_TH.Models;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;

namespace THUVIEN_CNPMNC_TH.Areas.Admin.Controllers
{
    public class SanPhamController : Controller
    {
        LIBRARY_FINALEntities1 database = new LIBRARY_FINALEntities1();

        private string GenerateNextMaSach()
        {
            string defaultMaSach = "S001";
            string nextMaSach = defaultMaSach;

            while (database.Books.Any(b => b.MaSach == nextMaSach))
            {
                string numericPart = nextMaSach.Substring(1);

                if (int.TryParse(numericPart, out int numericValue))
                {
                    numericValue++;
                    nextMaSach = $"S{numericValue:D3}";
                }
                else
                {
                    nextMaSach = defaultMaSach;
                    break;
                }
            }

            return nextMaSach;
        }

        public ActionResult QLSach()
        {
            return View();
        }

        public ActionResult QLSTTL(string maTheLoai)
        {
            var theLoais = database.TheLoais.ToList();
            if (string.IsNullOrEmpty(maTheLoai))
            {
                maTheLoai = theLoais.FirstOrDefault()?.MaTheLoai;
            }
            var books = database.Books.Where(b => b.MaTheLoai == maTheLoai).ToList();
            ViewBag.Books = books;
            ViewBag.SelectedTheLoai = maTheLoai;

            return View(theLoais);
        }

        public ActionResult AddBookView()
        {
            var tenSachList = database.PhieuNhaps.Select(pn => pn.TenSach).Distinct().ToList();
            ViewBag.TenSachList = new SelectList(tenSachList);

            ViewBag.TheLoaiList = new SelectList(database.TheLoais, "MaTheLoai", "TenTheLoai");
            ViewBag.NhaXuatBanList = new SelectList(database.NhaXuatBans, "MaNXB", "TenNXB");

            var nextMaSach = GenerateNextMaSach();
            var book = new Book { MaSach = nextMaSach };

            return View(book);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        public ActionResult AddBookView(Book book, HttpPostedFileBase imageFile, string selectedTenSach, string maTheLoai, string maNXB, bool? chooseTheLoai, bool? chooseMaNXB)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    book.TrungBay = false;
                    book.LuotMuonTra = 0;

                    if (!chooseTheLoai.HasValue)
                    {
                        book.MaTheLoai = maTheLoai;
                    }
                    if (!chooseMaNXB.HasValue)
                    {
                        book.MaNXB = maNXB;
                    }

                    book.MaSach = GenerateNextMaSach();
                    book.TenSach = selectedTenSach;

                    // Query the quantity of selectedTenSach from PhieuNhap
                    int quantityInPhieuNhap = database.PhieuNhaps
                        .Where(pn => pn.TenSach == selectedTenSach)
                        .Sum(pn => pn.SoLuong) ?? 0;

                    if (book.SoLuong <= quantityInPhieuNhap)
                    {
                        if (imageFile != null && imageFile.ContentLength > 0)
                        {
                            string fileName = Path.GetFileName(imageFile.FileName);
                            string filePath = Path.Combine(Server.MapPath("~/Content/Images/"), fileName);
                            imageFile.SaveAs(filePath);
                            book.HinhAnh = "~/Content/Images/" + fileName;
                        }
                        database.Books.Add(book);
                        database.SaveChanges();

                        return RedirectToAction("QLSach");
                    }
                    else
                    {
                        ModelState.AddModelError("SoLuong", "Số lượng sách không được vượt quá " + quantityInPhieuNhap);
                    }
                }
                catch (Exception ex)
                {
                    return RedirectToAction("Error");
                }
            }
            var tenSachList = database.PhieuNhaps.Select(pn => pn.TenSach).Distinct().ToList();
            ViewBag.TenSachList = new SelectList(tenSachList);
            ViewBag.TheLoaiList = new SelectList(database.TheLoais, "MaTheLoai", "TenTheLoai");
            ViewBag.NhaXuatBanList = new SelectList(database.NhaXuatBans, "MaNXB", "TenNXB");

            return View(book);
        }



        // Rest of your controller actions...

        public ActionResult DeleteBookView()
        {
            return View(database.Books.ToList());
        }

        [HttpPost]
        public ActionResult DeleteBook(string MaSach)
        {
            var book = database.Books.FirstOrDefault(b => b.MaSach == MaSach);
            if (book != null)
            {
                database.Books.Remove(book);
                database.SaveChanges();
                return Json(new { success = true });
            }
            else
            {
                return Json(new { success = false });
            }
        }


        public ActionResult ArrayBookView()
        {
            var books = database.Books.ToList();
            foreach (var book in books)
            {
                if (book.SoLuong <= 3)
                {
                    book.TrungBay = false;
                }
            }
            database.SaveChanges();

            return View(books);
        }

        public ActionResult DetailBookView(string MaSach)
        {
            var book = database.Books.FirstOrDefault(s => s.MaSach == MaSach);
            return View(book);
        }

        [HttpGet]
        public ActionResult EditBookView(string MaSach)
        {
            var book = database.Books.FirstOrDefault(b => b.MaSach == MaSach);
            if (book != null)
            {
                ViewBag.TheLoaiList = new SelectList(database.TheLoais, "MaTheLoai", "TenTheLoai", book.MaTheLoai);
                ViewBag.NhaXuatBanList = new SelectList(database.NhaXuatBans, "MaNXB", "TenNXB", book.MaNXB);
                return View(book);
            }
            else
            {
                return RedirectToAction("Error");
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditBook(Book book, HttpPostedFileBase imageFile)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    Book existingBook = database.Books.FirstOrDefault(b => b.MaSach == book.MaSach);
                    if (existingBook != null)
                    {
                        existingBook.TenSach = book.TenSach;
                        existingBook.MaTheLoai = book.MaTheLoai;
                        existingBook.MaNXB = book.MaNXB;
                        existingBook.TacGia = book.TacGia;
                        existingBook.SoLuong = book.SoLuong;
                        existingBook.NamXuatBan = book.NamXuatBan;
                        existingBook.MoTa = book.MoTa;

                        if (imageFile != null && imageFile.ContentLength > 0)
                        {
                            string fileName = Path.GetFileName(imageFile.FileName);
                            string filePath = Path.Combine(Server.MapPath("~/Content/Images/"), fileName);
                            imageFile.SaveAs(filePath);
                            existingBook.HinhAnh = "~/Content/Images/" + fileName;
                        }

                        database.SaveChanges();
                    }

                    return RedirectToAction("QLSach");
                }
                catch (Exception ex)
                {
                    return RedirectToAction("Error");
                }
            }

            ViewBag.TheLoaiList = new SelectList(database.TheLoais, "MaTheLoai", "TenTheLoai", book.MaTheLoai);
            ViewBag.NhaXuatBanList = new SelectList(database.NhaXuatBans, "MaNXB", "TenNXB", book.MaNXB);

            return View(book);
        }

        [HttpPost]
        public ActionResult UpdateTrungBay(string maSach, bool isChecked)
        {
            try
            {
                var book = database.Books.FirstOrDefault(b => b.MaSach == maSach);
                if (book != null)
                {
                    book.TrungBay = isChecked;
                    database.SaveChanges();
                    return Json(new { success = true });
                }
                else
                {
                    return Json(new { success = false });
                }
            }
            catch (Exception ex)
            {
                // Handle any exceptions here
                return Json(new { success = false });
            }
        }


        //////////////////////////


        public ActionResult QLTheLoaiView()
        {
            return View(database.TheLoais.ToList());
        }

        private string GenerateNextMaTheLoai()
        {
            string defaultMaTheLoai = "TL001";
            string nextMaTheLoai = defaultMaTheLoai;

            while (database.TheLoais.Any(tl => tl.MaTheLoai == nextMaTheLoai))
            {
                string numericPart = nextMaTheLoai.Substring(2);

                if (int.TryParse(numericPart, out int numericValue))
                {
                    numericValue++;
                    nextMaTheLoai = $"TL{numericValue:D3}";
                }
                else
                {
                    nextMaTheLoai = defaultMaTheLoai;
                    break;
                }
            }

            return nextMaTheLoai;
        }

        public ActionResult AddTheLoai()
        {
            var nextMaTheLoai = GenerateNextMaTheLoai();
            var theLoai = new TheLoai { MaTheLoai = nextMaTheLoai };

            return View(theLoai);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddTheLoai(TheLoai theLoai)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    theLoai.MaTheLoai = GenerateNextMaTheLoai();

                    database.TheLoais.Add(theLoai);
                    database.SaveChanges();

                    return RedirectToAction("QLTheLoaiView");
                }
                catch (Exception ex)
                {
                    return RedirectToAction("Error");
                }
            }

            return View(theLoai);
        }

        ////////////////

        public ActionResult QLNXB()
        {
            return View(database.NhaXuatBans.ToList());
        }

        private string GenerateNextNXB()
        {
            string defaultMaTheLoai = "NXB001";
            string nextMaTheLoai = defaultMaTheLoai;

            while (database.TheLoais.Any(tl => tl.MaTheLoai == nextMaTheLoai))
            {
                string numericPart = nextMaTheLoai.Substring(2);

                if (int.TryParse(numericPart, out int numericValue))
                {
                    numericValue++;
                    nextMaTheLoai = $"TL{numericValue:D3}";
                }
                else
                {
                    nextMaTheLoai = defaultMaTheLoai;
                    break;
                }
            }

            return nextMaTheLoai;
        }

        public ActionResult AddNXB()
        {
            var nextNXB = GenerateNextNXB();
            var nxb = new NhaXuatBan { MaNXB= nextNXB };

            return View(nxb);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddNXB(NhaXuatBan nxb)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    nxb.MaNXB = GenerateNextNXB();

                    database.NhaXuatBans.Add(nxb);
                    database.SaveChanges();

                    return RedirectToAction("QLNXB");
                }
                catch (Exception ex)
                {
                    return RedirectToAction("Error");
                }
            }

            return View(nxb);
        }
    }
}
