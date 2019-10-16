using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoAn1.App_Data
{
    public class GioHang
    {
        [Key]
        public string IDGioHang { get; set; }
        public int TongTienGioHang { get; set; }
        public string TinhTrang { get; set; }
        public bool isDeleted { get; set; }
        public ICollection<ChiTietGioHang> ChiTietGioHangs { get; set; }


        public GioHang(string id)
        {
            this.IDGioHang = id;
            TinhTrang = "chưa thanh toán";
            TongTienGioHang = 0;
        }

        public GioHang()
        { }
    }
}
