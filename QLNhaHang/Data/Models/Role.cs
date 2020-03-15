using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace QLNhaHang.Data.Models
{
    public class Role
    {
        public int Id { get; set; }

        [DisplayName("Tên Role")]
        [MaxLength(50), Column(TypeName = "nvarchar")]
        [StringLength(50)]
        public string Name { get; set; }

        [DisplayName("Miêu Tả")]
        [MaxLength(150), Column(TypeName = "nvarchar")]
        [StringLength(150)]
        public string MieuTa { get; set; }
    }
}
