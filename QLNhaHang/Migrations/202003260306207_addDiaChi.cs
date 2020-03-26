namespace QLNhaHang.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addDiaChi : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.KhachHangs", "DiaChi", c => c.String(maxLength: 250));
        }
        
        public override void Down()
        {
            DropColumn("dbo.KhachHangs", "DiaChi");
        }
    }
}
