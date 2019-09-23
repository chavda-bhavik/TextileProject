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
    [Table("Jobwork")]
    public class Jobwork : Entity
    {
        [Key]
        public String VoucherNo { get; set; }
        public int JobworkType { get; set; }
        public int ProductCode { get; set; }
    }
}
