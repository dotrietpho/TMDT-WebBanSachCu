using System.ComponentModel.DataAnnotations;

namespace ReBook.App_Data
{
    public class Banner
    {
        [Key]
        public int id { get; set; }

        public string LinkAnh { get; set; }
        public bool isHienThi { get; set; }

        public Banner()
        {
        }

        public Banner(int id, string linkanh, bool hienthi)
        {
            this.id = id;
            this.LinkAnh = linkanh;
            this.isHienThi = hienthi;
        }
    }
}