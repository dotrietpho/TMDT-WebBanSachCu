namespace ReBook.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class createdb : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ChiTietGioHangs",
                c => new
                {
                    IDGioHang = c.String(nullable: false, maxLength: 128),
                    idSach = c.Int(nullable: false),
                    count = c.Int(nullable: false),
                    Sach_id = c.Int(),
                })
                .PrimaryKey(t => new { t.IDGioHang, t.idSach })
                .ForeignKey("dbo.GioHangs", t => t.IDGioHang, cascadeDelete: true)
                .ForeignKey("dbo.Saches", t => t.Sach_id)
                .Index(t => t.IDGioHang)
                .Index(t => t.Sach_id);

            CreateTable(
                "dbo.GioHangs",
                c => new
                {
                    IDGioHang = c.String(nullable: false, maxLength: 128),
                    TongTienGioHang = c.Int(nullable: false),
                    TinhTrang = c.String(),
                    isDeleted = c.Boolean(nullable: false),
                })
                .PrimaryKey(t => t.IDGioHang);

            CreateTable(
                "dbo.Saches",
                c => new
                {
                    id = c.Int(nullable: false, identity: true),
                    TenSach = c.String(),
                    HinhSach = c.String(),
                    ChuDe = c.String(),
                    GiaSach = c.Int(nullable: false),
                    TenTacGia = c.String(),
                    SoTrang = c.Int(nullable: false),
                    NgayXuatBan = c.String(),
                    SoLuongXem = c.Int(nullable: false),
                    MoTa = c.String(),
                    isDeleted = c.Boolean(nullable: false),
                })
                .PrimaryKey(t => t.id);

            CreateTable(
                "dbo.ChiTietHoaDons",
                c => new
                {
                    idHoaDon = c.String(nullable: false, maxLength: 128),
                    idSach = c.Int(nullable: false),
                    count = c.Int(nullable: false),
                    HoaDon_id = c.Int(),
                    Sach_id = c.Int(),
                })
                .PrimaryKey(t => new { t.idHoaDon, t.idSach })
                .ForeignKey("dbo.HoaDons", t => t.HoaDon_id)
                .ForeignKey("dbo.Saches", t => t.Sach_id)
                .Index(t => t.HoaDon_id)
                .Index(t => t.Sach_id);

            CreateTable(
                "dbo.HoaDons",
                c => new
                {
                    id = c.Int(nullable: false, identity: true),
                    TinhTrang = c.String(),
                    TongTien = c.Int(nullable: false),
                    DiaChiGiaoHang = c.String(),
                    SDTGiaoHang = c.String(),
                    GhiChu = c.String(),
                    NgayLapHD = c.String(),
                    NgayHenGiaoHang = c.String(),
                    isDeleted = c.Boolean(nullable: false),
                    idKhachHang = c.String(),
                    KhachHang_TaiKhoan = c.String(maxLength: 128),
                })
                .PrimaryKey(t => t.id)
                .ForeignKey("dbo.KhachHangs", t => t.KhachHang_TaiKhoan)
                .Index(t => t.KhachHang_TaiKhoan);

            CreateTable(
                "dbo.KhachHangs",
                c => new
                {
                    TaiKhoan = c.String(nullable: false, maxLength: 128),
                    MatKhau = c.String(),
                    TenKH = c.String(),
                    NgaySinh = c.String(),
                    SDT = c.String(),
                    GioiTinh = c.String(),
                    SoLanTruyCap = c.Int(nullable: false),
                    AnhDaiDien = c.String(),
                    isDeleted = c.Boolean(nullable: false),
                    idGioHang = c.String(maxLength: 128),
                })
                .PrimaryKey(t => t.TaiKhoan)
                .ForeignKey("dbo.GioHangs", t => t.idGioHang)
                .Index(t => t.idGioHang);

            CreateTable(
                "dbo.TaiKhoanAdmins",
                c => new
                {
                    id = c.String(nullable: false, maxLength: 128),
                    TaiKhoan = c.String(),
                    MatKhau = c.String(),
                })
                .PrimaryKey(t => t.id);
        }

        public override void Down()
        {
            DropForeignKey("dbo.ChiTietHoaDons", "Sach_id", "dbo.Saches");
            DropForeignKey("dbo.ChiTietHoaDons", "HoaDon_id", "dbo.HoaDons");
            DropForeignKey("dbo.HoaDons", "KhachHang_TaiKhoan", "dbo.KhachHangs");
            DropForeignKey("dbo.KhachHangs", "idGioHang", "dbo.GioHangs");
            DropForeignKey("dbo.ChiTietGioHangs", "Sach_id", "dbo.Saches");
            DropForeignKey("dbo.ChiTietGioHangs", "IDGioHang", "dbo.GioHangs");
            DropIndex("dbo.KhachHangs", new[] { "idGioHang" });
            DropIndex("dbo.HoaDons", new[] { "KhachHang_TaiKhoan" });
            DropIndex("dbo.ChiTietHoaDons", new[] { "Sach_id" });
            DropIndex("dbo.ChiTietHoaDons", new[] { "HoaDon_id" });
            DropIndex("dbo.ChiTietGioHangs", new[] { "Sach_id" });
            DropIndex("dbo.ChiTietGioHangs", new[] { "IDGioHang" });
            DropTable("dbo.TaiKhoanAdmins");
            DropTable("dbo.KhachHangs");
            DropTable("dbo.HoaDons");
            DropTable("dbo.ChiTietHoaDons");
            DropTable("dbo.Saches");
            DropTable("dbo.GioHangs");
            DropTable("dbo.ChiTietGioHangs");
        }
    }
}