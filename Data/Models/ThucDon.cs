﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Data.Models
{
    public class ThucDon
    {
        public int Id { get; set; }

        [MaxLength(200), Column(TypeName = "varchar(200)")]
        [DisplayName("Tên món")]
        public string TenMon { get; set; }

        [DisplayName("Giá tiền")]
        public decimal GiaTien { get; set; }

        [DisplayName("Phụ phí")]
        public decimal PhuPhi { get; set; }

        [DisplayName("Đơn vị tính")]
        [MaxLength(20), Column(TypeName = "varchar(20)")]
        public string DonViTinh { get; set; }

        [DisplayName("Loại thực đơn")]
        public int MaLoaiId { get; set; }

        [ForeignKey("MaLoaiId")]
        public virtual LoaiThucDon LoaiThucDon { get; set; }

        [DisplayName("Ghi chú")]
        [MaxLength(200), Column(TypeName = "varchar(200)")]
        public string GhiChu { get; set; }
    }
}
