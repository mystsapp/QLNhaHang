namespace QLNhaHang.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class fixHoaDon : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.HoaDons", "PhiPhucvu", c => c.Decimal(precision: 18, scale: 2));
            AddColumn("dbo.HoaDons", "VAT", c => c.Decimal(precision: 18, scale: 2));
            AddColumn("dbo.HoaDons", "TongTien", c => c.Decimal(precision: 18, scale: 2));
            AlterColumn("dbo.HoaDons", "ThanhTienHD", c => c.Decimal(precision: 18, scale: 2));
            AlterColumn("dbo.MonDaGois", "ThanhTien", c => c.Decimal(precision: 18, scale: 2));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.MonDaGois", "ThanhTien", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AlterColumn("dbo.HoaDons", "ThanhTienHD", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            DropColumn("dbo.HoaDons", "TongTien");
            DropColumn("dbo.HoaDons", "VAT");
            DropColumn("dbo.HoaDons", "PhiPhucvu");
        }
    }
}
