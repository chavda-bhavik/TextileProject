using System;
using System.Collections.Generic;
using Repository.Pattern.Ef6;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Pal.Entities.Models
{
    [Table("AuditLog")]
    public partial class AuditLog :Repository.Pattern.Ef6.Entity
    {

        public Guid AuditLogID { get; set; }

        [Required]
        [StringLength(100)]
        public string userid { get; set; }

        public DateTime eventdateutc { get; set; }

        [Required]
        [StringLength(1)]
        public string eventtype { get; set; }

        [Required]
        [StringLength(100)]
        public string tablename { get; set; }

        [Required]
        [StringLength(100)]
        public string recordid { get; set; }

        [Required]
        [StringLength(100)]
        public string columnname { get; set; }

        public string originalvalue { get; set; }

        public string newvalue { get; set; }
    }
}
