using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.IO;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;

namespace EcommerceReconcileApp
{
	public class EcommerceBlobStorage
	{

		/// <summary>
		/// Following storage containers are the import storage containers
		/// </summary>
		public string IMPORT_AX_DEPOSITS_CONTAINER_NAME { get { return "import-axdeposits"; } }
		public string IMPORT_BT_DISBURSEMENTS_CONTAINER_NAME { get { return "import-btdisbursements"; } }
		public string IMPORT_BT_TRANSACTIONS_CONTAINER_NAME { get { return "import-bttransactions"; } }

		private System.Web.SessionState.HttpSessionState session = null;
		public static string BlobStorage = "BlobStorage";


		/// <summary>
		/// Following are processed storage containers
		/// </summary>
		public string PROCESSED_AX_DEPOSITS_CONTAINER_NAME { get { return "processed-axdeposits"; } }
		public string PROCESSED_BT_DISBURSEMENTS_CONTAINER_NAME { get { return "processed-btdisbursements"; } }
		public string PROCESSED_BT_TRANSACTIONS_CONTAINER_NAME { get { return "processed-bttransactions"; } }


		//TODO: Need dictionary of files that were uploaded to imported blob storage container so they can be either moved or deleted 
		//after successful/failed import into sql database
		//Dictionary where key is container and value is List of files to be or that has been uploaded in this session
		Dictionary<string, List<string>> containerDictionary = new Dictionary<string, List<string>>();

		public EcommerceBlobStorage(System.Web.SessionState.HttpSessionState sessionState)
		{
			session = sessionState;  //keep reference to current sessionState to save object upon additions
		}

		private CloudStorageAccount GetEcommerceStorageAccount()
		{
			CloudStorageAccount account = null;
			try
			{
				string StorageAccountName = "ecommercereconciliation";
				string AccountKey = "d3+n+9pem+3g03wJWxDqGh+ubiq6H76rFiOL+8Nhy19yMscv5bEZ0N0pGWKAJDc6za0EfUgIYI6aUO6KLAJY/w==";

				string connectionString = String.Format("DefaultEndpointsProtocol=https;AccountName={0};AccountKey={1}", StorageAccountName, AccountKey);
				account = CloudStorageAccount.Parse(connectionString);
			}
			catch (Exception ex)
			{
				//log ex
				return null;
			}
			return account;
		}
		private CloudBlobContainer GetEcommerceStorageContainer(string containerName)
		{
			CloudBlobContainer blobContainer = null;
			try
			{
				CloudStorageAccount Account = GetEcommerceStorageAccount();
				CloudBlobClient _blobClient = null; ;
				if (_blobClient == null)
				{
					if (_blobClient == null)
					{
						_blobClient = Account.CreateCloudBlobClient();
					}
				}
				blobContainer = _blobClient.GetContainerReference(containerName);
			}catch(Exception ex)
			{
			}
			return blobContainer;
		}



		public bool ImportToStorage(string containerName, HttpPostedFile file)
		{
			try
			{
				CloudBlobContainer blobContainer = GetEcommerceStorageContainer(containerName);
				blobContainer.CreateIfNotExists(BlobContainerPublicAccessType.Blob);

				// write a blob to the container
				CloudBlockBlob blob = blobContainer.GetBlockBlobReference(file.FileName);

				blob.UploadFromStream(file.InputStream);
				//TODO: if successful, then add this filename to the dictionary for this storage account...or maybe can just delete all blobs in each import container
				this.Add(containerName, file.FileName);
				session[BlobStorage] = this;
			}
			catch (Exception ex)
			{
				return false;
			}
			return true;
		}

		public bool ImportToStorage(string containerName, string filename, MemoryStream file)
		{
			try
			{
				CloudBlobContainer blobContainer = GetEcommerceStorageContainer(containerName);
				blobContainer.CreateIfNotExists(BlobContainerPublicAccessType.Blob);

				// write a blob to the container
				CloudBlockBlob blob = blobContainer.GetBlockBlobReference(filename);

				blob.UploadFromStream(file);
				//TODO: if successful, then add this filename to the dictionary for this storage account...or maybe can just delete all blobs in each import container
				this.Add(containerName, filename);
				session[BlobStorage] = this;
			}
			catch (Exception ex)
			{
				return false;
			}
			return true;
		}
		private void Add(string containerName, string filename)
		{
			if (containerDictionary==null)
				containerDictionary = new Dictionary<string, List<string>>();

			List<string> files;
			if (containerDictionary.TryGetValue(containerName, out files))
			{
				containerDictionary.Remove(containerName);
			}
			if (files==null)
				files = new List<string>();
			files.Add(filename);
			containerDictionary.Add(containerName, files);

			session.Add(EcommerceBlobStorage.BlobStorage, containerDictionary ); //Save
		}


