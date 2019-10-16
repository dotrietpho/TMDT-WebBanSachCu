using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DoAn1.Models
{
    public class ChiTietGioHangModel
    {
        public int idSach { get; set; }
        public string TenSach { get; set; }
        public int GiaSach { get; set; }
        public string HinhSach { get; set; }
        public int SoLuong { get; set; }

        public ChiTietGioHangModel(int id, string ten, int gia, string hinh, int soluong)
        {
            this.idSach = id;
            this.TenSach = ten;
            this.GiaSach = gia;
            this.HinhSach = hinh;
            this.SoLuong = soluong;
        }
        public ChiTietGioHangModel() { }
    }
}