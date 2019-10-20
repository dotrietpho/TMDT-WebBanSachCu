using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DoAn1.App_Data
{
    public class Sach
    {
        [Key]
        public int id { get; set; }

        public string TenSach { get; set; }
        public string HinhSach { get; set; }
        public string ChuDe { get; set; }
        public int GiaSach { get; set; }
        public string TenTacGia { get; set; }
        public int SoTrang { get; set; }
        public string NgayXuatBan { get; set; }
        public int SoLuongXem { get; set; }
        public string MoTa { get; set; }
        public bool isDeleted { get; set; }
        public ICollection<ChiTietGioHang> ChiTietGioHangs { get; set; }

        public Sach(int id, string ten, string ngayXuatBan, int gia, string tenTacGia, string chude)
        {
            this.id = id;
            TenSach = ten;
            NgayXuatBan = ngayXuatBan;
            ChuDe = chude;
            GiaSach = gia;
            TenTacGia = tenTacGia;
            isDeleted = false;
        }

        public Sach()
        {
        }

        public Sach(Sach a)
        {
            this.id = a.id;
            this.HinhSach = a.HinhSach;
            this.ChuDe = a.ChuDe;
            this.TenSach = a.TenSach;
            this.TenTacGia = a.TenTacGia;
            this.GiaSach = a.GiaSach;
            this.TenTacGia = a.TenTacGia;
            this.SoTrang = a.SoTrang;
            this.NgayXuatBan = a.NgayXuatBan;
            this.SoLuongXem = a.SoLuongXem;
            this.MoTa = a.MoTa;
            this.isDeleted = a.isDeleted;
        }
    }
}