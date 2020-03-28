namespace QLNhaHang.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addKHToHD : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.HoaDons", "MaKH", "dbo.KhachHangs");
            DropIndex("dbo.HoaDons", new[] { "MaKH" });
            AddColumn("dbo.HoaDons", "TenKH", c => c.String(maxLength: 100));
            AddColumn("dbo.HoaDons", "Phone", c => c.String(maxLength: 15, unicode: false));
            AddColumn("dbo.HoaDons", "DiaChi", c => c.String(maxLength: 250));
            AddColumn("dbo.HoaDons", "TenDonVi", c => c.String(maxLength: 100));
            AddColumn("dbo.HoaDons", "MaSoThue", c => c.String(maxLength: 20, unicode: false));
            DropColumn("dbo.HoaDons", "MaKH");
        }
        
        public override void Down()
        {
            AddColumn("dbo.HoaDons", "MaKH", c => c.String(maxLength: 50, unicode: false));
            DropColumn("dbo.HoaDons", "MaSoThue");
            DropColumn("dbo.HoaDons", "TenDonVi");
            DropColumn("dbo.HoaDons", "DiaChi");
            DropColumn("dbo.HoaDons", "Phone");
            DropColumn("dbo.HoaDons", "TenKH");
            CreateIndex("dbo.HoaDons", "MaKH");
            AddForeignKey("dbo.HoaDons", "MaKH", "dbo.KhachHangs", "MaKH");
        }
    }
}
