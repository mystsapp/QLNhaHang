namespace QLNhaHang.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class roleNvarchar : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.VanPhongs", "Role", c => c.String(maxLength: 15));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.VanPhongs", "Role", c => c.String(maxLength: 15, unicode: false));
        }
    }
}
