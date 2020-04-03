namespace QLNhaHang.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class fixVPRole : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.VanPhongs", "Role", c => c.String(maxLength: 15, unicode: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.VanPhongs", "Role");
        }
    }
}
