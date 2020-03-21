namespace QLNhaHang.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class fixDonGiaCTHD : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.ChiTietHDs", "DonGia", c => c.Decimal(precision: 18, scale: 2));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.ChiTietHDs", "DonGia", c => c.Decimal(nullable: false, precision: 18, scale: 2));
        }
    }
}
