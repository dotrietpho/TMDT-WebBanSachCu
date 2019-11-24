namespace ReBook.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class editHoaDon : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.HoaDons", "isPaid", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.HoaDons", "isPaid");
        }
    }
}
