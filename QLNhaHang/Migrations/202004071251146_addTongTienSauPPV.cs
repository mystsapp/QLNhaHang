namespace QLNhaHang.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addTongTienSauPPV : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.HoaDons", "TongTienSauPPV", c => c.Decimal(precision: 18, scale: 2));
        }
        
        public override void Down()
        {
            DropColumn("dbo.HoaDons", "TongTienSauPPV");
        }
    }
}
