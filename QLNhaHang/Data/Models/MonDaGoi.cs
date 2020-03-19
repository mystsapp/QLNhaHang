using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QLNhaHang.Data.Models
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
        [DisplayFormat(DataFormatString = "{0:N0}", ApplyFormatInEditMode = true)]
        public decimal? ThanhTien { get; set; }
        
        [DisplayName("Giá tiền")]
        [DisplayFormat(DataFormatString = "{0:N0}", ApplyFormatInEditMode = true)]
        public decimal? GiaTien { get; set; }

        [DisplayName("Phụ phí")]
        [DisplayFormat(DataFormatString = "{0:N0}", ApplyFormatInEditMode = true)]
        public decimal? PhuPhi { get; set; }
        
        [DisplayName("Phí phục vụ")]
        public bool PhiPhucVu { get; set; }

        [DisplayName("Bàn")]
        [MaxLength(20), Column(TypeName = "varchar")]
        [StringLength(20)]
        public string MaBan { get; set; }

        [ForeignKey("MaBan")]
        public virtual Ban Ban { get; set; }

        [DisplayName("Tên món")]
        public int ThucDonId { get; set; }

        [ForeignKey("ThucDonId")]
        public virtual ThucDon ThucDon { get; set; }

        [DisplayFormat(DataFormatString = "{0:N0}", ApplyFormatInEditMode = true)]
        public decimal? TongTien { get; set; }
    }
}