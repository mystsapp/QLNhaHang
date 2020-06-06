namespace QLNhaHang.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addVPNameToMon : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.MonDaGois", "VanPhong", c => c.String(maxLength: 100));
        }
        
        public override void Down()
        {
            DropColumn("dbo.MonDaGois", "VanPhong");
        }
    }
}
