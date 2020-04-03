namespace QLNhaHang.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class fixVP : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.VanPhongs", "NgayTao", c => c.DateTime(nullable: false));
            AddColumn("dbo.VanPhongs", "NguoiTao", c => c.String(maxLength: 50));
            DropColumn("dbo.VanPhongs", "Role");
        }
        
        public override void Down()
        {
            AddColumn("dbo.VanPhongs", "Role", c => c.String(maxLength: 50));
            DropColumn("dbo.VanPhongs", "NguoiTao");
            DropColumn("dbo.VanPhongs", "NgayTao");
        }
    }
}
