using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Windows.Input;
using System.Web.Services;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using System.Data;
using Microsoft.VisualBasic.FileIO;

namespace EcommerceReconcileApp
{


	public partial class _Default : Page
	{

		private EcommerceBlobStorage ecommerceBlobStorage;

		public EcommerceBlobStorage BlobStorage
		{
			get
			{
				if (Session[EcommerceBlobStorage.BlobStorage] == null)
				{
					ecommerceBlobStorage = new EcommerceBlobStorage(Session);
					Session[EcommerceBlobStorage.BlobStorage] = ecommerceBlobStorage;
				}
				else
				{
					object obj = (object)Session[EcommerceBlobStorage.BlobStorage];
					ecommerceBlobStorage = (EcommerceBlobStorage)Session[EcommerceBlobStorage.BlobStorage];
				}
				return ecommerceBlobStorage;
			}
			set
			{
				Session[EcommerceBlobStorage.BlobStorage] = value;
			}
		}

		protected void Page_Load(object sender, EventArgs e)
		{
			//TODO: when page loads - read in any blobs in import containers in case they were left there on previous run

			//ProcessFilesBtn.Enabled = false;
			//UploadAndProcessBtn.Visible = false;
			ecommerceBlobStorage = BlobStorage;
			UpdateAxDeposit_gridview();
			this.UpdateBTDisbursements_gridview();
			this.UpdateBtTransaction_gridview();
		}

		protected void MyRefreshPage()
		{
			UpdateAxDeposit_gridview();
			UpdateBTDisbursements_gridview();
			UpdateBtTransaction_gridview();
		}
		
		public bool UploadToStorage(string containerName, HttpPostedFile file)
		{
			return ecommerceBlobStorage.ImportToStorage(containerName, file);

		}

		protected void DeleteUploadedBlobsBtn_Click(object sender, EventArgs e)
		{
			DeleteAllUploadedBlobsFromSession();
			MyRefreshPage();
		}

		protected bool DeleteAllUploadedBlobsFromSession()
		{
			bool successful = false;
			try
			{
				ecommerceBlobStorage.DeleteAllBlobsInContainer(ecommerceBlobStorage.IMPORT_AX_DEPOSITS_CONTAINER_NAME);
				ecommerceBlobStorage.DeleteAllBlobsInContainer(ecommerceBlobStorage.IMPORT_BT_DISBURSEMENTS_CONTAINER_NAME);
				ecommerceBlobStorage.DeleteAllBlobsInContainer(ecommerceBlobStorage.IMPORT_BT_TRANSACTIONS_CONTAINER_NAME);
			}
			catch (Exception ex)
			{
				successful = false;
			}
			return successful;
		}

		protected void ViewImportAxDataBtn_Click(object sender, EventArgs e)
		{
			Response.Redirect("DataShowImportAxDeposits.aspx");
		}
		protected void ViewAxDataBtn_Click(object sender, EventArgs e)
		{
			Response.Redirect("DataShowAxDeposits.aspx");
		}


		protected void ProcessBtDisbursementsBtn_Click(object sender, EventArgs e)
		{
		}
		protected void ProcessBtTransactionBtn_Click(object sender, EventArgs e)
		{
		}
		protected void ProcessAxDepositBtn_Click(object sender, EventArgs e)
		{
		}


