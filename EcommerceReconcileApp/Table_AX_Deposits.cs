using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;

namespace EcommerceReconcileApp
{
	public class Table_AX_Deposits : ITableDetails
	{
		private bool _containsHeaderRow = false;
		static public string Name = "AX_DEPOSITS";

		public string TableName { get { return Table_AX_Deposits.Name; } }
		private static Table_AX_Deposits _me;
		public static Table_AX_Deposits AxDeposits
		{
			get
			{
				if (_me == null)
					_me = new Table_AX_Deposits();
				return _me;
			}
		}
		/// <summary>
		/// Columns to show not necessarily all columns in the table.
		/// </summary>
		/// <param name="columns"></param>
		/// <param name="tablename"></param>
		public void TableDetails(ref DataColumnCollection columns, string tablename)
		{
			_containsHeaderRow = true;

			DataColumn datecolumn = new DataColumn("CollectionName");
			datecolumn.AllowDBNull = true;
			columns.Add(datecolumn);
			datecolumn = new DataColumn("CustomerAccount");
			datecolumn.AllowDBNull = true;
			columns.Add(datecolumn);
			datecolumn = new DataColumn("DepositDate");
			datecolumn.AllowDBNull = true;
			columns.Add(datecolumn);
			datecolumn = new DataColumn("DueDate");
			datecolumn.AllowDBNull = true;
			columns.Add(datecolumn);
			datecolumn = new DataColumn("PONumber");
			datecolumn.AllowDBNull = true;
			columns.Add(datecolumn);
			datecolumn = new DataColumn("Invoice");
			datecolumn.AllowDBNull = true;
			columns.Add(datecolumn);
			datecolumn = new DataColumn("AmountInTransactionCurrency");
			datecolumn.AllowDBNull = true;
			columns.Add(datecolumn);
			datecolumn = new DataColumn("Balance");
			datecolumn.AllowDBNull = true;
			columns.Add(datecolumn);
			datecolumn = new DataColumn("Description");
			datecolumn.AllowDBNull = true;
			columns.Add(datecolumn);

		}



		public DataTable GetCurrentData()
		{
			bool success = true;
			DataTable data = new DataTable();
			try
			{
				data = EcommerceDatabase.GetData("select * from AX_DEPOSITS");
			}
			catch (Exception ex)
			{
				success = false;
				return null;
			}
			return data;
		}

		static public int NumOfColumnsForDelete = 1;
		public void DeleteRow(string columnValues)
		{
			string[] args = null;
			if (columnValues != null)
			{
				args = columnValues.Split(';');
				if (args.Length >= NumOfColumnsForDelete)
				{
					string uniqueIdStr = args[0];
					int result;
					if (int.TryParse(uniqueIdStr, out result))
					{ 
						try
						{
							DataTable data = new DataTable();
							string s = "DELETE FROM AX_DEPOSITS WHERE ID=" + result;
							data = EcommerceDatabase.GetData(s);
						}
						catch (Exception ex)
						{

						}
					}
				}
			}

		}

	}
}