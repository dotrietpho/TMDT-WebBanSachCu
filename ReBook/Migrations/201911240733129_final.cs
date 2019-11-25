namespace ReBook.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class final : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.GioHangs", "TinhTrang");
        }
        
        public override void Down()
        {
            AddColumn("dbo.GioHangs", "TinhTrang", c => c.String());
        }
    }
}
