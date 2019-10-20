using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DoAn1.App_Data
{
    public class ChiTietGioHang
    {
        [Key]
        [Column(Order = 1)]
        public string IDGioHang { get; set; }

        [Key]
        [Column(Order = 2)]
        public int idSach { get; set; }

        public int count { get; set; }
        public GioHang GioHang { get; set; }
        public Sach Sach { get; set; }

        public ChiTietGioHang(string idGioHang, int idSach)
        {
            this.IDGioHang = idGioHang;
            this.idSach = idSach;
            this.count = 1;
        }

        public ChiTietGioHang()
        {
        }
    }
}