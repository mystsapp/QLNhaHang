namespace QLNhaHang.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class fixMauSoKyHieu : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.VanPhongs", "QuyenSo", c => c.String(maxLength: 20, unicode: false));
            AlterColumn("dbo.VanPhongs", "So", c => c.String(maxLength: 20, unicode: false));
            AlterColumn("dbo.VanPhongs", "SoThuTu", c => c.String(maxLength: 20, unicode: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.VanPhongs", "SoThuTu", c => c.String(nullable: false, maxLength: 20, unicode: false));
            AlterColumn("dbo.VanPhongs", "So", c => c.Long(nullable: false));
            AlterColumn("dbo.VanPhongs", "QuyenSo", c => c.Int(nullable: false));
        }
    }
}
