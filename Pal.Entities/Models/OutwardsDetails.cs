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
    [Table("OutwardsDetails")]
    public class OutwardsDetails:Entity
    {
        [Key, Column(Order = 1)]
        public int OutwardID { get; set; }
        [Key, Column(Order = 2)]
        public int PartyCode { get; set; }
        public DateTime ProvideDate { get; set; }
        public DateTime EstimatedReturn { get; set; }
        public int ProvidedItems { get; set; }
        public int Unit { get; set; }
        public String Color { get; set; }
        public double Length { get; set; }
    }
}
