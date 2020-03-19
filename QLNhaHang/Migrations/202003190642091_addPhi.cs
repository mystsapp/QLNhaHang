namespace QLNhaHang.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addPhi : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.MonDaGois", "PhuPhi", c => c.Decimal(precision: 18, scale: 2));
            AddColumn("dbo.MonDaGois", "PhiPhucVu", c => c.Decimal(precision: 18, scale: 2));
        }
        
        public override void Down()
        {
            DropColumn("dbo.MonDaGois", "PhiPhucVu");
            DropColumn("dbo.MonDaGois", "PhuPhi");
        }
    }
}
