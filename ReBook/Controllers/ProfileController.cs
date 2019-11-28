using ReBook.App_Data;
using ReBook.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;

namespace ReBook.Controllers
{
    public class ProfileController : Controller
    {
        // GET: Profile
        [HttpGet]
        public ActionResult Index()
        {
            try
            {
                //Neu session trong (chua dang nhap)
                if (Session["User"] == null)
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
                        var KH = db.KhachHang.First(x => x.TaiKhoan == user.TaiKhoan);
                        ViewBag.Message = TempData["Message"];
                        ViewBag.MessageType = TempData["MessageType"];
                        return View(KH);
                    }
                }
            }
            catch
            {
                ViewBag.Messenge = "Some thing wong";
                return RedirectToAction("Index", "Home");
            }
        }

        [HttpPost]
        public ActionResult Index(KhachHang KH, string nsNgay = "", string nsThang = "", string nsNam = "")
        {
            try
            {
                //Neu session trong (chua dang nhap)
                if (Session["User"] == null)
                {
                    TempData["messenge"] = "Vui lòng đăng nhập!";
                    return Redirect(Url.Content("~/Login"));
                }
                else
                {
                    KH.NgaySinh = string.Join("-", new string[] { nsNam, nsThang, nsNgay });

                    // Validate
                    if (string.IsNullOrWhiteSpace(KH.TenKH))
                    {
                        ViewBag.Message = "Tên khách hàng không hợp lệ";
                        return View(KH);
                    }
                    if (!int.TryParse(nsNgay, out int ngay) || ngay < 1 || ngay > 31)
                    {
                        ViewBag.Message = "Ngày sinh không hợp lệ";
                        return View(KH);
                    }
                    if (!int.TryParse(nsThang, out int thang) || thang < 1 || thang > 12)
                    {
                        ViewBag.Message = "Ngày sinh không hợp lệ";
                        return View(KH);
                    }
                    if (!int.TryParse(nsNam, out int nam) || nam.ToString().Length != 4)
                    {
                        ViewBag.Message = "Ngày sinh không hợp lệ";
                        return View(KH);
                    }
                    if (string.IsNullOrWhiteSpace(KH.DiaChi))
                    {
                        ViewBag.Message = "Địa chỉ không hợp lệ";
                        return View(KH);
                    }
                    try
                    {
                        System.Net.Mail.MailAddress m = new System.Net.Mail.MailAddress(KH.Email);
                    }
                    catch
                    {
                        ViewBag.Message = "Email không hợp lệ";
                        return View(KH);
                    }
                    if (string.IsNullOrWhiteSpace(KH.GioiTinh) || (KH.GioiTinh != "Nam" && KH.GioiTinh != "Nữ"))
                    {
                        ViewBag.Message = "Hãy chọn giá đúng";
                        return View(KH);
                    }

                    //Sửa thông tin KH
                    var user = (LoginModel)HttpContext.Session["User"];
                    using (var db = new DBConText())
                    {
                        var khachHang = db.KhachHang.First(x => x.TaiKhoan == user.TaiKhoan);
                        khachHang.TenKH = KH.TenKH;
                        khachHang.DiaChi = KH.DiaChi;
                        khachHang.NgaySinh = KH.NgaySinh;
                        khachHang.GioiTinh = KH.GioiTinh;
                        khachHang.Email = KH.Email;
                        db.SaveChanges();

                        ViewBag.Message = "Thay đổi thông tin thành công!";
                        ViewBag.MessageType = "Success";
                        return View(khachHang);
                    }
                }
            }
            catch
            {
                ViewBag.Messenge = "Some thing wong";
                return RedirectToAction("Index", "Home");
            }
        }

        // GET: Profile/Orders
        public ActionResult Orders()
        {
            try
            {
                //Neu session trong (chua dang nhap)
                if (Session["User"] == null)
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
                        var orders = db.HoaDon.Where(x => x.idKhachHang == user.TaiKhoan);

                        // Sắp xếp
                        var completedOrders = orders.Where(x => x.TinhTrang == "Hoàn thành" || x.TinhTrang == "Đã huỷ").OrderByDescending(x => x.NgayLapHD);
                        var deliveringOrders = orders.Where(x => x.TinhTrang == "Đang giao hàng").OrderByDescending(x => x.NgayLapHD);
                        var paidOrders = orders.Where(x => x.TinhTrang == "Đã thanh toán").OrderByDescending(x => x.NgayLapHD);
                        var validatedOrders = orders.Where(x => x.TinhTrang == "Chờ xác nhận").OrderByDescending(x => x.NgayLapHD);

                        List<HoaDon> lstHoaDon = new List<HoaDon>();
                        lstHoaDon.AddRange(validatedOrders);
                        lstHoaDon.AddRange(paidOrders);
                        lstHoaDon.AddRange(deliveringOrders);
                        lstHoaDon.AddRange(completedOrders);

                        ViewData["Orders"] = lstHoaDon;
                        return View();
                    }
                }
            }
            catch
            {
                ViewBag.Messenge = "Some thing wong";
                return RedirectToAction("Index", "Home");
            }
        }
        [HttpPost]
        public ActionResult Orders(string from = "", string to = "")
        {
            try
            {
                //Neu session trong (chua dang nhap)
                if (Session["User"] == null)
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
                        var orders = db.HoaDon.Where(x => x.idKhachHang == user.TaiKhoan).ToList();

                        if (!string.IsNullOrWhiteSpace(from))
                            orders = orders.FindAll(x => DateTime.Parse(x.NgayLapHD) >= DateTime.Parse(from));
                        if (!string.IsNullOrWhiteSpace(to))
                            orders = orders.FindAll(x => DateTime.Parse(x.NgayLapHD) <= DateTime.Parse(to));

                        // Sắp xếp
                        var completedOrders = orders.FindAll(x => x.TinhTrang == "Hoàn thành" || x.TinhTrang == "Đã huỷ").OrderByDescending(x => x.NgayLapHD);
                        var deliveringOrders = orders.FindAll(x => x.TinhTrang == "Đang giao hàng").OrderByDescending(x => x.NgayLapHD);
                        var paidOrders = orders.FindAll(x => x.TinhTrang == "Đã thanh toán").OrderByDescending(x => x.NgayLapHD);
                        var validatedOrders = orders.FindAll(x => x.TinhTrang == "Chờ xác nhận").OrderByDescending(x => x.NgayLapHD);

                        List<HoaDon> lstHoaDon = new List<HoaDon>();
                        lstHoaDon.AddRange(validatedOrders);
                        lstHoaDon.AddRange(paidOrders);
                        lstHoaDon.AddRange(deliveringOrders);
                        lstHoaDon.AddRange(completedOrders);

                        ViewData["Orders"] = lstHoaDon;
                        ViewBag.From = from;
                        ViewBag.To = to;
                        return View();
                    }
                }
            }
            catch
            {
                ViewBag.Messenge = "Some thing wong";
                return RedirectToAction("Index", "Home");
            }
        }

        [Route("Profile/Orders/Cancel/{id}")]
        public ActionResult Cancel(int id)
        {
            try
            {
                //Neu session trong (chua dang nhap)
                if (Session["User"] == null)
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
                        try
                        {
                            var order = db.HoaDon.Where(x => x.idKhachHang == user.TaiKhoan && x.id == id);
                            if (order.Count() > 0)
                            {
                                order.First().TinhTrang = "Đã huỷ";
                                db.SaveChanges();
                            }
                        }
                        catch { }
                    }

                    return RedirectToAction("Orders");
                }
            }
            catch
            {
                ViewBag.Messenge = "Some thing wong";
                return RedirectToAction("Index", "Home");
            }
        }

        public ActionResult ChangePassword()
        {
            try
            {
                //Neu session trong (chua dang nhap)
                if (Session["User"] == null)
                {
                    TempData["messenge"] = "Vui lòng đăng nhập!";
                    return Redirect(Url.Content("~/Login"));
                }
                else
                {
                        return View(new ChangePasswordModel());
                }
            }
            catch
            {
                ViewBag.Messenge = "Some thing wong";
                return RedirectToAction("Index", "Home");
            }
        }

        [HttpPost]
        public ActionResult ChangePassword(ChangePasswordModel a)
        {
            try
            {
                //Neu session trong (chua dang nhap)
                if (Session["User"] == null)
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
                        if (a.MatKhau == null || a.MatKhauMoi == null || a.MatKhauMoiNhapLai == null)
                        {
                            ViewBag.Messenge = "Vui lòng không bỏ trống dòng!";
                            return View(a);
                        }
                        if (user.MatKhau != a.MatKhau)
                        {
                            ViewBag.Messenge = "Mật khẩu cũ không đúng!";
                            return View(a);
                        }
                        if (a.MatKhauMoi != a.MatKhauMoiNhapLai)
                        {
                            ViewBag.Messenge = "Mật khẩu nhập lại không đúng!";
                            return View(a);
                        }
                        if(!IsPassword(a.MatKhauMoi))
                        {
                            ViewBag.Messenge = "Password phải có ít nhất 1 ký tự, 1 số";
                            return View(a);
                        }
                        if (user.MatKhau == a.MatKhauMoi)
                        {
                            ViewBag.Messenge = "Mật khẩu mới giống mật khẩu cũ y như đúc ???!";
                            return View(a);
                        }
                        else
                        {
                            var olduser = db.KhachHang.FirstOrDefault(p => p.TaiKhoan == user.TaiKhoan);
                            olduser.MatKhau = a.MatKhauMoi;
                            db.SaveChanges();
                            TempData["Message"] = "Chúc mừng bạn đã đổi mật khẩu thành công, mật khẩu mới là: ********";
                            TempData["MessageType"] = "Success";
                            return RedirectToAction("Index");
                        }

                    }
                }
            }
            catch
            {
                ViewBag.Messenge = "Some thing wong";
                return RedirectToAction("Index", "Home");
            }
        }

        public bool IsPassword(string psw)
        {
            var hasWord = new Regex(@"[a-zA-Z]+");
            var hasDigit = new Regex(@"[0-9]+");
            var hasSpecialChar = new Regex("[;\"]+");

            if (hasWord.IsMatch(psw) && hasDigit.IsMatch(psw) && !hasSpecialChar.IsMatch(psw))
                return true;
            else
            {
                return false;
            }
        }
    }
}