namespace QLNhaHang.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addThonTinthem : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.HoaDons", "NoiDung", c => c.String(maxLength: 300));
            AddColumn("dbo.HoaDons", "SoTien", c => c.Decimal(precision: 18, scale: 2));
            DropColumn("dbo.HoaDons", "ThanhTienM");
        }
        
        public override void Down()
        {
            AddColumn("dbo.HoaDons", "ThanhTienM", c => c.Decimal(precision: 18, scale: 2));
            DropColumn("dbo.HoaDons", "SoTien");
            DropColumn("dbo.HoaDons", "NoiDung");
        }
    }
}
