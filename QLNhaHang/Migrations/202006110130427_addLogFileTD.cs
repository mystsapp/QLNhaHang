namespace QLNhaHang.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addLogFileTD : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ThucDons", "Xoa", c => c.Boolean());
            AddColumn("dbo.ThucDons", "LogFile", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.ThucDons", "LogFile");
            DropColumn("dbo.ThucDons", "Xoa");
        }
    }
}
