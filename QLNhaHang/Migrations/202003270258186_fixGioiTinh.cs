namespace QLNhaHang.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class fixGioiTinh : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.KhachHangs", "GioiTinh", c => c.Boolean());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.KhachHangs", "GioiTinh", c => c.String(maxLength: 10));
        }
    }
}
