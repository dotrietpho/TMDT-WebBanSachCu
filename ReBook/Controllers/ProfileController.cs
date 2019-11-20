using ReBook.App_Data;
using ReBook.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ReBook.Controllers
{
    public class ProfileController : Controller
    {
        // GET: Profile
        public ActionResult Index()
        {
            try
            {
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
                        var KH = db.KhachHang.First(x => x.TaiKhoan == user.TaiKhoan);

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
                if (Session.Count == 0)
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
    }
}