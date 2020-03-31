namespace QLNhaHang.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class fixNullHD : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.HoaDons", "QuyenSo", c => c.Int());
            AlterColumn("dbo.HoaDons", "So", c => c.Long());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.HoaDons", "So", c => c.Long(nullable: false));
            AlterColumn("dbo.HoaDons", "QuyenSo", c => c.Int(nullable: false));
        }
    }
}
