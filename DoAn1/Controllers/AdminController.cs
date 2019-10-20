using DoAn1.App_Data;
using DoAn1.Models;
using System.Linq;
using System.Web.Mvc;

namespace DoAn1.Controllers
{
    public class AdminController : Controller
    {
        // GET: Admin
        public ActionResult Index()
        {
            Session.RemoveAll();
            return View();
        }

        [HttpPost]
        public ActionResult Login(AdminModel a)
        {
            using (var db = new DbContext())
            {
                var user = db.TaiKhoanAdmin.Where(p => p.TaiKhoan == a.TaiKhoan).FirstOrDefault();
                if (user != null && user.MatKhau == a.MatKhau)
                {
                    Session["Admin"] = a;
                    return Redirect(Url.Content("~/Book"));
                }
                else
                {
                    TempData["messenge"] = "";
                    return RedirectToAction("Index");
                }
            }
        }
    }
}