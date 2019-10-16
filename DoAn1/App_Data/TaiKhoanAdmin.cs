   using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
