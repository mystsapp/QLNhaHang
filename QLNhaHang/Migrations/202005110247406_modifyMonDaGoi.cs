namespace QLNhaHang.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class modifyMonDaGoi : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.MonDaGois", "LanGui", c => c.Int(nullable: false));
            AddColumn("dbo.MonDaGois", "DaGui", c => c.Boolean(nullable: false));
            AddColumn("dbo.MonDaGois", "DaLam", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.MonDaGois", "DaLam");
            DropColumn("dbo.MonDaGois", "DaGui");
            DropColumn("dbo.MonDaGois", "LanGui");
        }
    }
}
