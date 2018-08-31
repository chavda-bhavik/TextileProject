using Pal.Entities.Models;
using Pal.Utility;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Html;

namespace PalWeb.Helpers
{
	public static class PalHTMLHelperExtensions
	{
		public static MvcHtmlString MyListBox(this HtmlHelper helper, string name, IEnumerable<string> selectedValue, EnumClass.EnumListType enmListType, object htmlAttributes = null, string noSelection = "")
		{

			SelectListItem blankItem = new SelectListItem { Selected = true, Value = "-1", Text = string.Format("-- {0} --", noSelection) };
			List<SelectListItem> selectedList = new List<SelectListItem>();

			if (string.IsNullOrEmpty(name))
			{
				selectedList.Add(blankItem);
				return helper.ListBox(name, selectedList, htmlAttributes);
			}

			CacheService _cacheservice = new CacheService();
			List<tblLists> list = _cacheservice.GetLookupList().Where(x => x.ListType == enmListType.ToString()).ToList();

			IEnumerable<SelectListItem> items = from a in list
												select new SelectListItem
												{
													Text = string.Format("{0}-{1}", a.Id, a.ListDescription),
													Value = a.Id.ToString(),
													Selected = selectedValue != null && selectedValue.Contains(a.Id.ToString())
												};

			return helper.ListBox(name, items, htmlAttributes);

		}

		//public static MvcHtmlString MyDropdownList(this HtmlHelper helper, string name, string selectedValue, EnumClass.EnumListType enmListType, string noSelection = "", bool search = false, object htmlAttributes = null)
		public static MvcHtmlString MyDropdownList(this HtmlHelper helper, string name, string selectedValue, EnumClass.EnumListType enmListType, object htmlAttributes = null, string noSelection = "", bool search = false)
		{

			SelectListItem blankItem = new SelectListItem { Selected = true, Value = "-1", Text = string.Format(" {0} ", noSelection) };
			List<SelectListItem> selectedList = new List<SelectListItem>();

			if (string.IsNullOrEmpty(name))
			{
				selectedList.Add(blankItem);
				return helper.DropDownList(name, selectedList, htmlAttributes);
			}

			CacheService _cacheservice = new CacheService();
			List<tblLists> list = _cacheservice.GetLookupList().Where(x => x.ListType == enmListType.ToString()).ToList();

			IEnumerable<SelectListItem> items = from a in list
												select new SelectListItem
												{
													Text = string.Format("{0}", a.ListDescription),
													Value = a.Id.ToString(),
													Selected = selectedValue != null && selectedValue.Equals(a.Id.ToString())
												};

			selectedList = items.ToList(); // Create mutable list

			if (search)
			{
				selectedList.Insert(0, blankItem); // Add at beginning of list
			}

			return helper.DropDownList(name, selectedList, htmlAttributes);
		}

		public static MvcHtmlString MyCheckBoxList(this HtmlHelper helper, string name, IEnumerable<string> selectedValue, EnumClass.EnumListType enmListType, object htmlAttributes = null)
		{

			if (String.IsNullOrEmpty(name))
			{
				return helper.CheckBox(name, false, htmlAttributes);
			}

			CacheService _cacheservice = new CacheService();
			List<tblLists> list = _cacheservice.GetLookupList().Where(x => x.ListType == enmListType.ToString()).ToList();

			IEnumerable<SelectListItem> listitems = from a in list
													select new SelectListItem
													{
														Text = string.Format("{0}", a.ListDescription),
														Value = a.Id.ToString(),
														Selected = selectedValue != null && selectedValue.Contains(a.Id.ToString())
													};
			//return helper.MyCheckBoxList(name, items, htmlAttributes);

			StringBuilder sb = new StringBuilder();
			foreach (var info in listitems)
			{
				TagBuilder builder = new TagBuilder("input");
				if (info.Selected) builder.MergeAttribute("checked", "checked");
				if (htmlAttributes != null) builder.MergeAttributes(HtmlHelper.AnonymousObjectToHtmlAttributes(htmlAttributes));
				builder.MergeAttribute("type", "checkbox");
				builder.MergeAttribute("value", info.Value);
				builder.MergeAttribute("name", name);
				builder.InnerHtml = info.Text;
				sb.Append(builder.ToString(TagRenderMode.Normal));
				sb.Append("<br />");
			}

			return new MvcHtmlString(sb.ToString());
		}

		public static MvcHtmlString MyLabel(this HtmlHelper helper, string name, string selectedValue, EnumClass.EnumListType enmListType, object htmlAttrib = null)
		{
			if (string.IsNullOrEmpty(name) || selectedValue == null || selectedValue == "-1") return helper.Label(name, "");

			CacheService _cacheservice = new CacheService();
			List<tblLists> list = _cacheservice.GetLookupList().Where(x => x.ListType == enmListType.ToString()).ToList();

			var item = list.FirstOrDefault(i => i.Id.ToString() == selectedValue);

			if (item == null) return helper.Label(name, "");

			if (htmlAttrib == null) htmlAttrib = new { @style = "font-weight: normal" };

			return helper.Label(name, item.ListDescription, htmlAttrib);

		}

		//public static MvcHtmlString MyHeader(this HtmlHelper helper, string RegardingEntity, int RegardingEntityId)
		//{

		//	//return helper.Label(Header.Value.ToString());
		//	return helper.Label("Header", MyStringHeader(RegardingEntity, RegardingEntityId));
		//}

		//public static string MyStringHeader(string RegardingEntity, int RegardingEntityId)
		//{
		//	string connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["PharmalogicIdentityConnection"].ConnectionString;

		//	DbContext db = new DbContext(connectionString);

		//	//var test = db.Database.SqlQuery<Order>("SP_GetHeader @id", new SqlParameter("@id", 1)).ToList();
		//	var Header = new SqlParameter();
		//	Header.ParameterName = "@Header";
		//	Header.Direction = ParameterDirection.Output;
		//	Header.SqlDbType = SqlDbType.NVarChar;
		//	Header.Size = 1000;

		//	db.Database.ExecuteSqlCommand("SP_GetHeader @RegardingEntity,@RegardingEntityId,@Header OUT",
		//		new SqlParameter("@RegardingEntity", RegardingEntity),
		//		new SqlParameter("@RegardingEntityId", RegardingEntityId),
		//		Header);

		//	return Header.Value.ToString();
		//}
	}
}