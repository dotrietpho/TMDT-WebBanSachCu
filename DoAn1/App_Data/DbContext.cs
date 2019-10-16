using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;

namespace DoAn1.App_Data
{
    public class DbContext : System.Data.Entity.DbContext
    {
        public DbContext() : base() {
            string databasename = "DoAn1 - 31/05";
            this.Database.Connection.ConnectionString = "Data Source=.;Initial Catalog=" + databasename + ";Trusted_Connection=Yes";
        }
        public DbSet<Sach> Sach { get; set; }
        public DbSet<ChiTietGioHang> ChiTietGioHang { get; set; }
        public DbSet<GioHang> GioHang { get; set; }
        public DbSet<HoaDon> HoaDon { get; set; }
        public DbSet<KhachHang> KhachHang { get; set; }
        public DbSet<TaiKhoanAdmin> TaiKhoanAdmin { get; set; }
        public DbSet<ChiTietHoaDon> ChiTietHoaDons { get; set; }
        public DbSet<Banner> Banner { get; set; }
    }
}
