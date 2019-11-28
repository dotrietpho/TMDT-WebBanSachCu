using ReBook.App_Data;
using System.Collections.Generic;
using System.Linq;

namespace ReBook.Models
{
    public class GioHangHelper
    {
        public bool isGioHangTonTai(string userID)
        {
            using (var db = new DBConText())
            {
                if (db.GioHang.Where(p => p.IDGioHang == userID).FirstOrDefault() == null)
                    return false;
                return true;
            }
        }

        public bool TaoGioHang(string userID)
        {
            try
            {
                using (var db = new DBConText())
                {
                    db.GioHang.Add(new GioHang(userID));
                    db.SaveChanges();
                }
                return true;
            }
            catch
            {
                return false;
            }
        }

        public bool ThemSanPham(int idSach, string userID)
        {
            try
            {
                using (var db = new DBConText())
                {
                    var checker = db.ChiTietGioHang.Where(p => p.IDGioHang == userID && p.idSach == idSach).FirstOrDefault();
                    if (checker != null)
                    {
                        checker.count++;
                    }
                    else
                    {
                        db.ChiTietGioHang.Add(new ChiTietGioHang(userID, idSach));
                    }
                    db.SaveChanges();
                    return true;
                }
            }
            catch
            {
                return false;
            }
        }

        //Tang so luong sach trong gio hang
        public bool TangSanPham(int idSach, string userID)
        {
            try
            {
                using (var db = new DBConText())
                {
                    var a = db.ChiTietGioHang.Where(p => p.IDGioHang == userID && p.idSach == idSach).FirstOrDefault();
                    a.count++;
                    db.SaveChanges();
                    return true;
                }
            }
            catch
            {
                return false;
            }
        }

        //Giam so luong sach trong gio hang
        public bool XoaSanPham(int idSach, string userID)
        {
            try
            {
                using (var db = new DBConText())
                {
                    var a = db.ChiTietGioHang.Where(p => p.IDGioHang == userID && p.idSach == idSach).FirstOrDefault();
                    if (a.count > 1)
                        a.count--;
                    db.SaveChanges();
                    return true;
                }
            }
            catch
            {
                return false;
            }
        }

        //Xoa sach khoi gio hang
        public bool RemoveSanPham(int idSach, string userID)
        {
            try
            {
                using (var db = new DBConText())
                {
                    var a = db.ChiTietGioHang.Where(p => p.IDGioHang == userID && p.idSach == idSach).FirstOrDefault();
                    db.ChiTietGioHang.Remove(a);
                    db.SaveChanges();
                    return true;
                }
            }
            catch
            {
                return false;
            }
        }

        public string TongTienGioHang(string userID)
        {
            try
            {
                int total = 0;
                using (var db = new DBConText())
                {
                    //Chọn ChiTietGioHangModel trực tiếp bằng query gây ra lỗi (không sử dụng được thực thể có dữ liệu hoặc constructor cho query)
                    //Sửa bằng cách
                    //B1: chọn dữ liệu tạm bằng query
                    var q = from p in db.ChiTietGioHang
                            where p.IDGioHang == userID
                            from sach in db.Sach
                            where sach.id == p.idSach
                            from x in db.ChiTietGioHang
                            where x.idSach == sach.id
                            select new { id = sach.id, TenSach = sach.TenSach, GiaSach = sach.GiaSach, HinhSach = sach.HinhSach, count = x.count };
                    //B2: lấy dữ liệu tạm từ query gắn vào model
                    IEnumerable<ChiTietGioHangModel> cart = from ca in q.AsEnumerable()
                                                            select new ChiTietGioHangModel
                                                            (
                                                                ca.id,
                                                                ca.TenSach,
                                                                ca.GiaSach,
                                                                ca.HinhSach,
                                                                ca.count
                                                            );
                    List<ChiTietGioHangModel> listed = cart.ToList();
                    foreach (var item in listed)
                    {
                        total += item.GiaSach * item.SoLuong;
                    }
                    if (db.GioHang.Where(p => p.IDGioHang == userID).FirstOrDefault() != null)
                        db.GioHang.Where(p => p.IDGioHang == userID).FirstOrDefault().TongTienGioHang = total;
                    db.SaveChanges();
                }
                string s = string.Format("{0}", total);
                return s;
            }
            catch
            {
                throw;
            }
        }

        public bool ChuyenGioHang(string userKhach, string userDaDangNhap)
        {
            try
            {
                using (var db = new DBConText())
                {
                    var ghKhachs = db.GioHang.Where(x => x.IDGioHang == userKhach);
                    var ghDaDangNhaps = db.GioHang.Where(x => x.IDGioHang == userDaDangNhap);
                    bool daTaoGH = false;

                    // TK Khách có GH
                    if (ghKhachs.Count() > 0 && ghDaDangNhaps.Count() <= 0)
                    {
                        TaoGioHang(userDaDangNhap);
                        daTaoGH = true;
                    }

                    // TK Đăng nhập có GH
                    // Không chuyển

                    // Cả 2 TK đều có giỏ hàng
                    if (ghKhachs.Count() > 0 && (ghDaDangNhaps.Count() > 0) || daTaoGH == true)
                    {
                        var chitietXoa = db.ChiTietGioHang.Where(x => x.IDGioHang == userDaDangNhap);
                        db.ChiTietGioHang.RemoveRange(chitietXoa);

                        var chitietChuyen = db.ChiTietGioHang.Where(x => x.IDGioHang == userKhach);
                        foreach (var ct in chitietChuyen)
                        {
                            ct.IDGioHang = userDaDangNhap;
                        }

                        var ghKhach = ghKhachs.First();
                        db.GioHang.Remove(ghKhach);

                        db.SaveChanges();
                    }

                    return true;
                }
            }
            catch
            {
                return false;
            }
        }

        public List<ChiTietGioHangModel> ChiTietGioHang(string userID)
        {
            try
            {
                using (var db = new DBConText())
                {
                    var q = from p in db.ChiTietGioHang
                            where p.IDGioHang == userID
                            from sach in db.Sach
                            where sach.id == p.idSach
                            from x in db.ChiTietGioHang
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
    }
}