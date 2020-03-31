namespace QLNhaHang.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addVPToHD3 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.HoaDons", "MaNV", c => c.String(maxLength: 20, unicode: false));
            AddColumn("dbo.HoaDons", "MaBan", c => c.String(maxLength: 20, unicode: false));
            CreateIndex("dbo.HoaDons", "MaNV");
            CreateIndex("dbo.HoaDons", "MaBan");
            AddForeignKey("dbo.HoaDons", "MaBan", "dbo.Bans", "MaBan");
            AddForeignKey("dbo.HoaDons", "MaNV", "dbo.NhanViens", "MaNV");
            DropColumn("dbo.HoaDons", "TenNV");
            DropColumn("dbo.HoaDons", "VanPhongId");
            DropColumn("dbo.HoaDons", "TenBan");
        }
        
        public override void Down()
        {
            AddColumn("dbo.HoaDons", "TenBan", c => c.String(nullable: false, maxLength: 50));
            AddColumn("dbo.HoaDons", "VanPhongId", c => c.Int(nullable: false));
            AddColumn("dbo.HoaDons", "TenNV", c => c.String(maxLength: 50));
            DropForeignKey("dbo.HoaDons", "MaNV", "dbo.NhanViens");
            DropForeignKey("dbo.HoaDons", "MaBan", "dbo.Bans");
            DropIndex("dbo.HoaDons", new[] { "MaBan" });
            DropIndex("dbo.HoaDons", new[] { "MaNV" });
            DropColumn("dbo.HoaDons", "MaBan");
            DropColumn("dbo.HoaDons", "MaNV");
        }
    }
}
