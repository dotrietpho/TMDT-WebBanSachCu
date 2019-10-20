using DoAn1.App_Data;
using PagedList;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DoAn1.Controllers
{
    public class BannerController : Controller
    {
        // GET: Banner
        public ActionResult Index(int? page)
        {
            ViewBag.Active = "Banner";
            if (Session.Count != 0)
            {
                using (var db = new DbContext())
                {
                    //Lay het tat ca Book co trong csdl
                    var books = db.Banner.ToList();
                    int pageSize = 3;
                    int pageNumber = (page ?? 1);
                    //Tra ve view
                    if (books.Count == 0)
                        ViewBag.Messenge = "";
                    return View(books.ToPagedList(pageNumber, pageSize));
                }
            }
            else
            {
                return Redirect(Url.Content("~/Admin"));
            }
        }

        public ActionResult Edit(int id)
        {
            ViewBag.Active = "Banner";
            using (var db = new DbContext())
            {
                //Lay book theo id
                var sach = db.Banner.Select(b => b).Where(b => b.id == id).FirstOrDefault();
                //Tra ve view Edit
                return View(sach);
            }
        }

        //Phuong thuc Edit, sau khi da lay duoc sach thong qua id
        [HttpPost]
        public ActionResult Edit(Banner editedBook, HttpPostedFileBase file)
        {
            ViewBag.Active = "Banner";
            try
            {
                using (var db = new DbContext())
                {
                    //Edit tung property
                    var book = db.Banner.Select(p => p).Where(p => p.id == editedBook.id).FirstOrDefault();

                    string _path = "";
                    if (file != null)
                    {
                        string _fileName = Path.GetFileName(file.FileName);
                        _path = Path.Combine(Server.MapPath("~/UploadedFiles"), _fileName);
                        file.SaveAs(_path);
                        book.LinkAnh = _fileName;
                    }

                    book.isHienThi = editedBook.isHienThi;
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
            }
            catch
            {
                return View();
            }
        }
    }
}