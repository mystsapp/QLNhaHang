namespace QLNhaHang.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class fixThanhTienM : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.HoaDons", "ThanhTienM", c => c.Decimal(precision: 18, scale: 2));
            DropColumn("dbo.HoaDons", "TongTien");
        }
        
        public override void Down()
        {
            AddColumn("dbo.HoaDons", "TongTien", c => c.Decimal(precision: 18, scale: 2));
            DropColumn("dbo.HoaDons", "ThanhTienM");
        }
    }
}
