namespace QLNhaHang.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class fixMauSoKyHieuHDToString : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.HoaDons", "QuyenSo", c => c.String(maxLength: 20, unicode: false));
            AlterColumn("dbo.HoaDons", "So", c => c.String(maxLength: 20, unicode: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.HoaDons", "So", c => c.Long());
            AlterColumn("dbo.HoaDons", "QuyenSo", c => c.Int());
        }
    }
}
