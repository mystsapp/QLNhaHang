namespace QLNhaHang.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class fixVPToMenu : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ThucDons", "VanPhong", c => c.String(nullable: false, maxLength: 100));
            DropColumn("dbo.ThucDons", "Name");
        }
        
        public override void Down()
        {
            AddColumn("dbo.ThucDons", "Name", c => c.String(nullable: false, maxLength: 100));
            DropColumn("dbo.ThucDons", "VanPhong");
        }
    }
}
