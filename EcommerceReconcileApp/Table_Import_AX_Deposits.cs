using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using Microsoft.VisualBasic.FileIO;

namespace EcommerceReconcileApp
{
	public class Table_Import_AX_Deposits : IImportTableDetails
	{
		private bool _containsHeaderRow = false;

		static public string Name = "Import_AX_DEPOSITS";
		static public int NumOfColumns = 9;
		private static Table_Import_AX_Deposits _me;
		public static Table_Import_AX_Deposits ImportAxDeposits
		{
			get
			{
				if (_me == null)
					_me = new Table_Import_AX_Deposits();
				return _me;
			}
		}
		public string TableName { get { return Table_Import_AX_Deposits.Name; } }
		static public string ProcessImportSP { get { return "SP_Process_Ax_Deposit_data"; } }

		public bool ContainsHeaderRow { get { return _containsHeaderRow; } }

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

		public static bool ImportData(DataTable csvFileData)
		{
			bool success = true;
			try
			{
				success = EcommerceDatabase.BulkImportTable(csvFileData, Name);
			}
			catch (Exception ex)
			{
				success = false;
				return success;
			}
			return success;
		}
		/// <summary>
		/// Run the process import stored procedure which brings data into the main tables from the import tables.  Processing
		/// the import does data integrity checks and processes only valid data.  Data that cannot be processed is left in the
		/// import table to be handled by the ecommerce manager.
		/// </summary>
		/// <returns></returns>
		public static bool ProcessImportedData()
		{
			bool success = true;
			try
			{
				success = EcommerceDatabase.RunStoredProcedure(ProcessImportSP);
			}
			catch (Exception ex)
			{
				success = false;
				return success;
			}
			return success;
		}

		public DataTable GetCurrentData()
		{
			bool success = true;
			DataTable data = new DataTable();
			try
			{
				data = EcommerceDatabase.GetData("select * from Import_AX_DEPOSITS");
			}
			catch (Exception ex)
			{
				success = false;
				return null;
			}
			return data;
		}

		public void DeleteRow(string columnValues)
		{
			string[] args = null;
			if (columnValues != null)
			{
				args = columnValues.Split(';');
				if (args.Length >= NumOfColumns)
				{
					string CollectionNameStr = args[0];
					string CustomerAccountStr = args[1];
					string DepositDateStr = args[2];
					string DueDateStr = args[3];
					string PONumberStr = args[4];
					string InvoiceStr = args[5];
					string AmountInTransactionCurrencyStr = args[6];
					string BalanceStr = args[7];
					string DescriptionStr = args[8];

					try
					{
						DataTable data = new DataTable();
						string p1 = (String.IsNullOrEmpty(CollectionNameStr)) ? "CollectionName is NULL" : ("CollectionName='" + CollectionNameStr + "'");
						string p2 = (String.IsNullOrEmpty(CustomerAccountStr)) ? "CustomerAccount is NULL" : ("CustomerAccount='" + CustomerAccountStr + "'");
						string p3 = (String.IsNullOrEmpty(DepositDateStr)) ? "DepositDate is NULL" : ("DepositDate='" + DepositDateStr + "'");
						string p4 = (String.IsNullOrEmpty(DueDateStr)) ? "DueDate is NULL" : ("DueDate='" + DueDateStr + "'");
						string p5 = (String.IsNullOrEmpty(PONumberStr)) ? "PONumber is NULL" : ("PONumber='" + PONumberStr + "'");
						string p6 = (String.IsNullOrEmpty(InvoiceStr)) ? "Invoice is NULL" : ("Invoice='" + InvoiceStr + "'");
						string p7 = (String.IsNullOrEmpty(AmountInTransactionCurrencyStr)) ? "AmountInTransactionCurrency is NULL" : ("AmountInTransactionCurrency='" + AmountInTransactionCurrencyStr + "'");
						string p8 = (String.IsNullOrEmpty(BalanceStr)) ? "Balance is NULL" : ("Balance='" + BalanceStr + "'");
						string p9 = (String.IsNullOrEmpty(DescriptionStr)) ? "Description is NULL" : ("Description='" + DescriptionStr + "'");
						string s = "DELETE FROM Import_AX_DEPOSITS WHERE " +
							p1 + " and " +
							p2 + " and " +
							p3 + " and " +
							p4 + " and " +
							p5 + " and " +
							p6 + " and " +
							p7 + " and " +
							p8 + " and " +
							p9;
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