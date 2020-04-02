namespace QLNhaHang.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class fixBan : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Bans", "NgayTao", c => c.DateTime(nullable: false));
            AddColumn("dbo.Bans", "NguoiTao", c => c.String(maxLength: 50));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Bans", "NguoiTao");
            DropColumn("dbo.Bans", "NgayTao");
        }
    }
}
