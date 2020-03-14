using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Data.Models
{
    public class Role
    {
        public int Id { get; set; }

        [DisplayName("Tên Role")]
        [MaxLength(50), Column(TypeName = "nvarchar(50)")]
        public string Name { get; set; }

        [DisplayName("Miêu Tả")]
        [MaxLength(50), Column(TypeName = "nvarchar(50)")]
        public string MieuTa { get; set; }
    }
}
