﻿using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ReBook.App_Data
{
    public class GioHang
    {
        [Key]
        public string IDGioHang { get; set; }

        public int TongTienGioHang { get; set; }
        public bool isDeleted { get; set; }
        public ICollection<ChiTietGioHang> ChiTietGioHangs { get; set; }

        public GioHang(string id)
        {
            this.IDGioHang = id;
            TongTienGioHang = 0;
        }

        public GioHang()
        { }
    }
}