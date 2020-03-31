namespace QLNhaHang.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class fixThucDobn : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ThucDons", "NgayTao", c => c.DateTime(nullable: false));
            AddColumn("dbo.ThucDons", "NguoiTao", c => c.String(maxLength: 50));
            AlterColumn("dbo.ThucDons", "GhiChu", c => c.String(maxLength: 200));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.ThucDons", "GhiChu", c => c.String(maxLength: 200, unicode: false));
            DropColumn("dbo.ThucDons", "NguoiTao");
            DropColumn("dbo.ThucDons", "NgayTao");
        }
    }
}
