using ReBook.App_Data;
using System.Collections.Generic;
using System.Linq;

namespace ReBook.Models.Helper
{
    public class GioHangHelper
    {
        public bool isGioHangTonTai(string idGioHang)
        {
            using (var db = new DBConText())
            {
                if (db.GioHang.Where(p => p.IDGioHang == idGioHang).FirstOrDefault() == null)
                    return false;
                return true;
            }
        }

        public bool TaoGioHang(string idGioHang)
        {
            try
            {
                using (var db = new DBConText())
                {
                    var ghMoi = new GioHang(idGioHang);
                    db.GioHang.Add(ghMoi);
                    db.SaveChanges();
                }
                return true;
            }
            catch
            {
                return false;
            }
        }

        public bool ClearGioHang(string idGioHang)
        {
            try
            {
                using (var db = new DBConText())
                {
                    db.ChiTietGioHang.RemoveRange(db.ChiTietGioHang.Where(x => x.IDGioHang == idGioHang));
                    db.SaveChanges();
                    return true;
                }
            }
            catch
            {
                return false;
            }
        }

        public bool ThemSanPham(int idSach, string idGioHang, bool forceCreate = false)
        {
            try
            {
                using (var db = new DBConText())
                {
                    if (isGioHangTonTai(idGioHang))
                    {
                        if (forceCreate)
                        {
                            ClearGioHang(idGioHang);
                        }
                    }
                    else
                    {
                        TaoGioHang(idGioHang);
                    }

                    var checker = db.ChiTietGioHang.Where(p => p.IDGioHang == idGioHang && p.idSach == idSach).FirstOrDefault();
                    if (checker != null)
                    {
                        checker.count++;
                    }
                    else
                    {
                        db.ChiTietGioHang.Add(new ChiTietGioHang(idGioHang, idSach));
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
        public bool TangSanPham(int idSach, string idGioHang)
        {
            try
            {
                using (var db = new DBConText())
                {
                    var a = db.ChiTietGioHang.Where(p => p.IDGioHang == idGioHang && p.idSach == idSach).FirstOrDefault();
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
        public bool GiamSanPham(int idSach, string idGioHang)
        {
            try
            {
                using (var db = new DBConText())
                {
                    var a = db.ChiTietGioHang.Where(p => p.IDGioHang == idGioHang && p.idSach == idSach).FirstOrDefault();
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
        public bool XoaSanPham(int idSach, string idGioHang)
        {
            try
            {
                using (var db = new DBConText())
                {
                    var a = db.ChiTietGioHang.Where(p => p.IDGioHang == idGioHang && p.idSach == idSach).FirstOrDefault();
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

        public string TongTienGioHang(string idGioHang)
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
                            where p.IDGioHang == idGioHang
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
                    if (db.GioHang.Where(p => p.IDGioHang == idGioHang).FirstOrDefault() != null)
                        db.GioHang.Where(p => p.IDGioHang == idGioHang).FirstOrDefault().TongTienGioHang = total;
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

        public bool ChuyenGioHang(string idGioHang, string userID)
        {
            try
            {
                using (var db = new DBConText())
                {
                    var user = db.KhachHang.Find(userID);
                    var gioHang = db.GioHang.Find(idGioHang);
                    var _gioHangs = db.GioHang.Where(x => x.IDGioHang == user.idGioHang).ToList();

                    user.idGioHang = idGioHang;
                    user.GioHang = gioHang;
                    db.SaveChanges();

                    if (_gioHangs.Count() > 0)
                    {
                        var _gioHang = _gioHangs[0];
                        var _chiTietGH = db.ChiTietGioHang.Where(x => x.IDGioHang == _gioHang.IDGioHang);

                        if (_chiTietGH.Count() > 0)
                        {
                            db.ChiTietGioHang.RemoveRange(_chiTietGH);
                            db.SaveChanges();
                        }

                        db.GioHang.Remove(_gioHang);
                        db.SaveChanges();
                    }
                }
                return true;
            }
            catch
            {
                return false;
            }
        }

        public List<ChiTietGioHangModel> ChiTietGioHang(string idGioHang)
        {
            try
            {
                using (var db = new DBConText())
                {
                    var q = from p in db.ChiTietGioHang
                            where p.IDGioHang == idGioHang
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