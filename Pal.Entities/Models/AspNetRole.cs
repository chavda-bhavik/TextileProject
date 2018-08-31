using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pal.Entities.Models
{
	public partial class AspNetRole
	{
		public AspNetRole()
		{
			AspNetUsers = new HashSet<AspNetUser>();
		}

		public string Id { get; set; }

		[Required]
		[StringLength(256)]
		public string Name { get; set; }

		public virtual ICollection<AspNetUser> AspNetUsers { get; set; }
	}
}
