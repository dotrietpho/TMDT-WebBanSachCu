using DoAn1.App_Data;
using PagedList;
using System.Linq;
using System.Web.Mvc;

namespace DoAn1.Controllers
{
    public class UserController : Controller
    {
        // GET: User
        public ActionResult Index(int? page, string searchString)
        {
            ViewBag.Active = "User";
            ViewBag.Search = searchString;
            if (page == null) page = 1;
            if (searchString == null)
                searchString = "";
            using (var db = new DbContext())
            {
                //Lay het tat ca Book co trong csdl
                var users = db.KhachHang.Where(p => !p.isDeleted && (p.TenKH.Contains(searchString) || p.TaiKhoan.Contains(searchString) || p.SDT.Contains(searchString))).ToList();
                int pageSize = 10;
                int pageNumber = (page ?? 1);
                //Tra ve view
                if (users.Count == 0)
                    ViewBag.Messenge = "Không tìm thấy khách hàng theo yêu cầu!";
                return View(users.ToPagedList(pageNumber, pageSize));
            }
        }

        //Phuong thuc create, Cho nay la GET method
        public ActionResult Create()
        {
            ViewBag.Active = "User";
            //Tra ve view ten la "Create" khi goi "localhost:49897/Book/Create"
            return View();
        }

        //Phuong thuc Post Create (Sau khi click button xac nhan)
        [HttpPost]
        public ActionResult Create(KhachHang newUser)
        {
            ViewBag.Active = "User";
            if (newUser.MatKhau.Length < 6)
            {
                ViewBag.Messenge = "Mật khẩu phải có ít nhất 6 ký tự";
                return View();
            }
            try
            {
                using (var db = new DbContext())
                {
                    //Them sach moi vao csdl
                    db.KhachHang.Add(newUser);
                    db.SaveChanges();
                    //Tra ve view Index de xem sach da them
                    return RedirectToAction("Index");
                }
            }
            catch
            {
                ViewBag.Messenge = "Some thing wrong";
                return View();
            }
        }

        //Phuong thuc Edit, method GET (Lay Book can edit thong qua id)
        public ActionResult Edit(string id)
        {
            ViewBag.Active = "User";
            using (var db = new DbContext())
            {
                //Lay book theo id
                var user = db.KhachHang.Select(b => b).Where(b => b.TaiKhoan == id).FirstOrDefault();
                //Tra ve view Edit
                return View(user);
            }
        }

        //Phuong thuc Edit, sau khi da lay duoc sach thong qua id
        [HttpPost]
        public ActionResult Edit(KhachHang editedUser)
        {
            ViewBag.Active = "User";
            try
            {
                using (var db = new DbContext())
                {
                    //Edit tung property
                    var user = db.KhachHang.Select(p => p).Where(p => p.TaiKhoan == editedUser.TaiKhoan).FirstOrDefault();

                    user.TaiKhoan = editedUser.TaiKhoan;
                    user.GioiTinh = editedUser.GioiTinh;
                    user.MatKhau = editedUser.MatKhau;
                    user.NgaySinh = editedUser.NgaySinh;
                    user.SDT = editedUser.SDT;
                    user.TenKH = editedUser.TenKH;
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
            }
            catch
            {
                return View();
            }
        }

        public ActionResult Delete(string id)
        {
            ViewBag.Active = "User";
            using (var db = new DbContext())
            {
                var user = db.KhachHang.Select(p => p).Where(p => p.TaiKhoan == id).FirstOrDefault();
                return View(user);
            }
        }

        [HttpPost]
        public ActionResult Delete(string id, FormCollection collection)
        {
            ViewBag.Active = "User";
            try
            {
                using (var db = new DbContext())
                {
                    var user = db.KhachHang.Select(p => p).Where(p => p.TaiKhoan == id).FirstOrDefault();
                    if (user != null)
                        user.isDeleted = true;
                    db.SaveChanges();
                }
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}