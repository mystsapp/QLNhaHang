using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Data.Models
{
    public class NhanVien
    {
        [Key]
        [MaxLength(20), Column(TypeName = "varchar(20)")]
        [DisplayName("Mã NV")]
        public string MaNV { get; set; }

        [DisplayName("Họ tên")]
        [MaxLength(50), Column(TypeName = "nvarchar(50)")]
        public string HoTen { get; set; }

        [DisplayName("Ngày sinh")]
        public DateTime NgaySinh { get; set; }

        [DisplayName("Giới tính")]
        [MaxLength(5), Column(TypeName = "nvarchar(5)")]
        public string GioiTinh { get; set; }
        
        [DisplayName("Địa chỉ")]
        [MaxLength(100), Column(TypeName = "nvarchar(100)")]
        public string DiaChi { get; set; }
        
        [DisplayName("Điện thoại")]
        [MaxLength(15), Column(TypeName = "varchar(15)")]
        public string DienThoai { get; set; }

        [DisplayName("Chức vụ")]
        [MaxLength(30), Column(TypeName = "nvarchar(30)")]
        public string ChucVu { get; set; }

        [Required(ErrorMessage = "Username không được bỏ trống.")]
        [MaxLength(50, ErrorMessage = "Không vượt qua 50 ký tự.")]
        //[Remote("UsersExists", "Users", ErrorMessage = "User đã tồn tại")]
        public string Username { get; set; }

        [DataType(DataType.Password)]
        [Required(ErrorMessage = "Password không được bỏ trống.")]
        [MaxLength(50, ErrorMessage = "Không vượt qua 50 ký tự.")]
        public string Password { get; set; }

        [DisplayName("Trạng thái")]
        public bool TrangThai { get; set; }
        
        [DisplayName("Người tạo")]
        [MaxLength(50), Column(TypeName = "nvarchar(50)")]
        public string NguoiTao { get; set; }
        public DateTime? Ngaytao { get; set; }

        [DisplayName("Người cập nhật")]
        [MaxLength(50), Column(TypeName = "nvarchar(50)")]
        public string NguoiCapNhat { get; set; }
        public DateTime? Ngaycapnhat { get; set; }

        [DisplayName("Role")]
        public int RoleId { get; set; }

        [ForeignKey("RoleId")]
        public virtual Role Role { get; set; }

        [MaxLength(50), Column(TypeName = "nvarchar(50)")]
        [DisplayName("Phòng ban")]
        public string PhongBan { get; set; }

        //[MaxLength(50), Column(TypeName = "nvarchar(50)")]
        //[DisplayName("Văn phòng")]
        //public string VanPhong { get; set; }

        [DisplayName("Văn phòng")]
        public int VanPhongId { get; set; }

        [ForeignKey("VanPhongId")]
        public virtual VanPhong VanPhong { get; set; }

    }
}
