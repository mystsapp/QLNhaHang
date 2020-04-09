namespace QLNhaHang.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class HDNumToWord : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.HoaDons", "SoTienBangChu", c => c.String(maxLength: 200));
            DropColumn("dbo.HoaDons", "ThanhTienTay");
        }
        
        public override void Down()
        {
            AddColumn("dbo.HoaDons", "ThanhTienTay", c => c.Decimal(precision: 18, scale: 2));
            DropColumn("dbo.HoaDons", "SoTienBangChu");
        }
    }
}
