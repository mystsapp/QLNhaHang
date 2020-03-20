namespace QLNhaHang.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class moveTongTienToBan : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Bans", "TongTien", c => c.Decimal(precision: 18, scale: 2));
            DropColumn("dbo.MonDaGois", "TongTien");
        }
        
        public override void Down()
        {
            AddColumn("dbo.MonDaGois", "TongTien", c => c.Decimal(precision: 18, scale: 2));
            DropColumn("dbo.Bans", "TongTien");
        }
    }
}
