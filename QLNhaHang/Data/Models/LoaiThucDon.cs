using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace QLNhaHang.Data.Models
{
    public class LoaiThucDon
    {
        [Key]
        public int Id { get; set; }

        [MaxLength(100), Column(TypeName = "nvarchar")]
        [StringLength(100)]
        [DisplayName("Tên loại")]
        public string TenLoai { get; set; }

        [DisplayName("Phụ phí")]
        public decimal? PhuPhi { get; set; }

        [MaxLength(200), Column(TypeName = "nvarchar")]
        [StringLength(200)]
        [DisplayName("Mô tả")]
        public string MoTa { get; set; }

        [MaxLength(200), Column(TypeName = "nvarchar")]
        [StringLength(200)]
        [DisplayName("Ghi chú")]
        public string GhiChu { get; set; }

        [DisplayName("Ngày tạo")]
        public DateTime NgayTao { get; set; }

        [DisplayName("Người tạo")]
        [MaxLength(50), Column(TypeName = "nvarchar")]
        [StringLength(50)]
        public string NguoiTao { get; set; }
    }
}
