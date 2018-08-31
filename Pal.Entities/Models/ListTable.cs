using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Repository.Pattern.Ef6;

namespace Pal.Entities.Models
{
    [Table("tblLists")]
	public partial class tblLists : Entity
	{
		[Key]
		public int Id { get; set; }

		[Required]
		[StringLength(100)]
		public string ListType { get; set; }

		[StringLength(100)]
		public int?  ListValue { get; set; }

		[Required]
		[StringLength(500)]
		public string ListDescription { get; set; }

		public int? SortOrder { get; set; }

		public bool Lock { get; set; }
	}
}
