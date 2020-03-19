namespace QLNhaHang.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class fixMenu : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.ThucDons", "TenMon", c => c.String(maxLength: 200));
            AlterColumn("dbo.ThucDons", "PhuPhi", c => c.Decimal(precision: 18, scale: 2));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.ThucDons", "PhuPhi", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AlterColumn("dbo.ThucDons", "TenMon", c => c.String(maxLength: 200, unicode: false));
        }
    }
}
