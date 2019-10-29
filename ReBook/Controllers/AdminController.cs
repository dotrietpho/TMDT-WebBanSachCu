using ReBook.App_Data;
using ReBook.Models;
using System.Linq;
using System.Web.Mvc;

namespace ReBook.Controllers
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