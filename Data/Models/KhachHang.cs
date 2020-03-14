using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Data.Models
{
    public class KhachHang
    {
        [Key]
        [DisplayName("Mã KH")]
        [MaxLength(50), Column(TypeName = "nvarchar(50)")]
        public string MaKH { get; set; }

        [MaxLength(100), Column(TypeName = "nvarchar(100)")]
        [DisplayName("Tên KH")]
        public string TenKH { get; set; }

        [MaxLength(10), Column(TypeName = "nvarchar(10)")]
        [DisplayName("Giới tính")]
        public string GioiTinh { get; set; }
        public DateTime NgayTao { get; set; }

        [MaxLength(50), Column(TypeName = "nvarchar(50)")]
        [DisplayName("Người tạo")]
        public string NguoiTao { get; set; }

        [MaxLength(50), Column(TypeName = "varchar(50)")]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [MaxLength(15), Column(TypeName = "varchar(15)")]
        public string Phone { get; set; }

        [MaxLength(100), Column(TypeName = "nvarchar(100)")]
        [DisplayName("Tên đơn vị")]
        public string TenDonVi { get; set; }

        [MaxLength(20), Column(TypeName = "varchar(20)")]
        [DisplayName("Mã số thuế")]
        public string MaSoThue { get; set; }
    }
}
