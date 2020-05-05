namespace QLNhaHang.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addTTHDToVP : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.VanPhongs", "MauSo", c => c.String(maxLength: 20, unicode: false));
            AddColumn("dbo.VanPhongs", "KyHieu", c => c.String(maxLength: 20, unicode: false));
            AddColumn("dbo.VanPhongs", "QuyenSo", c => c.Int(nullable: false));
            AddColumn("dbo.VanPhongs", "So", c => c.Long(nullable: false));
            AddColumn("dbo.VanPhongs", "SoThuTu", c => c.String(nullable: false, maxLength: 20, unicode: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.VanPhongs", "SoThuTu");
            DropColumn("dbo.VanPhongs", "So");
            DropColumn("dbo.VanPhongs", "QuyenSo");
            DropColumn("dbo.VanPhongs", "KyHieu");
            DropColumn("dbo.VanPhongs", "MauSo");
        }
    }
}
