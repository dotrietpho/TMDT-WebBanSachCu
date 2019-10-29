namespace ReBook.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class a1 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Banners",
                c => new
                {
                    id = c.Int(nullable: false, identity: true),
                    LinkAnh = c.String(),
                    isHienThi = c.Boolean(nullable: false),
                })
                .PrimaryKey(t => t.id);
        }

        public override void Down()
        {
            DropTable("dbo.Banners");
        }
    }
}