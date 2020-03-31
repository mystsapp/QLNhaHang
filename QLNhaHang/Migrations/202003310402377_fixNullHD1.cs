namespace QLNhaHang.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class fixNullHD1 : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.HoaDons", "MaVP");
        }
        
        public override void Down()
        {
            AddColumn("dbo.HoaDons", "MaVP", c => c.String(nullable: false, maxLength: 10, unicode: false));
        }
    }
}
