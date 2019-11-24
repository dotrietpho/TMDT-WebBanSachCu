﻿using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ReBook.App_Data
{
    public class HoaDon
    {
        [Key]
        public int id { get; set; }

        public string TinhTrang { get; set; }
        public int TongTien { get; set; }
        public string DiaChiGiaoHang { get; set; }
        public string SDTGiaoHang { get; set; }
        public string GhiChu { get; set; }
        public string NgayLapHD { get; set; }
        public string NgayHenGiaoHang { get; set; }
        public bool isDeleted { get; set; }
        public bool isPaid { get; set; }
        public string idKhachHang { get; set; }
        public KhachHang KhachHang { get; set; }

        public HoaDon(string tinhTrang, GioHang gioHang, string diaChi, string sdt, string ngayHen, string idKhachHang, string ghiChu, bool IsPaid)
        {
            this.TinhTrang = tinhTrang;
            this.TongTien = gioHang.TongTienGioHang;
            this.DiaChiGiaoHang = diaChi;
            this.SDTGiaoHang = sdt;
            this.NgayHenGiaoHang = ngayHen;
            this.idKhachHang = idKhachHang;
            this.NgayLapHD = DateTime.Now.ToString();
            this.GhiChu = ghiChu;
            this.isPaid = IsPaid;
        }

        public HoaDon()
        {
        }
    }
}