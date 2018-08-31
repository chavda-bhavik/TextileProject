
using Pal.Entities.Models;
using Pal.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Caching;
using System.Web.Mvc;

namespace PalWeb.Helpers
{
	public class CacheService
	{
		IListService _listService;
		public CacheService(IListService listService)
		{

			_listService = listService;

		}

		public CacheService()
		{
			// TODO: Complete member initialization
		}
		public IEnumerable<tblLists> GetLookupList()
		{
			if (HttpRuntime.Cache["PalList"] == null)
			{
				RefreshLookupListFromDB();
			}
			return (IEnumerable<tblLists>)HttpRuntime.Cache["PalList"];
		}
		private IEnumerable<tblLists> RefreshLookupListFromDB()
		{
			if (_listService == null) _listService = DependencyResolver.Current.GetService<IListService>();
			List<tblLists> list = _listService.Queryable().ToList();
			HttpRuntime.Cache["PalList"] = list;
			return list;
		}
	}
}