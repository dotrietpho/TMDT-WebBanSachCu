﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DoAn1.App_Data
{
    public class ChiTietHoaDon
    {
        [Key]
        [Column(Order = 1)]
        public int idHoaDon { get; set; }

        [Key]
        [Column(Order = 2)]
        public int idSach { get; set; }

        public int count { get; set; }
        public HoaDon HoaDon { get; set; }
        public Sach Sach { get; set; }

        public ChiTietHoaDon(int idHoaDon, int idSach, int count)
        {
            this.idHoaDon = idHoaDon;
            this.idSach = idSach;
            this.count = count;
        }

        public ChiTietHoaDon()
        {
        }
    }
}