namespace QLNhaHang.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class delNumberIdinMDG : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.MonDaGois", "NumberId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.MonDaGois", "NumberId", c => c.String(maxLength: 20, unicode: false));
        }
    }
}
