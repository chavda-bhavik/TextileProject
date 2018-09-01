using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;

namespace Pal.Utility
{
	class EnumHelper
	{
		public static T Parse<T>(string input)
		{
			return (T)Enum.Parse(typeof(T), input, true);
		}
	}

	public class Helper
    {
		public static string GetAutoCode(int id)
		{
			id = id + 1;
			if (id < 10)
				return "00" + id.ToString();
			else if(id > 9 && id <100)
				return "0" + id.ToString();
			else
				return id.ToString();
		}

		public static string GetListToString(List<string> lstValue)
		{
			string sValue = "";
			foreach (var item in lstValue)
			{
				sValue += item.ToString() + ",";
			}

			return sValue;
		}

		public static string GetSQLValue(object value)
        {
            string sqlValue = string.Empty;

            if (value is DateTime)
            {
                sqlValue = "'" + Convert.ToString((DateTime)value) + "'";
            }

            if (value is string)
            {
                sqlValue = "'" + ((string)value).Replace("'", "''") + "'";
            }

            if (value is int || value is double || value is long || value is decimal)
            {
                sqlValue = value.ToString();
            }

            if (value is bool)
            {
                sqlValue = (value.Equals(true) ? "1" : "0");
            }
            if (value == null)
            {
                sqlValue = "null";
            }
            return sqlValue;
        }

        //public static string GetSQLValue(object value, Field f)
        //{

        //	string sqlValue = string.Empty;
        //	if (value == null || value.Equals(string.Empty) )
        //	{
        //		return "null";
        //	}

        //	switch (f.FieldType.ToLower())
        //	{
        //		case "datetime":
        //			{
        //				sqlValue = "'" + value.ToString() + "'" ;//"'" + Convert.ToString((DateTime)value) + "'";
        //				break;
        //			}
        //		case "text":
        //		case "longtext":
        //		case "char":
        //		case "html":
        //		case "multiline text":
        //		case "multiselect":
        //			{
        //				sqlValue = "'" + ((string)value).Replace("'", "''") + "'";
        //				break;
        //			}
        //		case "integer":
        //		case "double":
        //		case "numeric":
        //		case "listitem":
        //		case "singleselection":
        //			{
        //				sqlValue = value.ToString();
        //				break;
        //			}
        //		case "boolean":
        //			{
        //				sqlValue = (value.Equals(true) ? "1" : "0");
        //				break;
        //			}
        //		case "documentattachment":
        //			{
        //				if (f.EntityId == 6)
        //				{
        //					sqlValue = "'" + ((string)value).Replace("'", "''") + "'";
        //				}
        //				else
        //				{
        //					sqlValue = value.ToString();
        //				}
        //				break;
        //			}
        //		default:
        //			{
        //				throw new InvalidOperationException();
        //			}
        //	}

        //	return sqlValue;

        //}

        public static string GetString(object value)
        {
            if ((value == null) || (value is DBNull && value.Equals(DBNull.Value)))
            {
                return string.Empty;
            }
            else
            {
                return System.Convert.ToString(value);
            }
        }

        public static int GetInt32(object value)
        {
            if ((value == null) || (value.Equals(DBNull.Value)) || (value is string && string.IsNullOrEmpty((string)value)))
            {
                return 0;
            }
            else
            {
                return System.Convert.ToInt32(value);
            }
        }

        public static bool GetBoolean(object value)
        {
            if (value == null || (value is DBNull && value.Equals(DBNull.Value)))
            {
                return false;
            }
            else
            {
                return System.Convert.ToBoolean(value);
            }
        }

        public static DateTime GetDateTime(object value)
        {
            if ((value == null) || (value is DBNull && value.Equals(DBNull.Value)))
            {
                return DateTime.MinValue;
            }
            else
            {
                return System.Convert.ToDateTime(value);
            }
        }

        public static DateTime GetDateTimeFromJavaScript(string val)
        {
            try
            {
                string s = val.Substring(4, 11);
                return System.DateTime.Parse(s);
            }
            catch
            {
                return DateTime.MinValue;
            }
        }

        public static string GetDateString(object value, string format = "dd/MM/yyyy")
        {
            if ((value == null) || (value is DBNull && value.Equals(DBNull.Value)))
            {
                return string.Empty;
            }
            else
            {
                return Convert.ToDateTime(value).ToString(format);
            }
        }

        public static string GetInt32Text(object value, int emptyValue)
        {
            int i = GetInt32(value);

            return (i.Equals(emptyValue)) ? String.Empty : i.ToString();
        }

        public static string GetDateShortText(DateTime dt)
        {
            return (dt.Equals(DateTime.MinValue)) ? string.Empty : dt.ToShortDateString();
        }

        public static string GetDateShortText(object o)
        {
            return GetDateShortText(GetDateTime(o));
        }

        public static double GetDouble(object value)
        {
            if ((value == null) || (value is DBNull && value.Equals(DBNull.Value)) || (value.Equals(string.Empty)))
            {
                return 0;
            }
            else
            {
                return System.Convert.ToDouble(value);
            }
        }

