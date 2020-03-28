using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace QLNhaHang.Data.Models
{
    public class ThongTinHD
    {
        public int Id { get; set; }

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