namespace QLNhaHang.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addThongTinHD : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ThongTinHDs",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        MauSo = c.String(maxLength: 20, unicode: false),
                        KyHieu = c.String(maxLength: 20, unicode: false),
                        QuyenSo = c.Int(nullable: false),
                        So = c.Long(nullable: false),
                        SoThuTu = c.String(maxLength: 20, unicode: false),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.ThongTinHDs");
        }
    }
}
