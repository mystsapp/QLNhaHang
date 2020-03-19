namespace QLNhaHang.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addBanFlag : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Bans", "Flag", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Bans", "Flag");
        }
    }
}
