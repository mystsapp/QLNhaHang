namespace QLNhaHang.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addNumberId : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.HoaDons", "NumberId", c => c.String(maxLength: 20, unicode: false));
            AddColumn("dbo.MonDaGois", "NumberId", c => c.String(maxLength: 20, unicode: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.MonDaGois", "NumberId");
            DropColumn("dbo.HoaDons", "NumberId");
        }
    }
}
