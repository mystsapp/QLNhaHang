namespace QLNhaHang.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addNoiLamViec : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.NhanViens", "NoiLamViec", c => c.String(maxLength: 50));
            AddColumn("dbo.LoaiThucDons", "NoiLamViec", c => c.String(maxLength: 50));
        }
        
        public override void Down()
        {
            DropColumn("dbo.LoaiThucDons", "NoiLamViec");
            DropColumn("dbo.NhanViens", "NoiLamViec");
        }
    }
}
