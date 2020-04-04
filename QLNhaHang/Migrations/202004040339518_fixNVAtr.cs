namespace QLNhaHang.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class fixNVAtr : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.NhanViens", "HoTen", c => c.String(nullable: false, maxLength: 50));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.NhanViens", "HoTen", c => c.String(maxLength: 50));
        }
    }
}
