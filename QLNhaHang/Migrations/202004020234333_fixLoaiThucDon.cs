namespace QLNhaHang.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class fixLoaiThucDon : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.LoaiThucDons", "NgayTao", c => c.DateTime(nullable: false));
            AddColumn("dbo.LoaiThucDons", "NguoiTao", c => c.String(maxLength: 50));
        }
        
        public override void Down()
        {
            DropColumn("dbo.LoaiThucDons", "NguoiTao");
            DropColumn("dbo.LoaiThucDons", "NgayTao");
        }
    }
}
