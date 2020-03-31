namespace QLNhaHang.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addVPBan : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Bans", "VanPhongId", c => c.Int(nullable: false));
            CreateIndex("dbo.Bans", "VanPhongId");
            AddForeignKey("dbo.Bans", "VanPhongId", "dbo.VanPhongs", "Id", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Bans", "VanPhongId", "dbo.VanPhongs");
            DropIndex("dbo.Bans", new[] { "VanPhongId" });
            DropColumn("dbo.Bans", "VanPhongId");
        }
    }
}
