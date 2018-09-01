using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Transactions;

namespace Pal.Utility
{
	public class SQLHelper
	{

		private static string _connectionString;
		public static string ConnectionString
		{
			get
			{
				if (string.IsNullOrEmpty(_connectionString))
				{
					string csKey = "DefaultConnection";   // ((UseLocalDatabase) ? "JCCompanyConnectionLocal" : "JCCompanyConnection");
					_connectionString = ConfigurationManager.ConnectionStrings[csKey].ConnectionString;
					//_connectionString = WebHelper.GetConnectionString();
				}
				return _connectionString;
			}
		}

		public static DataSet GetData(string sql)
		{
			System.Data.SqlClient.SqlDataAdapter da = new System.Data.SqlClient.SqlDataAdapter
				(sql, ConnectionString);
			DataSet ds = new DataSet();

			da.Fill(ds);

			return ds;
		}

		public static DataSet ExecuteProcedure(string procedureName, System.Data.SqlClient.SqlParameter[] parameters)
		{

			string connString = ConnectionString;

#if DEBUG
			System.Diagnostics.Debug.WriteLine("Start Executing: " + procedureName + " " + DateTime.Now.ToString("MM/dd/yyyy hh:mm:ss.fff tt"));
#endif

			DataSet ds = new DataSet();
			SqlConnection conn = null;
			//SqlDataReader dr = null;

			TransactionOptions option = new TransactionOptions();
			option.IsolationLevel = System.Transactions.IsolationLevel.ReadCommitted;
			option.Timeout = new TimeSpan(0, 2, 0);

			try
			{
				using (TransactionScope trn = new TransactionScope(TransactionScopeOption.Required, option))
				{
					using (conn = new SqlConnection(connString))
					{
						System.Data.SqlClient.SqlCommand sc = new System.Data.SqlClient.SqlCommand
							(procedureName, conn);
						sc.CommandType = CommandType.StoredProcedure;
						sc.Parameters.AddRange(parameters);
						SqlDataAdapter da = new SqlDataAdapter(sc);

#if DEBUG
						{
							string s = PrepareExecuteStatement(procedureName, parameters);

						}
#endif

						da.Fill(ds);
						sc.Parameters.Clear();
					}
					trn.Complete();
				}

				return ds;
			}
			catch (Exception ex)
			{
				throw ex;
			}
			finally
			{
#if DEBUG
				System.Diagnostics.Debug.WriteLine("End Executing: " + procedureName + " " + DateTime.Now.ToString("MM/dd/yyyy hh:mm:ss.fff tt"));
#endif
				if (conn != null && conn.State != ConnectionState.Broken && conn.State != ConnectionState.Closed)
				{
					conn.Close();
				}
			}
		}

		public static DataSet ExecuteSelectStatement(string sql)
		{
			string connString = ConnectionString;

#if DEBUG
			System.Diagnostics.Debug.WriteLine("Start Executing: " + sql + " " + DateTime.Now.ToString("MM/dd/yyyy hh:mm:ss.fff tt"));
#endif

			DataSet ds = new DataSet();
			SqlConnection conn = null;
			//SqlDataReader dr = null;

			TransactionOptions option = new TransactionOptions();
			option.IsolationLevel = System.Transactions.IsolationLevel.ReadCommitted;
			option.Timeout = new TimeSpan(0, 2, 0);

			try
			{
				using (TransactionScope trn = new TransactionScope(TransactionScopeOption.Required, option))
				{
					using (conn = new SqlConnection(connString))
					{
						System.Data.SqlClient.SqlCommand sc = new System.Data.SqlClient.SqlCommand
							(sql, conn);
						//sc.CommandType = CommandType.TableDirect;
						SqlDataAdapter da = new SqlDataAdapter(sc);

						da.Fill(ds);
						sc.Parameters.Clear();
					}
					trn.Complete();
				}

				return ds;
			}
			catch (Exception ex)
			{
				throw ex;
			}
			finally
			{
#if DEBUG
				System.Diagnostics.Debug.WriteLine("End Executing: " + sql + " " + DateTime.Now.ToString("MM/dd/yyyy hh:mm:ss.fff tt"));
#endif
				if (conn != null && conn.State != ConnectionState.Broken && conn.State != ConnectionState.Closed)
				{
					conn.Close();
				}
			}
		}

