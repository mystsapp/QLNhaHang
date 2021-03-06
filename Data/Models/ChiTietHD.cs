﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Data.Models
{
    public class ChiTietHD
    {
        [Key]
        public int MaCTHD { get; set; }

        [DisplayName("Mã HD")]
        [MaxLength(20), Column(TypeName = "varchar(20)")]
        public int MaHD { get; set; }

        [ForeignKey("MaHD")]
        public virtual HoaDon HoaDon { get; set; }
        
        [DisplayName("Mã HD")]
        public int MaThucDon { get; set; }

        [ForeignKey("MaThucDon")]
        public virtual ThucDon ThucDon { get; set; }

        [DisplayName("Đơn giá")]
        public decimal DonGia { get; set; }

        [DisplayName("Số lượng")]
        public int SoLuong { get; set; }
    }
}
