namespace QLNhaHang.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class fixNgaySinhNullable : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.NhanViens", "NgaySinh", c => c.DateTime());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.NhanViens", "NgaySinh", c => c.DateTime(nullable: false));
        }
    }
}
