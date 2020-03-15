using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Data.Models
{
    public class HoaDon
    {
        [Key]
        [DisplayName("Mã HD")]
        [MaxLength(20), Column(TypeName = "varchar(20)")]
        public string MaHD { get; set; }

        [DisplayName("Nhân viên")]
        [MaxLength(20), Column(TypeName = "varchar(20)")]
        public string MaNV { get; set; }

        [ForeignKey("MaNV")]
        public virtual NhanVien NhanVien { get; set; }
        
        [DisplayName("Khách hàng")]
        [MaxLength(50), Column(TypeName = "varchar(50)")]
        public string MaKH { get; set; }

        [ForeignKey("MaKH")]
        public virtual KhachHang KhachHang { get; set; }
        
        [DisplayName("Bàn")]
        [MaxLength(20), Column(TypeName = "varchar(20)")]
        public string MaBan { get; set; }

        [ForeignKey("MaBan")]
        public virtual Ban Ban { get; set; }

        [MaxLength(50), Column(TypeName = "nvarchar(50)")]
        [DisplayName("HT Thanh Toán")]
        public string HTThanhToan { get; set; }

        public DateTime NgayTao { get; set; }
        public DateTime NgayGiao { get; set; }

        [MaxLength(300), Column(TypeName = "nvarchar(300)")]
        [DisplayName("Ghi chú")]
        public string GhiChu { get; set; }

        [DisplayName("Thành tiền HD")]
        public decimal ThanhTienHD { get; set; }
    }
}
