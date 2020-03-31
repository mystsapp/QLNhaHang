namespace QLNhaHang.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addVPHD : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.HoaDons", "VanPhongId", c => c.Int(nullable: false));
            CreateIndex("dbo.HoaDons", "VanPhongId");
            AddForeignKey("dbo.HoaDons", "VanPhongId", "dbo.VanPhongs", "Id", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.HoaDons", "VanPhongId", "dbo.VanPhongs");
            DropIndex("dbo.HoaDons", new[] { "VanPhongId" });
            DropColumn("dbo.HoaDons", "VanPhongId");
        }
    }
}
