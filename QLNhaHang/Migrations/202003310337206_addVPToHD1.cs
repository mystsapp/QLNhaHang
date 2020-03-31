namespace QLNhaHang.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addVPToHD1 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.HoaDons", "VanPhongId", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.HoaDons", "VanPhongId");
        }
    }
}
