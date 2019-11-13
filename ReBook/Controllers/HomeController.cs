using ReBook.App_Data;
using ReBook.Models;
using System;
using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;

namespace ReBook.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            using (var db = new DBConText())
            {
                var banners = db.Banner.ToList();
                ViewBag.Banners = banners;
                //Lay tat ca sach sap xep theo id giam dan (moi nhat)
                var listBook = db.Sach.Where(p => !p.isDeleted).OrderByDescending(p => p.id).Take(16).ToList();
                ViewBag.ListBook = listBook;
                if (listBook.Count() == 0)
                    ViewBag.Messenge = "Không tìm được sách theo yêu cầu!";
                return View();
            }
        }

        //Tim sach theo ten
        public ActionResult SearchBook(string searchString)
        {

            using (var db = new DBConText())
            {
                db.Sach.Load();

                //Tat ca sach trong csdl
                var books = db.Sach.Local.ToList();

                //Ten saches chua ky tu can tim kiem
                if (!String.IsNullOrEmpty(searchString))
                {
                    searchString = StringHelper.convertToUnSign(searchString);
                    books = db.Sach.Local.Where(s => StringHelper.convertToUnSign(s.TenSach).Contains(searchString) ||
                    StringHelper.convertToUnSign(s.TenTacGia).Contains(searchString) ||
                    StringHelper.convertToUnSign(s.ChuDe).Contains(searchString)).Take(12).ToList();
                }

                //Tra ra ViewBag
                var banners = db.Banner.ToList();
                ViewBag.Banners = banners;
                ViewBag.ListBook = books.ToList();
                if (books.Count() == 0)
                    ViewBag.Messenge = "Không tìm được sách theo yêu cầu!";
                return View();
            }
        }

        //Tim sach theo ten chu de
        public ActionResult CategoryBook(string categoryString)
        {
            using (var db = new DBConText())
            {
                var books = from l in db.Sach
                            select l;

                if (!String.IsNullOrEmpty(categoryString))
                {
                    books = books.Where(s => s.ChuDe.Contains(categoryString));
                }
                ViewBag.ListBook = books.ToList();
                ViewBag.Messenge = categoryString;
                if (books.Count() == 0)
                    ViewBag.Messenge = "Không tìm được sách theo yêu cầu!";
                return View();
            }
        }

        //Chi tiet sach theo ID
        public ActionResult Detail(int id)
        {
            //Kiem tra TempData truoc khi gan vao ViewBag
            if (TempData["messenge"] != null)
                ViewBag.Messenge = TempData["messenge"].ToString();
            try
            {
                //Tim sach theo id
                using (var db = new DBConText())
                {
                    var book = db.Sach.Where(p => p.id == id).FirstOrDefault();
                    if (book == null)
                    {
                        ViewBag.Messenge = "Không tìm được sách theo yêu cầu!";
                    }
                    else
                    {
                        ViewBag.Book = book;
                        var books = db.Sach.Where(p => p.ChuDe == book.ChuDe || p.TenTacGia == book.TenTacGia).Take(12).ToList();
                        ViewBag.ListBookLienQuan = books;
                    }
                    return View();
                }
            }
            catch
            {
                ViewBag.Messenge = "Không tìm được sách theo yêu cầu!";
                return View();
            }
        }

        //Them sach vao Gio hang
        [HttpPost]
        public ActionResult AddCart(int id)
        {
            try
            {
                //Goi helper cua gio hang
                var helper = new GioHangHelper();
                //Neu session trong (chua dang nhap)
                if (Session.Count == 0)
                {
                    TempData["messenge"] = "Vui lòng đăng nhập!";
                    return Redirect(Url.Content("~/Login"));
                }
                else
                {
                    //Lay du lieu tu session
                    var user = (LoginModel)HttpContext.Session["User"];
                    using (var db = new DBConText())
                    {
                        //Neu gio hang chua ton tai va nguoc lai
                        if (!helper.isGioHangTonTai(user.TaiKhoan))
                        {
                            helper.TaoGioHang(user.TaiKhoan);
                            helper.ThemSanPham(id, user.TaiKhoan);
                        }
                        else
                        {
                            helper.ThemSanPham(id, user.TaiKhoan);
                        }
                    }
                    TempData["messenge"] = "Thêm vào giỏ hàng thành công!";
                    return Redirect(Request.UrlReferrer.PathAndQuery);
                }
            }
            catch
            {
                ViewBag.Messenge = "Some thing wong";
                return Redirect(Request.UrlReferrer.PathAndQuery);
            }
        }

        public ActionResult CartDetail()
        {
            var helper = new GioHangHelper();
            try
            {
                if (Session.Count == 0)
                {
                    TempData["messenge"] = "Vui lòng đăng nhập!";
                    return Redirect(Url.Content("~/Login"));
                }
                else
                {
                    var user = (LoginModel)HttpContext.Session["User"];
                    ViewBag.ListBook = helper.ChiTietGioHang(user.TaiKhoan);
                    ViewBag.TongGia = helper.TongTienGioHang(user.TaiKhoan);
                    if (helper.ChiTietGioHang(user.TaiKhoan).Count == 0)
                        TempData["listemptyMessage"] = "No results";
                    return View();
                }
            }
            catch
            {
                throw;
            }
        }

        public ActionResult RemoveCart(int id)
        {
            try
            {
                var helper = new GioHangHelper();
                if (Session.Count == 0)
                {
                    TempData["messenge"] = "Vui lòng đăng nhập!";
                    return Redirect(Url.Content("~/Login"));
                }
                else
                {
                    var user = (LoginModel)HttpContext.Session["User"];
                    helper.RemoveSanPham(id, user.TaiKhoan);
                    ViewBag.Messenge = "Xoá sản phẩm thành công!";
                    return Redirect(Request.UrlReferrer.PathAndQuery);
                }
            }
            catch
            {
                ViewBag.Messenge = "Some thing wong";
                return Redirect(Request.UrlReferrer.PathAndQuery);
            }
        }

        public ActionResult InsertCart(int id)
        {
            try
            {
                var helper = new GioHangHelper();
                if (Session.Count == 0)
                {
                    TempData["messenge"] = "Vui lòng đăng nhập!";
                    return Redirect(Url.Content("~/Login"));
                }
                else
                {
                    var user = (LoginModel)HttpContext.Session["User"];
                    helper.TangSanPham(id, user.TaiKhoan);
                    ViewBag.Messenge = "Thêm sản phẩm thành công!";
                    return Redirect(Request.UrlReferrer.PathAndQuery);
                }
            }
            catch
            {
                ViewBag.Messenge = "Some thing wong";
                return Redirect(Request.UrlReferrer.PathAndQuery);
            }
        }

        public ActionResult ExtractCart(int id)
        {
            try
            {
                var helper = new GioHangHelper();
                if (Session.Count == 0)
                {
                    TempData["messenge"] = "Vui lòng đăng nhập!";
                    return Redirect(Url.Content("~/Login"));
                }
                else
                {
                    var user = (LoginModel)HttpContext.Session["User"];
                    helper.XoaSanPham(id, user.TaiKhoan);
                    ViewBag.Messenge = "Thêm sản phẩm thành công!";
                    return Redirect(Request.UrlReferrer.PathAndQuery);
                }
            }
            catch
            {
                ViewBag.Messenge = "Some thing wong";
                return Redirect(Request.UrlReferrer.PathAndQuery);
            }
        }

        [HttpGet]
        public ActionResult ThanhToan()
        {
            if (Session.Count == 0)
            {
                TempData["messenge"] = "Vui lòng đăng nhập!";
                return Redirect(Url.Content("~/Login"));
            }

            var helper = new GioHangHelper();
            var user = (LoginModel)HttpContext.Session["User"];
            ViewBag.TongGia = helper.TongTienGioHang(user.TaiKhoan);

            return View();
        }

        [HttpPost]
        public ActionResult ThanhToan([Bind(Exclude = "id")]HoaDon newHoaDon)
        {
            try
            {
                var helper = new HoaDonHelper();
                var ghhelper = new GioHangHelper();

                if (Session.Count == 0)
                {
                    TempData["messenge"] = "Vui lòng đăng nhập!";
                    return Redirect(Url.Content("~/Login"));
                }
                else
                {
                    var user = (LoginModel)HttpContext.Session["User"];
                    ViewBag.TongGia = ghhelper.TongTienGioHang(user.TaiKhoan);
                    if (newHoaDon.DiaChiGiaoHang.Length < 5 || newHoaDon.SDTGiaoHang.Length < 9)
                    {
                        ViewBag.Messenge = "Yêu cầu nhập địa chỉ và số điện thoại chính xác!";
                        return View();
                    }
                    helper.LapHoaDon(user.TaiKhoan, newHoaDon.DiaChiGiaoHang, newHoaDon.SDTGiaoHang, newHoaDon.NgayHenGiaoHang.ToString(), newHoaDon.GhiChu);
                    TempData["messenge"] = "Đơn hàng đã được tạo thành công";
                    return RedirectToAction("Switch");
                }
            }
            catch
            {
                var ghhelper = new GioHangHelper();
                var user = (LoginModel)HttpContext.Session["User"];
                ViewBag.TongGia = ghhelper.TongTienGioHang(user.TaiKhoan);
                return View();
            }
        }

        public ActionResult Switch()
        {
            if (TempData["messenge"] != null)
            {
                ViewBag.Messenge = TempData["messenge"].ToString();
            }
            return View();
        }
    }
}