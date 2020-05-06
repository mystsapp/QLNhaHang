namespace QLNhaHang.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class fixTenLoai : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.LoaiThucDons", "TenLoai", c => c.String(nullable: false, maxLength: 100));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.LoaiThucDons", "TenLoai", c => c.String(maxLength: 100));
        }
    }
}
