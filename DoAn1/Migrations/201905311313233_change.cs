namespace DoAn1.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class change : DbMigration
    {
        public override void Up()
        {
            DropPrimaryKey("dbo.ChiTietHoaDons");
            AlterColumn("dbo.ChiTietHoaDons", "idHoaDon", c => c.Int(nullable: false));
            AddPrimaryKey("dbo.ChiTietHoaDons", new[] { "idHoaDon", "idSach" });
        }
        
        public override void Down()
        {
            DropPrimaryKey("dbo.ChiTietHoaDons");
            AlterColumn("dbo.ChiTietHoaDons", "idHoaDon", c => c.String(nullable: false, maxLength: 128));
            AddPrimaryKey("dbo.ChiTietHoaDons", new[] { "idHoaDon", "idSach" });
        }
    }
}
