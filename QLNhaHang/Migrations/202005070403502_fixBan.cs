namespace QLNhaHang.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class fixBan : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Bans", "VanPhongId", "dbo.VanPhongs");
            DropIndex("dbo.Bans", new[] { "VanPhongId" });
            AddColumn("dbo.Bans", "TenVP", c => c.String(nullable: false, maxLength: 100));
            DropColumn("dbo.Bans", "VanPhongId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Bans", "VanPhongId", c => c.Int(nullable: false));
            DropColumn("dbo.Bans", "TenVP");
            CreateIndex("dbo.Bans", "VanPhongId");
            AddForeignKey("dbo.Bans", "VanPhongId", "dbo.VanPhongs", "Id", cascadeDelete: true);
        }
    }
}
