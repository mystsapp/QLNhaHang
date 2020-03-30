namespace QLNhaHang.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addCheckIn : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.HoaDons", "DaIn", c => c.Boolean());
            AddColumn("dbo.HoaDons", "NgayIn", c => c.DateTime());
        }
        
        public override void Down()
        {
            DropColumn("dbo.HoaDons", "NgayIn");
            DropColumn("dbo.HoaDons", "DaIn");
        }
    }
}
