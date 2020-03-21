namespace QLNhaHang.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class fixNgayTaoNullable : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.HoaDons", "NgayTao", c => c.DateTime());
            AlterColumn("dbo.HoaDons", "NgayGiao", c => c.DateTime());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.HoaDons", "NgayGiao", c => c.DateTime(nullable: false));
            AlterColumn("dbo.HoaDons", "NgayTao", c => c.DateTime(nullable: false));
        }
    }
}
