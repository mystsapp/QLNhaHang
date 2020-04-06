namespace QLNhaHang.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addVATMoney : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.HoaDons", "ThanhTienVAT", c => c.Decimal(precision: 18, scale: 2));
            AddColumn("dbo.HoaDons", "ThanhTienTay", c => c.Decimal(precision: 18, scale: 2));
        }
        
        public override void Down()
        {
            DropColumn("dbo.HoaDons", "ThanhTienTay");
            DropColumn("dbo.HoaDons", "ThanhTienVAT");
        }
    }
}
