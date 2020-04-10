namespace QLNhaHang.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class fixRequiredHD : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.ThongTinHDs", "SoThuTu", c => c.String(nullable: false, maxLength: 20, unicode: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.ThongTinHDs", "SoThuTu", c => c.String(maxLength: 20, unicode: false));
        }
    }
}
