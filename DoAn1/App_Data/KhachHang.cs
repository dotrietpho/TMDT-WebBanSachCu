using System.ComponentModel.DataAnnotations;

namespace DoAn1.App_Data
{
    public class KhachHang
    {
        [Key]
        public string TaiKhoan { get; set; }

        public string MatKhau { get; set; }
        public string TenKH { get; set; }
        public string NgaySinh { get; set; }
        public string SDT { get; set; }
        public string GioiTinh { get; set; }
        public int SoLanTruyCap { get; set; }
        public string AnhDaiDien { get; set; }
        public bool isDeleted { get; set; }
        public string idGioHang { get; set; }

        public virtual GioHang GioHang { get; set; }

        public KhachHang()
        { }

        public KhachHang(string taikhoan, string matkhau, string tenKH, string NgaySinh)
        {
            this.TaiKhoan = taikhoan;
            this.MatKhau = matkhau;
            this.TenKH = tenKH;
            this.NgaySinh = NgaySinh;
        }

        public KhachHang(string taikhoan, string matkhau, string tenKH, string SDT, string ngaysinh)
        {
            this.TaiKhoan = taikhoan;
            this.MatKhau = matkhau;
            this.TenKH = tenKH;
            this.NgaySinh = ngaysinh;
            this.SDT = SDT;
        }

        public KhachHang(KhachHang a)
        {
            this.TaiKhoan = a.TaiKhoan;
            this.MatKhau = a.MatKhau;
            this.TenKH = a.TenKH;
            this.NgaySinh = a.NgaySinh;
            this.SDT = a.SDT;
            this.GioiTinh = a.GioiTinh;
            this.SoLanTruyCap = a.SoLanTruyCap;
            this.AnhDaiDien = a.AnhDaiDien;
            this.isDeleted = a.isDeleted;
            this.idGioHang = a.idGioHang;
            this.GioHang = a.GioHang;
        }
    }
}