namespace QLNhaHang.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addForRole : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Roles", "NgayTao", c => c.DateTime(nullable: false));
            AddColumn("dbo.Roles", "NguoiTao", c => c.String(maxLength: 50));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Roles", "NguoiTao");
            DropColumn("dbo.Roles", "NgayTao");
        }
    }
}
