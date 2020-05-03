using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace QLNhaHang.Data.Models
{
    public class KhuVuc
    {
        public int Id { get; set; }

        [DisplayName("Tên KV")]
        [Required(ErrorMessage = "Tên không được để trống")]
        [MaxLength(50), Column(TypeName = "nvarchar")]
        [StringLength(50)]
        public string Name { get; set; }

        [DisplayName("Ghi chú")]
        [MaxLength(250), Column(TypeName = "nvarchar")]
        [StringLength(250)]
        public string GhiChu { get; set; }

        [DisplayName("Cơ sở")]
        public int VanPhongId { get; set; }

        [ForeignKey("VanPhongId")]
        public virtual VanPhong VanPhong { get; set; }
    }
}