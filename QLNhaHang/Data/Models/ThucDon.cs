using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace QLNhaHang.Data.Models
{
    public class ThucDon
    {
        public int Id { get; set; }

        [MaxLength(200), Column(TypeName = "nvarchar")]
        [StringLength(200)]
        [DisplayName("Tên món")]
        public string TenMon { get; set; }

        [DisplayName("Giá tiền")]
        public decimal GiaTien { get; set; }

        [DisplayName("Đơn vị tính")]
        [MaxLength(20), Column(TypeName = "nvarchar")]
        [StringLength(20)]
        public string DonViTinh { get; set; }

        [DisplayName("Loại thực đơn")]
        public int MaLoaiId { get; set; }

        [ForeignKey("MaLoaiId")]
        public virtual LoaiThucDon LoaiThucDon { get; set; }

        [DisplayName("Ghi chú")]
        [MaxLength(200), Column(TypeName = "nvarchar")]
        [StringLength(200)]
        public string GhiChu { get; set; }
        
        [DisplayName("Ngày tạo")]
        public DateTime NgayTao { get; set; }
        
        [DisplayName("Người tạo")]
        [MaxLength(50), Column(TypeName = "nvarchar")]
        [StringLength(50)]
        public string NguoiTao { get; set; }

        [Required(ErrorMessage = "Tên cơ sở không được để trống.")]
        [MaxLength(100), Column(TypeName = "nvarchar")]
        [StringLength(100)]
        [DisplayName("Cơ sở")]
        public string VanPhong { get; set; }
    }
}
