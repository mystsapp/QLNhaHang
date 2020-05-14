namespace QLNhaHang.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class roleNvarcharNV : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.NhanViens", "Role", c => c.String(maxLength: 15));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.NhanViens", "Role", c => c.String(maxLength: 15, unicode: false));
        }
    }
}