		public bool DeleteFromImportStorage(string containerName, string filename)
		{
			if (String.IsNullOrWhiteSpace(filename))
				return false;

			//TODO:  Can we just delete all files in this container...import container rather than send in filename
			int count = containerDictionary.Count;
			try
			{
				CloudBlobContainer blobContainer = GetEcommerceStorageContainer(containerName);
				blobContainer.CreateIfNotExists(BlobContainerPublicAccessType.Blob);
				ICloudBlob blob = blobContainer.GetBlobReferenceFromServer(filename, null, null, null);
				blob.DeleteIfExists();
				session[BlobStorage] = this;
			}
			catch (Exception ex)
			{
				return false;
			}
			return true;
		}

		public bool DeleteAllBlobsInContainer(string containerName)
		{
			bool success = false;
			try
			{
				CloudBlobContainer blobContainer = GetEcommerceStorageContainer(containerName);
				BlobContinuationToken token = new BlobContinuationToken();
				IEnumerable<IListBlobItem> bloblist = null;
				do
				{
					BlobResultSegment segment = blobContainer.ListBlobsSegmented(string.Empty, true, BlobListingDetails.Metadata, 5000, token, null, null);
					bloblist = segment.Results.OfType<CloudBlockBlob>().OrderBy(m => m.Properties.LastModified);
					token = segment.ContinuationToken;
					foreach (IListBlobItem item in bloblist)
					{
						((CloudBlockBlob)item).FetchAttributes();
						//Console.WriteLine(i.ToString() + "  last modified=" + strLastModifiedDate + " blobname= " + ((CloudBlockBlob)item).Name);
						((CloudBlockBlob)item).DeleteIfExists();
					}

				} while (token != null);
			}catch (Exception ex)
			{
				success = false;
				return success;
			}
			session[BlobStorage] = this;
			return success;
		}

		public List<string> ReturnFilesInContainer(string containerName)
		{
			List<string> files = new List<string>();
			try
			{
				CloudBlobContainer blobContainer = GetEcommerceStorageContainer(containerName);
				BlobContinuationToken token = new BlobContinuationToken();
				IEnumerable<IListBlobItem> bloblist = null;
				do
				{
					BlobResultSegment segment = blobContainer.ListBlobsSegmented(string.Empty, true, BlobListingDetails.Metadata, 5000, token, null, null);
					bloblist = segment.Results.OfType<CloudBlockBlob>().OrderBy(m => m.Properties.LastModified);
					token = segment.ContinuationToken;
					foreach (IListBlobItem item in bloblist)
					{
						((CloudBlockBlob)item).FetchAttributes();
						files.Add(((CloudBlockBlob)item).Name);
					}

				} while (token != null);
			}
			catch (Exception ex)
			{
				return null;
			}
			return files;
		}


		public MemoryStream ReturnFilesInContainer(string containerName, string filename)
		{
			MemoryStream memStream = new MemoryStream();
			try
			{
				CloudBlobContainer blobContainer = GetEcommerceStorageContainer(containerName);
				BlobContinuationToken token = new BlobContinuationToken();
				IEnumerable<IListBlobItem> bloblist = null;
				bool found = false;

				do
				{
					BlobResultSegment segment = blobContainer.ListBlobsSegmented(string.Empty, true, BlobListingDetails.Metadata, 5000, token, null, null);
					bloblist = segment.Results.OfType<CloudBlockBlob>().OrderBy(m => m.Properties.LastModified);
					token = segment.ContinuationToken;
					
					foreach (IListBlobItem item in bloblist)
					{
						((CloudBlockBlob)item).FetchAttributes();
						if (filename.Equals(((CloudBlockBlob)item).Name))
						{
							found = true;   //found the file
							((CloudBlockBlob)item).DownloadToStream(memStream, null, null, null);
							memStream.Seek(0, SeekOrigin.Begin);
							break;
						}
					}

				} while ((token != null) && (!found));
			}
			catch (Exception ex)
			{
				return null;
			}
			return memStream;
		}

	}
}