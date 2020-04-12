namespace QLNhaHang.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addMaSoBan : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Bans", "MaSo", c => c.String(nullable: false, maxLength: 10, unicode: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Bans", "MaSo");
        }
    }
}
