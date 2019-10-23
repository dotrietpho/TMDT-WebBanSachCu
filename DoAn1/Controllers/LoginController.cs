using DoAn1.App_Data;
using DoAn1.Models;
using System.Linq;
using System.Web.Mvc;

namespace DoAn1.Controllers
{
    public class LoginController : Controller
    {
        // GET: Login
        public ActionResult Index()
        {
            Session.RemoveAll();
            if (TempData["messenge"] != null)
            {
                ViewBag.Messenge = TempData["messenge"].ToString();
            }
            if (Session.Count > 0)
                return Redirect(Url.Content("~/"));
            return View();
        }

        //Login theo LoginModel (Phai co LoginModel thi moi gan cho Session duoc)
        [HttpPost]
        public ActionResult LoginCheck(LoginModel a)
        {
            using (var db = new DbContext())
            {
                var user = db.KhachHang.Where(p => p.TaiKhoan == a.TaiKhoan).FirstOrDefault();
                if (user != null && user.MatKhau == a.MatKhau)
                {
                    a.TenKH = user.TenKH;
                    Session["User"] = a;
                    return Redirect(Url.Content("~/"));
                }
                else
                {
                    TempData["messenge"] = "Sai tên đăng nhập hoặc mật khẩu!";
                    return RedirectToAction("Index");
                }
            }
        }

        public ActionResult Register()
        {
            //Neu Session da ton tai (da dang nhap) -> tra ve trang chu
            if (Session.Count > 0)
                return Redirect(Url.Content("~/"));
            return View();
        }

        // Su dung RegisterModel de luu du lieu Model tra ve tu view, xu ly tren model register
        [HttpPost]
        public ActionResult Register(RegisterModel a)
        {
            try
            {
                using (var db = new DbContext())
                {
                    var user = db.KhachHang.Where(p => p.TaiKhoan == a.TaiKhoan).FirstOrDefault();
                    //Neu da co user su dung tai khoan nay
                    if (user != null)
                    {
                        ViewBag.Messenge = "Tài khoản đã tồn tại!";
                        return View();
                    }
                    if (a.password.Length == 0 || a.ReTypedpassword.Length == 0 || a.TaiKhoan.Length == 0 || a.SDT.Length == 0 || a.TenKH.Length == 0)
                    {
                        ViewBag.Messenge = "Yêu cầu nhập đẩy đủ thông tin!";
                        return View();
                    }
                    if (a.password.Length <6 || a.TaiKhoan.Length<6)
                    { }
                    //Neu password nhap k trung khop
                    if (a.password == a.ReTypedpassword)
                    {
                        db.KhachHang.Add(new KhachHang(a.TaiKhoan, a.password, a.TenKH, a.SDT, a.NgaySinh));
                        db.SaveChanges();
                        Session["User"] = new LoginModel(a.TaiKhoan, a.password, a.TenKH);
                        return RedirectToAction("Redirect");
                    }
                    else
                    {
                        ViewBag.Messenge = "Mật khẩu nhập lại không đúng!";
                        return View();
                    }
                }
            }
            catch
            {
                ViewBag.Messenge = "Some thing wong";
                return RedirectToAction("Register");
            }
        }

        public ActionResult Redirect()
        {
            return View();
        }

        [HttpGet]
        public ActionResult Logout(LoginModel a)
        {
            Session.RemoveAll();
            Session.Clear();
            return Redirect(Url.Content("~/"));
        }

        static bool IsLetter(char c)
        {
            return (c >= 'a' && c <= 'z') || (c >= 'A' && c <= 'Z');
        }

        static bool IsDigit(char c)
        {
            return c >= '0' && c <= '9';
        }

        static bool IsSymbol(char c)
        {
            return c > 32 && c < 127 && !IsDigit(c) && !IsLetter(c);
        }

        static bool IsValidPassword(string password)
        {
            return
               password.Any(c => IsLetter(c)) &&
               password.Any(c => IsDigit(c)) &&
               password.Any(c => IsSymbol(c));
        }
    }
}