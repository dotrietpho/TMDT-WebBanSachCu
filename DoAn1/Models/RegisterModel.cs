using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DoAn1.Models
{
    public class RegisterModel
    {
        public string TaiKhoan { get; set; }
        public string password { get; set; }
        public string ReTypedpassword { get; set; }
        public string TenKH { get; set; }
        public string SDT { get; set; }
        public string NgaySinh { get; set; }
    }
}