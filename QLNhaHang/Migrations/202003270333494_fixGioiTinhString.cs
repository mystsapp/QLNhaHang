namespace QLNhaHang.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class fixGioiTinhString : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.KhachHangs", "GioiTinh", c => c.String(maxLength: 10));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.KhachHangs", "GioiTinh", c => c.Boolean());
        }
    }
}