		public static DataSet ExecuteSelectStatement(string[] sqlStatements)
		{
			string connString = ConnectionString;

#if DEBUG
			//System.Diagnostics.Debug.WriteLine("Start Executing: " + sql + " " + DateTime.Now.ToString("MM/dd/yyyy hh:mm:ss.fff tt"));
#endif

			DataSet ds = new DataSet();
			SqlConnection conn = null;
			//SqlDataReader dr = null;

			TransactionOptions option = new TransactionOptions();
			option.IsolationLevel = System.Transactions.IsolationLevel.ReadCommitted;
			option.Timeout = new TimeSpan(0, 2, 0);

			try
			{
				using (TransactionScope trn = new TransactionScope(TransactionScopeOption.Required, option))
				{
					using (conn = new SqlConnection(connString))
					{
						for (int i = 0; i < sqlStatements.Length; i++)
						{
							string sql = sqlStatements[i];
							DataSet dsInd = new DataSet();
							System.Data.SqlClient.SqlCommand sc = new System.Data.SqlClient.SqlCommand(sql, conn);
							//sc.CommandType = CommandType.TableDirect;
							SqlDataAdapter da = new SqlDataAdapter(sc);

							da.Fill(dsInd);
							dsInd.Tables[0].TableName = "Table" + i.ToString();
							ds.Merge(dsInd);
							//sc.Parameters.Clear();
						}
					}
					trn.Complete();
				}

				return ds;
			}
			catch (Exception ex)
			{
				throw ex;
			}
			finally
			{
#if DEBUG
				//System.Diagnostics.Debug.WriteLine("End Executing: " + sql + " " + DateTime.Now.ToString("MM/dd/yyyy hh:mm:ss.fff tt"));
#endif
				if (conn != null && conn.State != ConnectionState.Broken && conn.State != ConnectionState.Closed)
				{
					conn.Close();
				}
			}
		}

		public static object ExecuteScaler(string fsQuery)
		{
			SqlConnection sqlConn = null;
			SqlCommand sqlCmd = null;
			try
			{
				//Create SqlConnection
				string connString = ConnectionString;
				sqlConn = new SqlConnection(connString);
				sqlConn.Open();
				sqlCmd = new SqlCommand(fsQuery, sqlConn);
				return sqlCmd.ExecuteScalar();
			}
			catch (Exception ex)
			{
				//UtilityMethods.writeTextFile("ERROR IN ExecuteScaler method [TIME : " + System.DateTime.Now.ToString("MMM-dd-yyyy  hh:mm:ss tt  ") + "] " + ex.Message, "log.txt");
				return null;
			}
			finally
			{
				sqlCmd.Dispose();
				sqlConn.Close();
				sqlConn.Dispose();
			}
		}
		public static object ExecuteScalar(string procedureName, System.Data.SqlClient.SqlParameter[] parameters)
		{
			string connString = ConnectionString;

			object retVal = null;
			SqlConnection conn = null;

			TransactionOptions option = new TransactionOptions();
			option.IsolationLevel = System.Transactions.IsolationLevel.ReadCommitted;
			option.Timeout = new TimeSpan(0, 2, 0);

			try
			{
				using (TransactionScope trn = new TransactionScope(TransactionScopeOption.Required, option))
				{
					using (conn = new SqlConnection(connString))
					{
						conn.Open();
						System.Data.SqlClient.SqlCommand sc = new System.Data.SqlClient.SqlCommand(procedureName, conn);
						sc.CommandType = CommandType.StoredProcedure;
						sc.Parameters.AddRange(parameters);
						retVal = sc.ExecuteScalar();
						sc.Parameters.Clear();
					}
					trn.Complete();
					return retVal;
				}
			}
			catch (Exception ex)
			{
				throw ex;
			}
			finally
			{
				if (conn != null && conn.State != ConnectionState.Broken && conn.State != ConnectionState.Closed)
				{
					conn.Close();
				}
			}
			//return retVal;
		}

