namespace QLNhaHang.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addKVTbl : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.KhuVucs",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 50),
                        GhiChu = c.String(maxLength: 250),
                        VanPhongId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.VanPhongs", t => t.VanPhongId, cascadeDelete: true)
                .Index(t => t.VanPhongId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.KhuVucs", "VanPhongId", "dbo.VanPhongs");
            DropIndex("dbo.KhuVucs", new[] { "VanPhongId" });
            DropTable("dbo.KhuVucs");
        }
    }
}
