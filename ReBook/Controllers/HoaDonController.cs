using PagedList;
using ReBook.App_Data;
using ReBook.Models;
using System.Linq;
using System.Web.Mvc;

namespace ReBook.Controllers
{
    public class HoaDonController : Controller
    {
        // GET: HoaDon
        public ActionResult Index(int? page, string searchString)
        {
            ViewBag.Active = "HoaDon";
            ViewBag.Search = searchString;
            if (Session.Count != 0)
            {
                if (page == null) page = 1;
                if (searchString == null)
                    searchString = "";
                using (var db = new DBConText())
                {
                    //Lay het tat ca hoadon co trong csdl phu hop voi searchString
                    var hoadons = db.HoaDon.Where(p => !p.isDeleted && (p.idKhachHang.Contains(searchString) || p.SDTGiaoHang.Contains(searchString) || p.TinhTrang.Contains(searchString))).OrderBy(p => p.id).ToList();
                    int pageSize = 5;
                    int pageNumber = (page ?? 1);
                    //Neu khong co hoadon phu hop voi searchString
                    if (hoadons.Count == 0)
                        ViewBag.Messenge = "Không tìm được hoá đơn theo yêu cầu!";
                    return View(hoadons.ToPagedList(pageNumber, pageSize));
                }
            }
            else
            {
                return Redirect(Url.Content("~/Admin"));
            }
        }

        public ActionResult Edit(int id)
        {
            ViewBag.Active = "HoaDon";
            using (var db = new DBConText())
            {
                //Lay book theo id
                var hoadon = db.HoaDon.Select(b => b).Where(b => b.id == id).FirstOrDefault();
                //Tra ve view Edit
                var helper = new HoaDonHelper();
                try
                {
                    ViewBag.ListBook = helper.ChiTietHoaDon(id);
                    ViewBag.TongGia = hoadon.TongTien.ToString();
                    if (helper.ChiTietHoaDon(id).Count == 0)
                        TempData["listemptyMessage"] = "No results";
                    return View(hoadon);
                }
                catch
                {
                    throw;
                }
            }
        }

        [HttpPost]
        public ActionResult Edit(HoaDon hoaDon)
        {
            ViewBag.Active = "HoaDon";
            try
            {
                using (var db = new DBConText())
                {
                    var hoadon = db.HoaDon.Select(p => p).Where(p => p.id == hoaDon.id).FirstOrDefault();
                    //Edit tung property
                    if (hoadon.TinhTrang == "Chờ xác nhận" || hoadon.TinhTrang == "Đã thanh toán")
                    {
                        hoadon.TinhTrang = "Đang giao hàng";
                    }
                    else if (hoadon.TinhTrang == "Đang giao hàng")
                    {
                        hoadon.TinhTrang = "Hoàn thành";
                    }
                    else if (hoadon.TinhTrang != "Hoàn thành" && hoadon.TinhTrang != "Đã huỷ")
                    {
                        hoadon.TinhTrang = "Không xác định";
                    }
                    db.SaveChanges();
                    return Redirect(Request.UrlReferrer.PathAndQuery);
                }
            }
            catch
            {
                return View();
            }
        }

        public ActionResult Delete(int id)
        {
            ViewBag.Active = "HoaDon";
            using (var db = new DBConText())
            {
                var hoadon = db.HoaDon.Select(p => p).Where(p => p.id == id).FirstOrDefault();
                var helper = new HoaDonHelper();
                try
                {
                    ViewBag.ListBook = helper.ChiTietHoaDon(id);
                    ViewBag.TongGia = hoadon.TongTien.ToString();
                    if (helper.ChiTietHoaDon(id).Count == 0)
                        TempData["listemptyMessage"] = "No results";
                    return View(hoadon);
                }
                catch
                {
                    throw;
                }
            }
        }

        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            ViewBag.Active = "HoaDon";
            try
            {
                using (var db = new DBConText())
                {
                    var sach = db.HoaDon.Select(p => p).Where(p => p.id == id).FirstOrDefault();
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

        public ActionResult ChangeStatus(int id)
        {
            ViewBag.Active = "HoaDon";
            try
            {
                using (var db = new DBConText())
                {
                    var hoadon = db.HoaDon.Select(p => p).Where(p => p.id == id).FirstOrDefault();
                    if (hoadon.TinhTrang == "Chờ xác nhận")
                        hoadon.TinhTrang = "Đang giao hàng";
                    else if (hoadon.TinhTrang == "Đang giao hàng")
                        hoadon.TinhTrang = "Hoàn thành";
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