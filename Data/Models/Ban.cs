using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Data.Models
{
    public class Ban
    {
        [Key]
        [MaxLength(20), Column(TypeName = "varchar(20)")]
        [DisplayName("Mã bàn")]
        public string MaBan { get; set; }

        [MaxLength(50), Column(TypeName = "nvarchar(50)")]
        [DisplayName("Tên bàn")]
        [Required]
        public string TenBan { get; set; }

        [DisplayName("Số lượng khách")]
        public int SoLuongKhach { get; set; }
        public int CheckBan { get; set; }

        [MaxLength(250), Column(TypeName = "nvarchar(250)")]
        [DisplayName("Ghi chú")]
        public string GhiChu { get; set; }
    }
}
