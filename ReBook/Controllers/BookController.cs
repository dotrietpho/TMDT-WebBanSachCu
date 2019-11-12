using PagedList;
using ReBook.App_Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ReBook.Controllers
{
    public class BookController : Controller
    {
        // GET: Book
        public ActionResult Index(int? page, string searchString)
        {
          
            ViewBag.Active = "Book";
            ViewBag.Search = searchString;
            if (Session.Count != 0)
            {
                if (page == null) page = 1;
                if (searchString == null)
                    searchString = "";
                using (var db = new DBConText())
                {
                    //Lay het tat ca Book co trong csdl
                    var books = db.Sach.Where(p => !p.isDeleted && (StringHelper.convertToUnSign(p.TenSach).Contains(searchString) || p.ChuDe.Contains(searchString) || p.TenTacGia.Contains(searchString))).OrderBy(p => p.id).ToList();
                    int pageSize = 3;
                    int pageNumber = (page ?? 1);
                    //Tra ve view
                    if (books.Count == 0)
                        ViewBag.Messenge = "Không tìm được sách theo yêu cầu!";
                    return View(books.ToPagedList(pageNumber, pageSize));
                }
            }
            else
            {
                return Redirect(Url.Content("~/Admin"));
            }
        }

        //Phuong thuc create, Cho nay la GET method
        public ActionResult Create()
        {
            ViewBag.Active = "Book";
            //Tra ve view ten la "Create" khi goi "localhost:49897/Book/Create"
            return View();
        }

        //Phuong thuc Post Create (Sau khi click button xac nhan)
        [HttpPost]
        public ActionResult Create([Bind(Exclude = "id")]Sach newBook, HttpPostedFileBase file)
        {
            ViewBag.Active = "Book";
            try
            {
                string _path = "";
                if (file.ContentLength > 0)
                {
                    string _fileName = Path.GetFileName(file.FileName);
                    _path = Path.Combine(Server.MapPath("~/UploadedFiles"), _fileName);
                    file.SaveAs(_path);
                    newBook.HinhSach = _fileName;
                }

                using (var db = new DBConText())
                {
                    //Them sach moi vao csdl
                    db.Sach.Add(newBook);
                    db.SaveChanges();
                    ViewBag.Messenge = "Successed";
                    return RedirectToAction("Index");
                }
            }
            catch
            {
                ViewBag.Messenge = "Some thing Wong";
                return View();
            }
        }

        //Phuong thuc Edit, method GET (Lay Book can edit thong qua id)
        public ActionResult Edit(int id)
        {
            ViewBag.Active = "Book";
            using (var db = new DBConText())
            {
                //Lay book theo id
                var sach = db.Sach.Select(b => b).Where(b => b.id == id).FirstOrDefault();
                //Tra ve view Edit
                return View(sach);
            }
        }

        //Phuong thuc Edit, sau khi da lay duoc sach thong qua id
        [HttpPost]
        public ActionResult Edit(Sach editedBook, HttpPostedFileBase file)
        {
            ViewBag.Active = "Book";
            try
            {
                using (var db = new DBConText())
                {
                    //Edit tung property
                    var book = db.Sach.Select(p => p).Where(p => p.id == editedBook.id).FirstOrDefault();

                    string _path = "";
                    if (file != null)
                    {
                        string _fileName = Path.GetFileName(file.FileName);
                        _path = Path.Combine(Server.MapPath("~/UploadedFiles"), _fileName);
                        file.SaveAs(_path);
                        book.HinhSach = _fileName;
                    }

                    book.TenSach = editedBook.TenSach;
                    book.SoTrang = editedBook.SoTrang;
                    book.ChuDe = editedBook.ChuDe;
                    book.GiaSach = editedBook.GiaSach;
                    book.MoTa = editedBook.MoTa;
                    book.isDeleted = editedBook.isDeleted;
                    book.TenTacGia = editedBook.TenTacGia;
                    book.SoLuongXem = editedBook.SoLuongXem;
                    book.NgayXuatBan = editedBook.NgayXuatBan;
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
            }
            catch
            {
                return View();
            }
        }

        public ActionResult Delete(int id)
        {
            ViewBag.Active = "Book";
            using (var db = new DBConText())
            {
                var book = db.Sach.Select(p => p).Where(p => p.id == id).FirstOrDefault();
                return View(book);
            }
        }

        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            ViewBag.Active = "Book";
            try
            {
                using (var db = new DBConText())
                {
                    var sach = db.Sach.Select(p => p).Where(p => p.id == id).FirstOrDefault();
                    if (sach != null)
                        sach.isDeleted = true;
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