using System.ComponentModel.DataAnnotations;

namespace ReBook.App_Data
{
    public class TaiKhoanAdmin
    {
        [Key]
        public string id { get; set; }

        public string TaiKhoan { get; set; }
        public string MatKhau { get; set; }
    }
}