		public static object ExecuteXMLScalar(string procedureName, System.Data.SqlClient.SqlParameter[] parameters)
		{
			string connString = ConnectionString;

			object retVal = null;
			SqlConnection conn = null;

			TransactionOptions option = new TransactionOptions();
			option.IsolationLevel = System.Transactions.IsolationLevel.ReadCommitted;
			option.Timeout = new TimeSpan(0, 2, 0);

			try
			{
				using (TransactionScope trn = new TransactionScope(TransactionScopeOption.Required, option))
				{
					using (conn = new SqlConnection(connString))
					{
						conn.Open();
						System.Data.SqlClient.SqlCommand sc = new System.Data.SqlClient.SqlCommand(procedureName, conn);
						sc.CommandType = CommandType.StoredProcedure;
						sc.Parameters.AddRange(parameters);

						System.Xml.XmlReader xr = sc.ExecuteXmlReader();


						if (xr.Read())
						{
							retVal = xr.ReadOuterXml();
						}

						sc.Parameters.Clear();
					}
					trn.Complete();
					return retVal;
				}
			}
			catch (Exception ex)
			{
				throw ex;
			}
			finally
			{
				if (conn != null && conn.State != ConnectionState.Broken && conn.State != ConnectionState.Closed)
				{
					conn.Close();
				}
			}
			//return retVal;
		}

		/// <summary>
		/// This function will is used for INSERT/UPDATE/DELETE queries and will return rowsaffected
		/// </summary>
		/// <param name="procedureName"></param>
		/// <param name="parameters"></param>
		/// <returns></returns>
		public static int ExecuteNonQuery(string procedureName, System.Data.SqlClient.SqlParameter[] parameters)
		{
			string connString = ConnectionString;

			//object retVal = null;
			SqlConnection conn = null;
			int rowsaffected = 0;

			TransactionOptions option = new TransactionOptions();
			option.IsolationLevel = System.Transactions.IsolationLevel.ReadCommitted;
			option.Timeout = new TimeSpan(0, 2, 0);

			try
			{
				using (TransactionScope trn = new TransactionScope(TransactionScopeOption.Required, option))
				{
					using (conn = new SqlConnection(connString))
					{
						conn.Open();
						System.Data.SqlClient.SqlCommand sc = new System.Data.SqlClient.SqlCommand(procedureName, conn);
						sc.CommandType = CommandType.StoredProcedure;
						sc.Parameters.AddRange(parameters);
						rowsaffected = sc.ExecuteNonQuery();
						//if (rowsaffected <= 0)
						//{
						//    throw new Exception("Deletion failed");
						//}
						trn.Complete();
						sc.Parameters.Clear();
						return rowsaffected;
					}
				}
			}
			catch (Exception ex)
			{
				throw ex;
			}
			finally
			{
				if (conn != null && conn.State != ConnectionState.Broken && conn.State != ConnectionState.Closed)
				{
					conn.Close();
				}
			}
		}

		public static int ExecuteStatement(string statement)
		{
			string connString = ConnectionString;

			//object retVal = null;
			SqlConnection conn = null;
			int rowsaffected = 0;

			TransactionOptions option = new TransactionOptions();
			option.IsolationLevel = System.Transactions.IsolationLevel.ReadCommitted;
			option.Timeout = new TimeSpan(0, 2, 0);

			try
			{
				using (TransactionScope trn = new TransactionScope(TransactionScopeOption.Required, option))
				{
					using (conn = new SqlConnection(connString))
					{
						conn.Open();

						System.Data.SqlClient.SqlCommand sc = new System.Data.SqlClient.SqlCommand(statement, conn);
						sc.CommandType = CommandType.Text;
						rowsaffected = sc.ExecuteNonQuery();
						//if (rowsaffected <= 0)
						//{
						//    throw new Exception("Deletion failed");
						//}
						trn.Complete();
						sc.Parameters.Clear();
						return rowsaffected;
					}
				}
			}
			catch (Exception ex)
			{
				throw ex;
			}
			finally
			{
				if (conn != null && conn.State != ConnectionState.Broken && conn.State != ConnectionState.Closed)
				{
					conn.Close();
				}
			}
		}


