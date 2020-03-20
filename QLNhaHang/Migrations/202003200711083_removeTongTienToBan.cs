namespace QLNhaHang.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class removeTongTienToBan : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.Bans", "TongTien");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Bans", "TongTien", c => c.Decimal(precision: 18, scale: 2));
        }
    }
}
