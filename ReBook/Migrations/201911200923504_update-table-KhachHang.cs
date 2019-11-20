namespace ReBook.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class updatetableKhachHang : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.KhachHangs", "DiaChi", c => c.String());
            AddColumn("dbo.KhachHangs", "Email", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.KhachHangs", "Email");
            DropColumn("dbo.KhachHangs", "DiaChi");
        }
    }
}