        public static decimal GetDecimal(object value)
        {
            if ((value == null) || (value is DBNull && value.Equals(DBNull.Value)) || (value is string && string.IsNullOrEmpty(value as string)))
            {
                return 0;
            }
            else
            {
                return System.Convert.ToDecimal(value);
            }
        }

        public static object GetValueIfContains(DataRow dr, string col)
        {
            return GetValueIfContains(dr.Table.Columns, dr, col);
        }

        public static object GetValueIfContains(DataColumnCollection columns, DataRow dr, string col)
        {
            return (columns.Contains(col) ? dr[col] : null);
        }

        public static DateTime GetString2Date(object value, string formatType = "dd/MM/yyyy")
        {
            if ((value == null) || (value is DBNull && value.Equals(DBNull.Value)))
            {
                return DateTime.MinValue;
            }
            else
            {
                return DateTime.ParseExact(value.ToString(), formatType, System.Globalization.CultureInfo.CreateSpecificCulture("en-GB"));
            }

        }

        //public static string ConvertDataTableToJSon(DataTable dt)
        //{
        //	System.Web.Script.Serialization.JavaScriptSerializer serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
        //	List<Dictionary<string, object>> rows = new List<Dictionary<string, object>>();
        //	Dictionary<string, object> row = new Dictionary<string, object>();
        //	DataColumnCollection columns = dt.Columns;

        //	try
        //	{
        //		foreach (DataRow dr in dt.Rows)
        //		{
        //			row = new Dictionary<string, object>();
        //			foreach (DataColumn col in columns)
        //			{
        //				row.Add(col.ColumnName, dr[col].ToString());
        //			}
        //			rows.Add(row);
        //		}

        //		return serializer.Serialize(rows);
        //	}

        //	catch (Exception ex)
        //	{
        //		throw;
        //	}
        //}

        public static List<SelectionDropdownVM> ConvertToSelectionDropdownVM(DataTable dt, string primaryKeyColName, string nameColName, string otherDataColName)
        {
            DataColumnCollection columns = dt.Columns;
            bool isOtherData = !string.IsNullOrEmpty(otherDataColName);
            List<SelectionDropdownVM> lst = new List<SelectionDropdownVM>();
            try
            {
                foreach (DataRow dr in dt.Rows)
                {
                    SelectionDropdownVM row = new SelectionDropdownVM();
                    row.PrimaryKey = GetInt32(dr[primaryKeyColName]).ToString();
                    row.Name = GetString(dr[nameColName]);
                    if (isOtherData)
                        row.OtherField = GetString(dr[otherDataColName]);

                    lst.Add(row);
                }

                return lst;
            }

            catch (Exception ex)
            {
                throw;
            }
        }

		public static DateTime ConvertUTCtoUserDate(string userTimeZone, DateTime utcDate)
		{
			DateTime dt = TimeZoneInfo.ConvertTimeFromUtc(utcDate, GetTZI(userTimeZone));
			dt = DateTime.SpecifyKind(dt, DateTimeKind.Unspecified);

			return dt;
		}

		public static TimeZoneInfo GetTZI(string userTimeZone)
		{
			TimeZoneInfo tzi;
			try
			{
				tzi = TimeZoneInfo.FindSystemTimeZoneById(userTimeZone);
			}
			catch
			{
				// Default is Eastern Standard Time if TimeZone is not found. 
				tzi = TimeZoneInfo.FindSystemTimeZoneById("Pacific Standard Time");
			}

			return tzi;
		}
		
	}

	public static class DHExtension
	{
		//private static Logger logger = LogManager.GetCurrentClassLogger();

		public static bool ContainsData(this DataSet ds)
		{
			return (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0);
		}
		public static void ConvertUTCToActualDate(this DataColumn column, string timeZone)
		{
			foreach (DataRow row in column.Table.Rows)
			{

				object value = row[column];
				if (value != null && "".Equals(value) == false)
				{
					DateTime dt = Helper.GetDateTime(value);
					if (dt.Equals(DateTime.MinValue) == true)
					{
						value = DBNull.Value;
					}
					else
					{
						dt = Helper.ConvertUTCtoUserDate(timeZone, dt);
						value = dt.ToString("g");
					}
					
				}
				row[column] = value;
				
			}
		}
	}

	public class SelectionDropdownVM
    {
        public string PrimaryKey { get; set; }
        public string Name { get; set; }
        public string OtherField { get; set; }

    }

	public static class IEnumerableExtensions
	{
		public static DataTable AsDataTable<T>(this IEnumerable<T> data)
		{
			PropertyDescriptorCollection properties = TypeDescriptor.GetProperties(typeof(T));
			var table = new DataTable();
			foreach (PropertyDescriptor prop in properties)
				table.Columns.Add(prop.Name, Nullable.GetUnderlyingType(prop.PropertyType) ?? prop.PropertyType);
			foreach (T item in data)
			{
				DataRow row = table.NewRow();
				foreach (PropertyDescriptor prop in properties)
					row[prop.Name] = prop.GetValue(item) ?? DBNull.Value;
				table.Rows.Add(row);
			}
			return table;
		}
	}
}
