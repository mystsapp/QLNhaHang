namespace QLNhaHang.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addVPToHD : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.HoaDons", "MaBan", "dbo.Bans");
            DropForeignKey("dbo.HoaDons", "MaNV", "dbo.NhanViens");
            DropIndex("dbo.HoaDons", new[] { "MaNV" });
            DropIndex("dbo.HoaDons", new[] { "MaBan" });
            AddColumn("dbo.HoaDons", "TenNV", c => c.String(maxLength: 50));
            AddColumn("dbo.HoaDons", "TenBan", c => c.String(nullable: false, maxLength: 50));
            DropColumn("dbo.HoaDons", "MaNV");
            DropColumn("dbo.HoaDons", "MaBan");
        }
        
        public override void Down()
        {
            AddColumn("dbo.HoaDons", "MaBan", c => c.String(maxLength: 20, unicode: false));
            AddColumn("dbo.HoaDons", "MaNV", c => c.String(maxLength: 20, unicode: false));
            DropColumn("dbo.HoaDons", "TenBan");
            DropColumn("dbo.HoaDons", "TenNV");
            CreateIndex("dbo.HoaDons", "MaBan");
            CreateIndex("dbo.HoaDons", "MaNV");
            AddForeignKey("dbo.HoaDons", "MaNV", "dbo.NhanViens", "MaNV");
            AddForeignKey("dbo.HoaDons", "MaBan", "dbo.Bans", "MaBan");
        }
    }
}
