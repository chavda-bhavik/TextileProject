using Pal.Entities.Models;
using Repository.Pattern.Repositories;
using Service.Pattern;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pal.Services
{
	public interface IListService : IService<tblLists>
	{
	}
	public class ListService : Service<tblLists>, IListService
	{
		public ListService(IRepositoryAsync<tblLists> respository)
			: base(respository)
		{


		}
	}
}
