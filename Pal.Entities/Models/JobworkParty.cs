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
    [Table("JobworkParty")]
    public class JobworkParty: Entity
    {
        [Key]
        public int Code { get; set; }
        public String OwnerName { get; set; }
        [Required]        
        public String AgencyName { get; set; }
        public String Address { get; set; }
        [StringLength(10)]
        public String Mobile { get; set; }
        public String EmailID { get; set; }
        public String GSTNo { get; set; }

    }
}
