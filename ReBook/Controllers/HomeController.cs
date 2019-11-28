using ReBook.App_Data;
using ReBook.Models;
using ReBook.Models.Helper;
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
                if (Session["User"] == null)
                {
                    if (Session["Guest"] == null)
                    {
                        string guestIP = Request.UserHostAddress;

                        Session["Guest"] = guestIP;
                        helper.ThemSanPham(id, guestIP, true);
                    }
                    else
                    {
                        string guestIP = Session["Guest"].ToString();

                        Session["Guest"] = guestIP;
                        helper.ThemSanPham(id, guestIP);
                    }
                    TempData["messenge"] = "Thêm vào giỏ hàng thành công!";
                    return Redirect(Request.UrlReferrer.PathAndQuery);
                }
                else
                {
                    //Lay du lieu tu session
                    var sessionUser = (LoginModel)HttpContext.Session["User"];
                    using (var db = new DBConText())
                    {
                        var user = db.KhachHang.Find(sessionUser.TaiKhoan);
                        helper.ThemSanPham(id, user.GetIdGioHang());
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
                if (Session["User"] == null)
                {
                    if (Session["Guest"] == null)
                    {
                        helper.ClearGioHang(Request.UserHostAddress);
                    }

                    var guestIP = Request.UserHostAddress;
                    ViewBag.ListBook = helper.ChiTietGioHang(guestIP);
                    ViewBag.TongGia = helper.TongTienGioHang(guestIP);
                    if (helper.ChiTietGioHang(guestIP).Count == 0)
                        TempData["listemptyMessage"] = "No results";

                    return View();
                }
                else
                {
                    var sessionUser = (LoginModel)HttpContext.Session["User"];

                    using (var db = new DBConText())
                    {
                        var user = db.KhachHang.Find(sessionUser.TaiKhoan);
                        ViewBag.ListBook = helper.ChiTietGioHang(user.GetIdGioHang());
                        ViewBag.TongGia = helper.TongTienGioHang(user.GetIdGioHang());
                        if (helper.ChiTietGioHang(user.GetIdGioHang()).Count == 0)
                            TempData["listemptyMessage"] = "No results";
                    }
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
                if (Session["User"] == null)
                {
                    string guestIP = Request.UserHostAddress;

                    Session["Guest"] = guestIP;
                    helper.XoaSanPham(id, guestIP);

                    ViewBag.Messenge = "Xoá sản phẩm thành công!";
                    return Redirect(Request.UrlReferrer.PathAndQuery);
                }
                else
                {
                    var sessionUser = (LoginModel)HttpContext.Session["User"];
                    using (var db = new DBConText())
                    {
                        var user = db.KhachHang.Find(sessionUser.TaiKhoan);
                        helper.XoaSanPham(id, user.GetIdGioHang());
                    }
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
                if (Session["User"] == null)
                {
                    string guestIP = Request.UserHostAddress;

                    Session["Guest"] = guestIP;
                    helper.TangSanPham(id, guestIP);

                    ViewBag.Messenge = "Tăng số lượng sản phẩm thành công!";
                    return Redirect(Request.UrlReferrer.PathAndQuery);
                }
                else
                {
                    var sessionUser = (LoginModel)HttpContext.Session["User"];
                    using (var db = new DBConText())
                    {
                        var user = db.KhachHang.Find(sessionUser.TaiKhoan);
                        helper.TangSanPham(id, user.GetIdGioHang());
                    }
                    ViewBag.Messenge = "Tăng số lượng sản phẩm thành công!";
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
                if (Session["User"] == null)
                {
                    string guestIP = Request.UserHostAddress;

                    Session["Guest"] = guestIP;
                    helper.GiamSanPham(id, guestIP);

                    ViewBag.Messenge = "Giảm sản phẩm thành công!";
                    return Redirect(Request.UrlReferrer.PathAndQuery);
                }
                else
                {
                    var sessionUser = (LoginModel)HttpContext.Session["User"];
                    using (var db = new DBConText())
                    {
                        var user = db.KhachHang.Find(sessionUser.TaiKhoan);
                        helper.GiamSanPham(id, user.GetIdGioHang());
                    }
                    ViewBag.Messenge = "Giảm sản phẩm thành công!";
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
            if (Session["User"] == null)
            {
                TempData["messenge"] = "Vui lòng đăng nhập!";
                return Redirect(Url.Content("~/Login"));
            }

            var helper = new GioHangHelper();
            var sessionUser = (LoginModel)HttpContext.Session["User"];
            using (var db = new DBConText())
            {
                var user = db.KhachHang.Find(sessionUser.TaiKhoan);
                ViewBag.TongGia = helper.TongTienGioHang(user.GetIdGioHang());
            }

            return View();
        }

        [HttpPost]
        public ActionResult ThanhToan([Bind(Exclude = "id")]HoaDon newHoaDon)
        {
            try
            {
                if (newHoaDon.NgayHenGiaoHang == null)
                    newHoaDon.NgayHenGiaoHang = "";
                if (newHoaDon.GhiChu == null)
                    newHoaDon.GhiChu = "";
                var helper = new HoaDonHelper();
                var ghhelper = new GioHangHelper();

                if (Session["User"] == null)
                {
                    TempData["messenge"] = "Vui lòng đăng nhập!";
                    return Redirect(Url.Content("~/Login"));
                }
                else
                {
                    var sessionUser = (LoginModel)HttpContext.Session["User"];
                    using (var db = new DBConText())
                    {
                        var user = db.KhachHang.Find(sessionUser.TaiKhoan);
                        ViewBag.TongGia = ghhelper.TongTienGioHang(user.GetIdGioHang());

                        if (!StringHelper.NumberValid(newHoaDon.SDTGiaoHang) || newHoaDon.SDTGiaoHang.Length < 9)
                        {
                            ViewBag.Messenge = "Số điện thoại không hợp lệ!";
                            return View();
                        }
                        ThongTinGiaoHangModel info = new ThongTinGiaoHangModel()
                        {
                            DiaChi = newHoaDon.DiaChiGiaoHang,
                            SDT = newHoaDon.SDTGiaoHang,
                            GhiChu = newHoaDon.GhiChu,
                            NgayGiaoHang = newHoaDon.NgayHenGiaoHang.ToString()
                        };
                        Session["Order"] = info;
                    }
                    return RedirectToAction("XacNhanThanhToan");
                }
            }
            catch
            {
                var ghhelper = new GioHangHelper();
                var sessionUser = (LoginModel)HttpContext.Session["User"];
                using (var db = new DBConText())
                {
                    var user = db.KhachHang.Find(sessionUser.TaiKhoan);
                    ViewBag.TongGia = ghhelper.TongTienGioHang(user.GetIdGioHang());
                }
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

        [HttpGet]
        public ActionResult XacNhanThanhToan()
        {
            var ghhelper = new GioHangHelper();
            try
            {
                if (Session["User"] == null)
                {
                    TempData["messenge"] = "Vui lòng đăng nhập!";
                    return Redirect(Url.Content("~/Login"));
                }
                else
                {
                    var sessionUser = (LoginModel)HttpContext.Session["User"];
                    using (var db = new DBConText())
                    {
                        var user = db.KhachHang.Find(sessionUser.TaiKhoan);
                        ViewBag.TongGia = ghhelper.TongTienGioHang(user.GetIdGioHang());
                        var info = Session["Order"] as ThongTinGiaoHangModel;

                        if (info == null)
                            return RedirectToAction("Index");

                        ViewBag.ListBook = ghhelper.ChiTietGioHang(user.GetIdGioHang());
                        ViewBag.TongGia = ghhelper.TongTienGioHang(user.GetIdGioHang());
                        ViewBag.Info = info;
                        ViewBag.TenKH = user.TenKH;

                        if (ghhelper.ChiTietGioHang(user.GetIdGioHang()).Count == 0)
                            TempData["listemptyMessage"] = "No results";
                    }
                    return View();
                }
            }
            catch
            {
                throw;
            }
        }

        [HttpPost]
        public ActionResult XacNhanThanhToan([Bind(Exclude = "id")]HoaDon newHoaDon)
        {
            try
            {
                var helper = new HoaDonHelper();
                var ghhelper = new GioHangHelper();

                if (Session["User"] == null)
                {
                    TempData["messenge"] = "Vui lòng đăng nhập!";
                    return Redirect(Url.Content("~/Login"));
                }
                else
                {
                    var sessionUser = (LoginModel)HttpContext.Session["User"];
                    using (var db = new DBConText())
                    {
                        var user = db.KhachHang.Find(sessionUser.TaiKhoan);
                        ViewBag.TongGia = ghhelper.TongTienGioHang(user.GetIdGioHang());
                        if (newHoaDon.NgayHenGiaoHang == null)
                            newHoaDon.NgayHenGiaoHang = "";
                        if (newHoaDon.GhiChu == null)
                            newHoaDon.GhiChu = "";
                        helper.LapHoaDon(user.TaiKhoan, user.GetIdGioHang(), newHoaDon.DiaChiGiaoHang, newHoaDon.SDTGiaoHang, newHoaDon.NgayHenGiaoHang.ToString(), newHoaDon.GhiChu, newHoaDon.isPaid);

                        TempData["messenge"] = "Đơn hàng đã được tạo thành công";
                    }
                    return RedirectToAction("Switch");
                }
            }
            catch
            {
                throw;
            }

        }
    }
}