using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ReBook.Models
{
    public class ChangePasswordModel
    {
        public string MatKhau { get; set; }
        public string MatKhauMoi { get; set; }
        public string MatKhauMoiNhapLai { get; set; }

        public ChangePasswordModel(string matkhau, string matkhaumoi, string matkhaumoinl)
        {
            this.MatKhau = matkhau;
            this.MatKhauMoi = matkhaumoi;
            this.MatKhauMoiNhapLai = matkhaumoinl;
        }

        public ChangePasswordModel()
        { }
    }
}