using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace QLNhaHang.Data.Models
{
    public class NhanVien
    {
        [Key]
        [MaxLength(20), Column(TypeName = "varchar")]
        [StringLength(20)]
        [DisplayName("Mã NV")]
        public string MaNV { get; set; }

        [DisplayName("Họ tên")]
        [Required(ErrorMessage = "Họ tên không được để trống")]
        [MaxLength(50), Column(TypeName = "nvarchar")]
        [StringLength(50)]
        public string HoTen { get; set; }

        //[DisplayName("Ngày sinh")]
        //[DataType(DataType.Date, ErrorMessage = "abcd")]
        //[DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
        public DateTime? NgaySinh { get; set; }

        [DisplayName("Giới tính")]
        [MaxLength(5), Column(TypeName = "nvarchar")]
        [StringLength(5)]
        public string GioiTinh { get; set; }
        
        [DisplayName("Địa chỉ")]
        [MaxLength(100), Column(TypeName = "nvarchar")]
        [StringLength(100)]
        public string DiaChi { get; set; }
        
        [DisplayName("Điện thoại")]
        [MaxLength(15), Column(TypeName = "varchar")]
        [StringLength(15)]
        public string DienThoai { get; set; }

        [DisplayName("Chức vụ")]
        [MaxLength(30), Column(TypeName = "nvarchar")]
        [StringLength(30)]
        public string ChucVu { get; set; }

        [Required(ErrorMessage = "Username không được bỏ trống.")]
        [MaxLength(50, ErrorMessage = "Không vượt qua 50 ký tự."), Column(TypeName = "varchar")]
        [StringLength(50)]
        //[Remote("UsersExists", "Users", ErrorMessage = "User đã tồn tại")]
        public string Username { get; set; }

        [DataType(DataType.Password)]
        [Required(ErrorMessage = "Password không được bỏ trống.")]
        [MaxLength(50, ErrorMessage = "Không vượt qua 50 ký tự.")]
        [StringLength(50), Column(TypeName = "varchar")]
        public string Password { get; set; }

        [DisplayName("Tình trạng")]
        public bool TrangThai { get; set; }
        
        [DisplayName("Người tạo")]
        [MaxLength(50), Column(TypeName = "nvarchar")]
        [StringLength(50)]
        public string NguoiTao { get; set; }
        public DateTime? NgayTao { get; set; }

        [DisplayName("Người cập nhật")]
        [MaxLength(50), Column(TypeName = "nvarchar")]
        [StringLength(50)]
        public string NguoiCapNhat { get; set; }
        public DateTime? NgayCapNhat { get; set; }

        //[DisplayName("Role")]
        //public int RoleId { get; set; }

        //[ForeignKey("RoleId")]
        //public virtual Role Role { get; set; }

        [MaxLength(15), Column(TypeName = "varchar")]
        [StringLength(15)]
        public string Role { get; set; }

        [MaxLength(50), Column(TypeName = "nvarchar")]
        [StringLength(50)]
        [DisplayName("VP làng")]
        public string PhongBan { get; set; }


        //[DisplayName("Cơ sở")]
        //public int VanPhongId { get; set; }

        //[ForeignKey("VanPhongId")]
        //public virtual VanPhong VanPhong { get; set; }


        [DisplayName("Khu vực")]
        [Required(ErrorMessage = "KV cơ sở không được để trống")]
        public int KhuVucId { get; set; }

        [ForeignKey("KhuVucId")]
        public virtual KhuVuc KhuVuc { get; set; }

    }
}
