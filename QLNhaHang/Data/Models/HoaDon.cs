using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QLNhaHang.Data.Models
{
    public class HoaDon
    {
        [Key]
        [DisplayName("Mã HD")]
        [StringLength(20)]
        [MaxLength(20), Column(TypeName = "varchar")]
        public string MaHD { get; set; }

        [DisplayName("Nhân viên")]
        [StringLength(20)]
        [MaxLength(20), Column(TypeName = "varchar")]
        public string MaNV { get; set; }

        [ForeignKey("MaNV")]
        public virtual NhanVien NhanVien { get; set; }

        [DisplayName("Khách hàng")]
        [StringLength(50)]
        [MaxLength(50), Column(TypeName = "varchar")]
        public string MaKH { get; set; }

        [ForeignKey("MaKH")]
        public virtual KhachHang KhachHang { get; set; }

        [DisplayName("Bàn")]
        [StringLength(20)]
        [MaxLength(20), Column(TypeName = "varchar")]
        public string MaBan { get; set; }

        [ForeignKey("MaBan")]
        public virtual Ban Ban { get; set; }

        [MaxLength(50), Column(TypeName = "nvarchar")]
        [StringLength(50)]
        [DisplayName("HT Thanh Toán")]
        public string HTThanhToan { get; set; }

        public DateTime? NgayTao { get; set; }
        public DateTime? NgayGiao { get; set; }


        [MaxLength(300), Column(TypeName = "nvarchar")]
        [StringLength(300)]
        [DisplayName("Ghi chú")]
        public string GhiChu { get; set; }

        [DisplayName("Thành tiền HD")]
        public decimal? ThanhTienHD { get; set; }
        public decimal? PhiPhucvu { get; set; }
        public decimal? VAT { get; set; }
        public decimal? TongTien { get; set; }

        [StringLength(20)]
        [MaxLength(20), Column(TypeName = "varchar")]
        public string NumberId { get; set; }

        [StringLength(20)]
        [MaxLength(20), Column(TypeName = "varchar")]
        public string MauSo { get; set; }
        [StringLength(20)]
        [MaxLength(20), Column(TypeName = "varchar")]
        public string KyHieu { get; set; }
        public int QuyenSo { get; set; }
        public long So { get; set; }
        [StringLength(20)]
        [MaxLength(20), Column(TypeName = "varchar")]
        public string SoThuTu { get; set; }

    }
}