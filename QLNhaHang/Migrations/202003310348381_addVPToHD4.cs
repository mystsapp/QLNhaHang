namespace QLNhaHang.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addVPToHD4 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.HoaDons", "MaVP", c => c.String(nullable: false, maxLength: 10, unicode: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.HoaDons", "MaVP");
        }
    }
}
