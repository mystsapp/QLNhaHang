﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace QLNhaHang.Data.Models
{
    public class KhachHang
    {
        [Key]
        [DisplayName("Mã KH")]
        [StringLength(50)]
        [MaxLength(50), Column(TypeName = "varchar")]
        public string MaKH { get; set; }

        [StringLength(100)]
        [MaxLength(100), Column(TypeName = "nvarchar")]
        [DisplayName("Tên KH")]
        public string TenKH { get; set; }

        [MaxLength(10), Column(TypeName = "nvarchar")]
        [StringLength(10)]
        [DisplayName("Giới tính")]
        public string GioiTinh { get; set; }
        public DateTime NgayTao { get; set; }

        [MaxLength(50), Column(TypeName = "nvarchar")]
        [StringLength(50)]
        [DisplayName("Người tạo")]
        public string NguoiTao { get; set; }

        [MaxLength(50), Column(TypeName = "varchar")]
        [StringLength(50)]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [MaxLength(15), Column(TypeName = "varchar")]
        [StringLength(15)]
        public string Phone { get; set; }

        [MaxLength(100), Column(TypeName = "nvarchar")]
        [StringLength(100)]
        [DisplayName("Tên đơn vị")]
        public string TenDonVi { get; set; }

        [MaxLength(20), Column(TypeName = "varchar")]
        [StringLength(20)]
        [DisplayName("Mã số thuế")]
        public string MaSoThue { get; set; }
    }
}
