namespace QLNhaHang.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class fixHDCol : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.HoaDons", "MauSo", c => c.String(maxLength: 20, unicode: false));
            AlterColumn("dbo.HoaDons", "KyHieu", c => c.String(maxLength: 20, unicode: false));
            AlterColumn("dbo.HoaDons", "SoThuTu", c => c.String(maxLength: 20, unicode: false));
            DropColumn("dbo.HoaDons", "QuyenSo");
            DropColumn("dbo.HoaDons", "So");
        }
        
        public override void Down()
        {
            AddColumn("dbo.HoaDons", "So", c => c.String());
            AddColumn("dbo.HoaDons", "QuyenSo", c => c.String());
            AlterColumn("dbo.HoaDons", "SoThuTu", c => c.String());
            AlterColumn("dbo.HoaDons", "KyHieu", c => c.String());
            AlterColumn("dbo.HoaDons", "MauSo", c => c.String());
        }
    }
}
