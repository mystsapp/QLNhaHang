using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace QLNhaHang.Data.Models
{
    public class Ban
    {
        [Key]
        [DisplayName("Mã bàn")]
        [StringLength(20)]
        [Column(TypeName = "varchar")]
        public string MaBan { get; set; }

        [StringLength(50)]
        [MaxLength(50), Column(TypeName = "nvarchar")]
        [DisplayName("Tên bàn")]
        [Required]
        public string TenBan { get; set; }

        [DisplayName("Số lượng khách")]
        public int SoLuongKhach { get; set; }
        public int CheckBan { get; set; }

        [StringLength(250)]
        [MaxLength(250), Column(TypeName = "nvarchar")]
        [DisplayName("Ghi chú")]
        public string GhiChu { get; set; }

        public bool Flag { get; set; }

        //[DisplayName("Văn phòng")]
        //public int VanPhongId { get; set; }

        //[ForeignKey("VanPhongId")]
        //public virtual VanPhong VanPhong { get; set; }

        [Required(ErrorMessage = "Tên cơ sở không được để trống")]
        [MaxLength(100), Column(TypeName = "nvarchar")]
        [StringLength(100)]
        [DisplayName("Cơ sở")]
        public string TenVP { get; set; }

        [DisplayName("Ngày tạo")]
        public DateTime NgayTao { get; set; }

        [DisplayName("Người tạo")]
        [MaxLength(50), Column(TypeName = "nvarchar")]
        [StringLength(50)]
        public string NguoiTao { get; set; }

        [Required]
        [MaxLength(10), Column(TypeName = "varchar")]
        [StringLength(10)]
        [DisplayName("Mã Số")]
        public string MaSo { get; set; }

        [DisplayName("Khu vực")]
        [Required(ErrorMessage = "KV cơ sở không được để trống")]
        public int KhuVucId { get; set; }

        [ForeignKey("KhuVucId")]
        public virtual KhuVuc KhuVuc { get; set; }

        public bool? Xoa { get; set; }
        [Column(TypeName = "nvarchar(MAX)")]
        public string LogFile { get; set; }

    }
}
