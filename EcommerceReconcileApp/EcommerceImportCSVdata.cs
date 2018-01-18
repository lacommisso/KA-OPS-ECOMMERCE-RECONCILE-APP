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
	public class EcommerceImportCSVdata
	{

		public static IImportTableDetails InitTableColumns(ref DataColumnCollection columns ,string tablename)
		{
			IImportTableDetails table = null;

			if (tablename.Equals(Table_Import_AX_Deposits.Name))
			{
				table = new Table_Import_AX_Deposits();
				table.TableDetails(ref columns, Table_Import_AX_Deposits.Name);
			}

			return table;
		}


		public static DataTable GetDataTabletFromCSVFile(Stream filestream, string tablename)
		{
			DataTable csvData = new DataTable();
			filestream.Position = 0;
			DataColumnCollection columns = csvData.Columns;
			IImportTableDetails tableDetails = InitTableColumns(ref columns, tablename);
			try
			{
				using (TextFieldParser csvReader = new TextFieldParser(filestream))
				{
					csvReader.SetDelimiters(new string[] { "|" });
					csvReader.HasFieldsEnclosedInQuotes = false;
					bool firstline = true;

					long lineNum = csvReader.LineNumber;
					while (!csvReader.EndOfData)
					{ 
						//string[] colFields = csvReader.ReadFields();
						//if (colFields.Length != columns.Count)
						//	throw new Exception();

						//foreach (string column in colFields)
						//{
						//	DataColumn datecolumn = new DataColumn(column);
						//	datecolumn.AllowDBNull = true;
						//	csvData.Columns.Add(datecolumn);
						//}
						while (!csvReader.EndOfData)
						{
							string[] fieldData = csvReader.ReadFields();
							//If datafile contains header info, skip that data.
							if (!(tableDetails.ContainsHeaderRow && lineNum == 1))
							{
								DataRow row = csvData.NewRow();
								//Making empty value as null
								for (int i = 0; i < fieldData.Length; i++)
								{
									if (fieldData[i] == "")
									{
										fieldData[i] = null;
									}
								}
								csvData.Rows.Add(fieldData);
							}
							lineNum++;
						}
						csvData.AcceptChanges();
					}
				}
			}
			catch (Exception ex)
			{
				return null;
			}
			return csvData;
		}

		public static bool InsertDataIntoSQLServerUsingSQLBulkCopy(DataTable csvFileData, string tablename)
		{
			bool success = true;
			try { 
				string connectionStr = "Data Source=tcp:irisdwhserver.database.windows.net,1433;Initial Catalog=TestEcommerceReconciliationData;Integrated Security=False;User ID=irisadmin@irisdwhserver.database.windows.net;Password=Rbm101Development;Connect Timeout=30;Encrypt=True";
				using (SqlConnection dbConnection = new SqlConnection(connectionStr))
				{
					dbConnection.Open();

					SqlCommand commandRowCount = new SqlCommand(
						"SELECT COUNT(*) FROM " +
						"dbo."+ tablename+"; ", dbConnection);
					long countStart = System.Convert.ToInt32(
						commandRowCount.ExecuteScalar());

					using (SqlBulkCopy s = new SqlBulkCopy(dbConnection))
					{
						s.DestinationTableName = tablename;
						foreach (var column in csvFileData.Columns)
							s.ColumnMappings.Add(column.ToString(), column.ToString());
						s.WriteToServer(csvFileData);
					}

					commandRowCount = new SqlCommand(
					"SELECT COUNT(*) FROM " +
					"dbo." + tablename + "; ", dbConnection);
					countStart = System.Convert.ToInt32(
						commandRowCount.ExecuteScalar());
				}
			}catch (Exception ex)
			{
				success = false;
				return success;
			}
			return success;
		}

		}

}