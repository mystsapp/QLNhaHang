namespace QLNhaHang.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addLogfileCol : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Bans", "LogFile", c => c.String());
            AddColumn("dbo.NhanViens", "LogFile", c => c.String());
            AlterColumn("dbo.HoaDons", "LogFile", c => c.String());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.HoaDons", "LogFile", c => c.String(maxLength: 4000));
            DropColumn("dbo.NhanViens", "LogFile");
            DropColumn("dbo.Bans", "LogFile");
        }
    }
}
