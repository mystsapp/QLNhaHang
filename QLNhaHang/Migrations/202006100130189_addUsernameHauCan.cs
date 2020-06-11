namespace QLNhaHang.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addUsernameHauCan : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Beps", "Username", c => c.String(maxLength: 50, unicode: false));
            AddColumn("dbo.PhaChes", "Username", c => c.String(maxLength: 50, unicode: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.PhaChes", "Username");
            DropColumn("dbo.Beps", "Username");
        }
    }
}
