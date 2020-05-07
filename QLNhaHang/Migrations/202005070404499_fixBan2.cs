namespace QLNhaHang.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class fixBan2 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Bans", "KhuVucId", c => c.Int(nullable: false));
            CreateIndex("dbo.Bans", "KhuVucId");
            AddForeignKey("dbo.Bans", "KhuVucId", "dbo.KhuVucs", "Id", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Bans", "KhuVucId", "dbo.KhuVucs");
            DropIndex("dbo.Bans", new[] { "KhuVucId" });
            DropColumn("dbo.Bans", "KhuVucId");
        }
    }
}