		public static string PrepareExecuteStatement(string procedureName, SqlParameter[] parameters)
		{
			try
			{
				string s = string.Empty;

				foreach (SqlParameter p in parameters)
				{
					if (p.Value.Equals(DBNull.Value))
					{
						s += ", @" + p.ParameterName + " =  NULL ";
					}
					else
					{
						s += ", @" + p.ParameterName + " = '" + p.Value.ToString() + "'";
					}
				}

				if (s.Length > 0)
					s = s.Substring(2);

				s = "EXEC " + procedureName + " " + s;

				System.Diagnostics.Debug.WriteLine(s);
				return s;
			}
			catch
			{
				return "Error While Creating Exec Statement";
			}
		}

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

		public static object GetParamValue(int val)
		{
			return (val == 0) ? (object)DBNull.Value : (object)val;
		}

		public static object GetParamValue(string val)
		{
			return (string.IsNullOrEmpty(val)) ? (object)DBNull.Value : (object)val;
		}

		public static object GetParamValue(DateTime val)
		{
			return (val == null || val.Equals(DateTime.MinValue)) ? (object)DBNull.Value : (object)val;
		}

		public static object GetParamValue(object val)
		{
			return (val == null) ? DBNull.Value : val;
		}

		public static object GetParamValue(double? val)
		{
			return (val == null || val.Equals(0.00)) ? (object)DBNull.Value : Convert.ToDouble(val);
		}

		public static object GetParamValue(DateTime? val)
		{
			return (val == null) ? (object)DBNull.Value : Convert.ToDateTime(val);
		}

		public static string GetTextFromHTML(string html)
		{

			System.Text.RegularExpressions.Regex regex = new System.Text.RegularExpressions.Regex("(<.*?>\\s*)+", System.Text.RegularExpressions.RegexOptions.Singleline);

			string resultText = regex.Replace(html, " ").Trim();

			return resultText;

		}

		public static void BulkInertMethod(string destinationTable, List<string> blukCopyCollection, DataTable srcDataTable, bool KeepIdentity)
		{
			using (SqlConnection conn = new SqlConnection(ConnectionString))
			{
				conn.Open();
				SqlTransaction transaction = conn.BeginTransaction();
				if (KeepIdentity == true)
				{
					using (SqlBulkCopy sqlBulkCopyTable = new SqlBulkCopy(conn, SqlBulkCopyOptions.KeepIdentity, transaction))
					{
						foreach (var bulkColumn in blukCopyCollection)
						{
							var split = bulkColumn.Split(new[] { ',' });
							sqlBulkCopyTable.ColumnMappings.Add(split[0].Trim(), split[1].Trim());
						}
						sqlBulkCopyTable.DestinationTableName = destinationTable;
						sqlBulkCopyTable.BulkCopyTimeout = 1800; //3 Minutes
						sqlBulkCopyTable.BatchSize = 100;
						try
						{
							sqlBulkCopyTable.WriteToServer(srcDataTable);
							transaction.Commit();
						}
						catch (SqlException ex)
						{
							conn.Close();
						}
					}
				}
				else
				{
					using (SqlBulkCopy sqlBulkCopyTable = new SqlBulkCopy(conn, SqlBulkCopyOptions.Default, transaction))
					{
						foreach (var bulkColumn in blukCopyCollection)
						{
							var split = bulkColumn.Split(new[] { ',' });
							sqlBulkCopyTable.ColumnMappings.Add(split[0].Trim(), split[1].Trim());
						}
						sqlBulkCopyTable.DestinationTableName = destinationTable;
						sqlBulkCopyTable.BulkCopyTimeout = 1800; //3 Minutes
						try
						{
							sqlBulkCopyTable.WriteToServer(srcDataTable);
							transaction.Commit();
						}
						catch (SqlException ex)
						{
							conn.Close();
						}
					}
				}
			}
		}
		
	}

}
