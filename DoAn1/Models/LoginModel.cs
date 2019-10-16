using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DoAn1.Models
{
    public class LoginModel
    {
        public string TaiKhoan { get; set; }
        public string MatKhau { get; set; }
        public string TenKH { get; set; }
        public LoginModel(string taikhoan, string matkhau, string tenkh)
        {
            this.TaiKhoan = taikhoan;
            this.MatKhau = matkhau;
            this.TenKH = tenkh;
        }
        public LoginModel()
        { }
    }
}