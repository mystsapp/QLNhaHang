namespace QLNhaHang.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class fixHDCol1 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.HoaDons", "QuyenSo", c => c.Int(nullable: false));
            AddColumn("dbo.HoaDons", "So", c => c.Long(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.HoaDons", "So");
            DropColumn("dbo.HoaDons", "QuyenSo");
        }
    }
}
