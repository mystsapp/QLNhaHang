namespace QLNhaHang.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class fixPhiPhucVu : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.MonDaGois", "PhiPhucVu", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.MonDaGois", "PhiPhucVu", c => c.Decimal(precision: 18, scale: 2));
        }
    }
}
