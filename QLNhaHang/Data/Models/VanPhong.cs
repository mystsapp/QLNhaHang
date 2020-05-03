using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace QLNhaHang.Data.Models
{
    public class VanPhong
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(10), Column(TypeName = "varchar")]
        [StringLength(10)]
        [DisplayName("Mã")]
        public string MaVP { get; set; }
        [Required]
        [MaxLength(100), Column(TypeName = "nvarchar")]
        [StringLength(100)]
        [DisplayName("Cơ sở")]
        public string Name { get; set; }

        [MaxLength(250), Column(TypeName = "nvarchar")]
        [StringLength(250)]
        [DisplayName("Địa chỉ")]
        public string DiaChi { get; set; }

        [MaxLength(15), Column(TypeName = "varchar")]
        [StringLength(15)]
        [DisplayName("Điện thoại")]
        public string DienThoai { get; set; }

        //[DisplayName("Chi Nhánh")]
        //public int ChiNhanhId { get; set; }

        //[ForeignKey("ChiNhanhId")]
        //public virtual ChiNhanh ChiNhanh { get; set; }

        [MaxLength(15), Column(TypeName = "varchar")]
        [StringLength(15)]
        public string Role { get; set; }

        [DisplayName("Ngày tạo")]
        public DateTime NgayTao { get; set; }

        [DisplayName("Người tạo")]
        [MaxLength(50), Column(TypeName = "nvarchar")]
        [StringLength(50)]
        public string NguoiTao { get; set; }

        [MaxLength(20), Column(TypeName = "varchar")]
        [StringLength(20)]
        [DisplayName("Mã số thuế")]
        public string MaSoThue { get; set; }
    }
}
