namespace QLNhaHang.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addLogFile : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Bans", "Xoa", c => c.Boolean());
            AddColumn("dbo.HoaDons", "Xoa", c => c.Boolean());
            AddColumn("dbo.HoaDons", "LogFile", c => c.String(maxLength: 4000));
            AddColumn("dbo.NhanViens", "Xoa", c => c.Boolean());
        }
        
        public override void Down()
        {
            DropColumn("dbo.NhanViens", "Xoa");
            DropColumn("dbo.HoaDons", "LogFile");
            DropColumn("dbo.HoaDons", "Xoa");
            DropColumn("dbo.Bans", "Xoa");
        }
    }
}
