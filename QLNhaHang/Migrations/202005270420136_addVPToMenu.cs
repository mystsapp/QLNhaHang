namespace QLNhaHang.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addVPToMenu : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ThucDons", "Name", c => c.String(nullable: false, maxLength: 100));
        }
        
        public override void Down()
        {
            DropColumn("dbo.ThucDons", "Name");
        }
    }
}
