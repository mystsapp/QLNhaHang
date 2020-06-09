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

        [DisplayName("Văn phòng")]
        public int VanPhongId { get; set; }

        [ForeignKey("VanPhongId")]
        public virtual VanPhong VanPhong { get; set; }

        //[Required]
        //[MaxLength(10), Column(TypeName = "varchar")]
        //[StringLength(10)]
        //[DisplayName("Mã VP")]
        //public string MaVP { get; set; }

        //[DisplayName("Khách hàng")]
        //[StringLength(50)]
        //[MaxLength(50), Column(TypeName = "varchar")]
        //public string MaKH { get; set; }

        //[ForeignKey("MaKH")]
        //public virtual KhachHang KhachHang { get; set; }

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

        public decimal? TyLePPV { get; set; }
        public decimal? PhiPhucvu { get; set; }
        public decimal? TongTienSauPPV { get; set; }
        public decimal? VAT { get; set; }

        [DisplayName("Tiền thuế GTGT")]
        public decimal? TienThueVAT { get; set; }

        [DisplayName("Thành tiền VAT")]
        public decimal? ThanhTienVAT { get; set; }

        [StringLength(200)]
        [MaxLength(200), Column(TypeName = "nvarchar")]
        public string SoTienBangChu { get; set; }

        [StringLength(20)]
        [MaxLength(20), Column(TypeName = "varchar")]
        public string NumberId { get; set; }

        /// <summary>
        /// for ThongTinHD
        /// </summary>
        [StringLength(20)]
        [MaxLength(20), Column(TypeName = "varchar")]
        public string MauSo { get; set; }

        [StringLength(20)]
        [MaxLength(20), Column(TypeName = "varchar")]
        public string KyHieu { get; set; }

        [StringLength(20)]
        [MaxLength(20), Column(TypeName = "varchar")]
        public string QuyenSo { get; set; }

        [StringLength(20)]
        [MaxLength(20), Column(TypeName = "varchar")]
        public string So { get; set; }

        [StringLength(20)]
        [MaxLength(20), Column(TypeName = "varchar")]
        public string SoThuTu { get; set; }

        ///////////// for KhachHang
        [StringLength(100)]
        [MaxLength(100), Column(TypeName = "nvarchar")]
        [DisplayName("Tên KH")]
        //[Remote("IsStringNameAvailable", "KhachHangs", ErrorMessage = "Tên KH đã tồn tại")]
        public string TenKH { get; set; }

        [MaxLength(15), Column(TypeName = "varchar")]
        [StringLength(15)]
        public string Phone { get; set; }

        [MaxLength(250), Column(TypeName = "nvarchar")]
        [StringLength(250)]
        [DisplayName("Địa chỉ")]
        public string DiaChi { get; set; }

        [MaxLength(100), Column(TypeName = "nvarchar")]
        [StringLength(100)]
        [DisplayName("Tên đơn vị")]
        public string TenDonVi { get; set; }

        [MaxLength(20), Column(TypeName = "varchar")]
        [StringLength(20)]
        [DisplayName("Mã số thuế")]
        public string MaSoThue { get; set; }

        public bool? DaIn { get; set; }
        public DateTime? NgayIn { get; set; }

        ///// thong tin them
        [MaxLength(300), Column(TypeName = "nvarchar")]
        [StringLength(300)]
        [DisplayName("Nội dung")]
        public string NoiDung { get; set; }

        [DisplayName("Số tiền")]
        public decimal? SoTien { get; set; }

        public bool? Xoa { get; set; }

        [Column(TypeName = "nvarchar")]
        public string LogFile { get; set; }
    }
}