namespace QLNhaHang.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addTyLePPV : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.HoaDons", "TyLePPV", c => c.Decimal(precision: 18, scale: 2));
        }
        
        public override void Down()
        {
            DropColumn("dbo.HoaDons", "TyLePPV");
        }
    }
}
