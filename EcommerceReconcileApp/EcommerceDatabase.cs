using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;

namespace EcommerceReconcileApp
{
	public class EcommerceDatabase
	{
		private string connectionStr = "Data Source=tcp:irisdwhserver.database.windows.net,1433;Initial Catalog=TestEcommerceReconciliationData;Integrated Security=False;User ID=irisadmin@irisdwhserver.database.windows.net;Password=Rbm101Development;Connect Timeout=30;Encrypt=True";
		private static EcommerceDatabase _me;
		public static EcommerceDatabase DB
		{
			get
			{
				if (_me == null)
					_me = new EcommerceDatabase();
				return _me;
			}
		}
		
		public static bool RunSQL(string statement)
		{
			bool success = true;
			try
			{
				string connectionStr = EcommerceDatabase.DB.connectionStr;
				using (SqlConnection dbConnection = new SqlConnection(connectionStr))
				{
					dbConnection.Open();

					SqlCommand commandRowCount = new SqlCommand(statement, dbConnection);
					long countStart = System.Convert.ToInt32(
						commandRowCount.ExecuteScalar());
				}
			}
			catch (Exception ex)
			{
				success = false;
				return success;
			}
			return success;
		}

		public static bool RunStoredProcedure(string procedureName)
		{
			bool success = true;
			SqlDataAdapter adapter;
			DataSet ds = new DataSet();
			try
			{
				using (SqlConnection dbConnection = new SqlConnection(EcommerceDatabase.DB.connectionStr))
				{
					dbConnection.Open();

					SqlCommand sqlCmd = new SqlCommand(procedureName, dbConnection);
					sqlCmd.CommandType = CommandType.StoredProcedure;
					adapter = new SqlDataAdapter(sqlCmd);
					adapter.Fill(ds);
				}
			}
			catch (Exception ex)
			{
				success = false;
				return success;
			}
			return success;
		}

		public static bool BulkImportTable(DataTable csvFileData, string tablename)
		{
			bool success = true;
			try
			{
				using (SqlConnection dbConnection = new SqlConnection(EcommerceDatabase.DB.connectionStr))
				{
					dbConnection.Open();
					using (SqlBulkCopy s = new SqlBulkCopy(dbConnection))
					{
						s.DestinationTableName = tablename;
						foreach (var column in csvFileData.Columns)
							s.ColumnMappings.Add(column.ToString(), column.ToString());
						s.WriteToServer(csvFileData);
					}
				}
			}
			catch (Exception ex)
			{
				success = false;
				return success;
			}
			return success;
		}

		public static DataTable GetData(string cmd)
		{
			bool success = true;
			SqlDataAdapter adapter;
			DataTable dt = new DataTable();
			try
			{
				using (SqlConnection dbConnection = new SqlConnection(EcommerceDatabase.DB.connectionStr))
				{
					dbConnection.Open();

					SqlCommand sqlCmd = new SqlCommand(cmd, dbConnection);
					sqlCmd.CommandType = CommandType.Text;
					adapter = new SqlDataAdapter(sqlCmd);
					adapter.Fill(dt);
				}
			}
			catch (Exception ex)
			{
				success = false;
				dt = null;
			}
			return dt;
		}


	}
}