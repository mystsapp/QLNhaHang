using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Data.Models
{
    public class MonDaGoi
    {
        public int Id { get; set; }

        //[MaxLength(200), Column(TypeName = "varchar(200)")]
        //[DisplayName("Tên món")]
        //public string TenMon { get; set; }        

        [DisplayName("Số lượng")]
        public int SoLuong { get; set; }

        [DisplayName("Thành tiền")]
        public decimal ThanhTien { get; set; }

        [DisplayName("Bàn")]
        [MaxLength(20), Column(TypeName = "varchar(20)")]
        public string MaBan { get; set; }

        [ForeignKey("MaBan")]
        public virtual Ban Ban { get; set; }
        
        [DisplayName("Tên món")]
        public int ThucDonId { get; set; }

        [ForeignKey("ThucDonId")]
        public virtual ThucDon ThucDon { get; set; }
    }
}
