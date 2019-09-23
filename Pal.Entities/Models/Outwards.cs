using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Repository.Pattern.Ef6;

namespace Pal.Entities.Models
{
    [Table("Outwards")]
    public class Outwards:Entity
    {
        [Key]
        public int OutwardID { get; set; }
        public String OutwardVoucherNo { get; set; }
        public int JobworkCode { get; set; }
    }
}
