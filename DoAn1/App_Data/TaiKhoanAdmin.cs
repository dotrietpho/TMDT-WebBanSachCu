using System.ComponentModel.DataAnnotations;

namespace DoAn1.App_Data
{
    public class TaiKhoanAdmin
    {
        [Key]
        public string id { get; set; }

        public string TaiKhoan { get; set; }
        public string MatKhau { get; set; }
    }
}