		protected void UpdateAxDeposit_gridview()
		{
			bool check = IsPostBack;
			//Want to always update the gridviews because we have a delete all uploaded blobs so we want the page to reflect current state
			List<string> files = ecommerceBlobStorage.ReturnFilesInContainer(ecommerceBlobStorage.IMPORT_AX_DEPOSITS_CONTAINER_NAME);
			Session["AxDepositFilesGridView"] = files;
			AxDepositGridView.DataSource = files;
			AxDepositGridView.DataBind();
		}
		protected void UpdateBTDisbursements_gridview()
		{
			bool check = IsPostBack;
			//Want to always update the gridviews because we have a delete all uploaded blobs so we want the page to reflect current state
			List<string> files = ecommerceBlobStorage.ReturnFilesInContainer(ecommerceBlobStorage.IMPORT_BT_DISBURSEMENTS_CONTAINER_NAME);
			Session["BTDisbursementsFilesGridView"] = files;
			BTDisbursementsGridView.DataSource = files;
			BTDisbursementsGridView.DataBind();
		}
		protected void UpdateBtTransaction_gridview()
		{
			bool check = IsPostBack;
			//Want to always update the gridviews because we have a delete all uploaded blobs so we want the page to reflect current state
			List<string> files = ecommerceBlobStorage.ReturnFilesInContainer(ecommerceBlobStorage.IMPORT_BT_TRANSACTIONS_CONTAINER_NAME);
			Session["btTransactionFilesGridView"] = files;
			btTransactionGridView.DataSource = files;
			btTransactionGridView.DataBind();
		}

		protected void AxDepositGridView_SelectedIndexChanged(object sender, EventArgs e)
		{
			UpdateAxDeposit_gridview();
		}
		protected void BTDisbursementsGridView_SelectedIndexChanged(object sender, EventArgs e)
		{
			UpdateBTDisbursements_gridview();
		}
		protected void btTransactionGridView_SelectedIndexChanged(object sender, EventArgs e)
		{
			UpdateAxDeposit_gridview();
		}

		protected void ImportAxDepositBtn_Click(object sender, EventArgs e)
		{
			ImportAxDeposits();
		}
		protected void ImportAxDeposits()
		{
			List<string> files = ecommerceBlobStorage.ReturnFilesInContainer(ecommerceBlobStorage.IMPORT_AX_DEPOSITS_CONTAINER_NAME);
			foreach (string file in files)
			{
				try
				{
					//Read the file from blob storage
					MemoryStream memStream = ecommerceBlobStorage.ReturnFilesInContainer(ecommerceBlobStorage.IMPORT_AX_DEPOSITS_CONTAINER_NAME, file);
					//Insert file contents into datatable
					DataTable data = EcommerceImportCSVdata.GetDataTabletFromCSVFile(memStream, Table_Import_AX_Deposits.Name);
					//bulk import datatable into database import table
					//bool success = EcommerceImportCSVdata.InsertDataIntoSQLServerUsingSQLBulkCopy(data, Table_Import_AX_Deposits.Name);
					bool success = Table_Import_AX_Deposits.ImportData(data);
					if (success)
					{
						success = Table_Import_AX_Deposits.ProcessImportedData();
						//popup message that import was successful and move blob into successful blob container location
						ecommerceBlobStorage.ImportToStorage(ecommerceBlobStorage.PROCESSED_AX_DEPOSITS_CONTAINER_NAME, file, memStream);
						ecommerceBlobStorage.DeleteFromImportStorage(ecommerceBlobStorage.IMPORT_AX_DEPOSITS_CONTAINER_NAME, file);
					}
					else
					{
						//popup message that import of file failed
						//Please check your spreadsheet to ensure it is properly formated for import.
					}
				}
				catch (Exception ex)
				{
				}
			}
		}
		protected void ImportBTDisbursements()
		{ }
		protected void ImportBtTransactions()
		{ }


		protected void ImportAllFilesBtn_Click(object sender, EventArgs e)
		{
			ImportAllFiles();
		}
		protected void ImportAllFiles()
		{
			ImportAxDeposits();
			ImportBTDisbursements();
			ImportBtTransactions();
		}

		/// <summary>
		/// NOT USED - was for if using Azure Data Factory
		/// </summary>
		protected void Process_ImportUploadedFiles()
		{
			//EcommerceDataFactory edf = new EcommerceReconcileApp.EcommerceDataFactory();
			//edf.DoWork();
		}


	}


}