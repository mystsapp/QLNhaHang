namespace QLNhaHang.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addGiaMonDaChon : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.MonDaGois", "GiaTien", c => c.Decimal(precision: 18, scale: 2));
        }
        
        public override void Down()
        {
            DropColumn("dbo.MonDaGois", "GiaTien");
        }
    }
}
