using ReBook.App_Data;
using System.Collections.Generic;
using System.Linq;

namespace ReBook.Models.Helper
{
    public class HoaDonHelper
    {
        public List<ChiTietGioHangModel> ChiTietHoaDon(int idHoaDon)
        {
            try
            {
                using (var db = new DBConText())
                {
                    var q = from p in db.ChiTietHoaDons
                            where p.idHoaDon == idHoaDon
                            from sach in db.Sach
                            where sach.id == p.idSach
                            from x in db.ChiTietHoaDons
                            where x.idSach == sach.id
                            select new { id = sach.id, TenSach = sach.TenSach, GiaSach = sach.GiaSach, HinhSach = sach.HinhSach, count = x.count };

                    IEnumerable<ChiTietGioHangModel> cart = from ca in q.AsEnumerable()
                                                            select new ChiTietGioHangModel
                                                            (
                                                                ca.id,
                                                                ca.TenSach,
                                                                ca.GiaSach,
                                                                ca.HinhSach,
                                                                ca.count
                                                            );
                    return cart.ToList();
                }
            }
            catch
            {
                throw;
            }
        }

        public void LapHoaDon(string idKhachHang, string idGioHang, string diaChi, string sdt, string ngayHen, string ghiChu, bool isPaid)
        {
            try
            {
                using (var db = new DBConText())
                {
                    var q = db.GioHang.Where(p => p.IDGioHang == idGioHang).FirstOrDefault();
                    HoaDon hoaDon = new HoaDon(isPaid ? "Đã thanh toán" : "Chờ xác nhận", q, diaChi, sdt, ngayHen, idKhachHang, ghiChu, isPaid);
                    db.HoaDon.Add(hoaDon);
                    db.SaveChanges();
                    LapChiTietHoaDon(hoaDon.id, idGioHang);
                }
            }
            catch
            {
                throw;
            }
        }

        public void LapChiTietHoaDon(int idhoaDon, string idGioHang)
        {
            try
            {
                using (var db = new DBConText())
                {
                    GioHangHelper a = new GioHangHelper();
                    var chitietGioHangs = a.ChiTietGioHang(idGioHang);
                    foreach (var item in chitietGioHangs)
                    {
                        db.ChiTietHoaDons.Add(new ChiTietHoaDon(idhoaDon, item.idSach, item.SoLuong));
                        var c = db.ChiTietGioHang.Where(p => p.IDGioHang == idGioHang && p.idSach == item.idSach).FirstOrDefault();
                        db.ChiTietGioHang.Remove(c);
                    }
                    var giohang = db.GioHang.Where(p => p.IDGioHang == idGioHang).FirstOrDefault();
                    db.GioHang.Remove(giohang);
                    db.SaveChanges();
                }
            }
            catch { throw; }
        }
    }
}