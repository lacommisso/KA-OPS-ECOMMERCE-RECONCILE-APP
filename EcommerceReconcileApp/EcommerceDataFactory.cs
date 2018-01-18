using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using Microsoft.Rest;
using Microsoft.Azure.Management.ResourceManager;
using Microsoft.Azure.Management.DataFactory;
using Microsoft.Azure.Management.DataFactory.Models;
using Microsoft.IdentityModel.Clients.ActiveDirectory;

namespace EcommerceReconcileApp
{

	public class EcommerceDataFactory
	{

		//Get from portal, Azure Active Directory, properties, Directory ID is your tenant ID  or
		//Get from ECOMMERCEDATAFACTORRY Settings form on portal
		private string tenantID = "d29b7a9b-6edb-4720-99a8-3c5c6c3eeeb0";
		private string applicationId = "d7ae1259-b4ff-40e0-b792-cfe518a74d52";
		private string authenticationKey = "0b9f4e69-205b-4b73-bb3d-d374f9cda2fd";
		private string subscriptionId = "63ab93d3-d9b5-47b4-a7ea-f7aeeaa68b7f";
		private string resourceGroup = "CLVNext-Dev";
		private string region = "EastUS";
		//V2 datafactory name
		private string dataFactoryName = "ECOMMERCEDATAFACTORY";
		private string storageAccount = "ecommercereconciliation";
		private string storageKey = "d3+n+9pem+3g03wJWxDqGh+ubiq6H76rFiOL+8Nhy19yMscv5bEZ0N0pGWKAJDc6za0EfUgIYI6aUO6KLAJY/w==";

		// specify the container and input folder from which all files need to be copied to the output folder. 
		private string inputBlobPath = "import-axdeposits";//the path to existing blob(s) to copy data from, e.g. containername/foldername>
		
		//specify the contains and output folder where the files are copied
		private string outputBlobPath = "axdeposits";

		private string storageLinkedServiceName = "AzureStorageLinkedService";  // name of the Azure Storage linked service
		private string blobDatasetName = "BlobDataset";             // name of the blob dataset
		private string pipelineName = "Adfv2QuickStartPipeline";    // name of the pipeline

		private string sqlDbLinkedServiceName = "";
		private string azureSqlConnString = "";

		public EcommerceDataFactory()
		{

		}

		/// <summary>
		/// Think these are all the steps needed to trigger the copy pipeline.  
		/// TODO: Need to debug this.
		/// </summary>
		public void DoWork()
		{
			bool status = false;

			DataFactoryManagementClient client = CreateDataFactoryClient();
			CreateDataFactory(client);
			CreateAzureStorageLinkedService(client);
			CreateAzureSQLDatabaseLinkedService(client);
			CreateDatasetSourceBlob(client);
			CreateDatasetSinkSQLDatabase(client);
			CreateCopyBlobToSQLPipeline(client);
			CreateRunResponse runResponse = this.TriggerPipeline(client);
			PipelineRun pipelineRun = this.MonitorTriggeredRun(client, runResponse);
			status = this.CheckTriggeredRunDetails(client, runResponse, pipelineRun);

		}

		public DataFactoryManagementClient CreateDataFactoryClient()
		{
			// Authenticate and create a data factory management client
			var context = new AuthenticationContext("https://login.windows.net/" + tenantID);
			ClientCredential cc = new ClientCredential(this.applicationId, authenticationKey);
			AuthenticationResult result = context.AcquireTokenAsync("https://management.azure.com/", cc).Result;
			ServiceClientCredentials cred = new TokenCredentials(result.AccessToken);
			DataFactoryManagementClient client = new DataFactoryManagementClient(cred) { SubscriptionId = this.subscriptionId };

			return client;
		}
		public bool CreateDataFactory(DataFactoryManagementClient client)
		{
			bool success = false;

			Factory dataFactory = new Factory
			{
				Location = region,
				Identity = new FactoryIdentity()

			};
			client.Factories.CreateOrUpdate(resourceGroup, dataFactoryName, dataFactory);
			Console.WriteLine(SafeJsonConvert.SerializeObject(dataFactory, client.SerializationSettings));

			while (client.Factories.Get(resourceGroup, dataFactoryName).ProvisioningState == "PendingCreation")
			{
				System.Threading.Thread.Sleep(1000);
			}
			success = true;
			return success;
		}

		public bool CreateAzureStorageLinkedService(DataFactoryManagementClient client)
		{
			// Create an Azure Storage linked service
			Console.WriteLine("Creating linked service " + storageLinkedServiceName + "...");

			LinkedServiceResource storageLinkedService = new LinkedServiceResource(
				new AzureStorageLinkedService
				{
					ConnectionString = new SecureString("DefaultEndpointsProtocol=https;AccountName=" + storageAccount + ";AccountKey=" + storageKey)
				}
			);
			client.LinkedServices.CreateOrUpdate(resourceGroup, dataFactoryName, storageLinkedServiceName, storageLinkedService);
			Console.WriteLine(SafeJsonConvert.SerializeObject(storageLinkedService, client.SerializationSettings));

			return true;
		}

		public bool CreateAzureSQLDatabaseLinkedService(DataFactoryManagementClient client)
		{
			// Create an Azure SQL Database linked service
			Console.WriteLine("Creating linked service " + sqlDbLinkedServiceName + "...");

			LinkedServiceResource sqlDbLinkedService = new LinkedServiceResource(
				new AzureSqlDatabaseLinkedService
				{
					ConnectionString = new SecureString(azureSqlConnString)
				}
			);
			client.LinkedServices.CreateOrUpdate(resourceGroup, dataFactoryName, sqlDbLinkedServiceName, sqlDbLinkedService);
			Console.WriteLine(SafeJsonConvert.SerializeObject(sqlDbLinkedService, client.SerializationSettings));

			return true;
		}

		public bool CreateDatasetSourceBlob(DataFactoryManagementClient client)
		{
		
			return true;
		}

		public bool CreateDatasetSinkSQLDatabase(DataFactoryManagementClient client)
		{

			return true;
		}

		public bool CreateCopyBlobToSQLPipeline(DataFactoryManagementClient client)
		{

			return true;
		}

		public CreateRunResponse TriggerPipeline(DataFactoryManagementClient client)
		{
			// Create a pipeline run
			Console.WriteLine("Creating pipeline run...");
			CreateRunResponse runResponse = client.Pipelines.CreateRunWithHttpMessagesAsync(resourceGroup, dataFactoryName, pipelineName).Result.Body;
			Console.WriteLine("Pipeline run ID: " + runResponse.RunId);
			
			return runResponse;
		}

		public PipelineRun MonitorTriggeredRun(DataFactoryManagementClient client, CreateRunResponse runResponse)
		{
			// Monitor the pipeline run
			Console.WriteLine("Checking pipeline run status...");
			PipelineRun pipelineRun;
			while (true)
			{
				pipelineRun = client.PipelineRuns.Get(resourceGroup, dataFactoryName, runResponse.RunId);
				Console.WriteLine("Status: " + pipelineRun.Status);
				if (pipelineRun.Status == "InProgress")
					System.Threading.Thread.Sleep(15000);
				else
					break;
			}

			return pipelineRun;
		}

		public bool CheckTriggeredRunDetails(DataFactoryManagementClient client, CreateRunResponse runResponse, PipelineRun pipelineRun)
		{
			// Check the copy activity run details
			Console.WriteLine("Checking copy activity run details...");

			List<ActivityRun> activityRuns = client.ActivityRuns.ListByPipelineRun(
			resourceGroup, dataFactoryName, runResponse.RunId, DateTime.UtcNow.AddMinutes(-10), DateTime.UtcNow.AddMinutes(10)).ToList();


			if (pipelineRun.Status == "Succeeded")
			{
				Console.WriteLine(activityRuns.First().Output);
			}
			else
				Console.WriteLine(activityRuns.First().Error);

			Console.WriteLine("\nPress any key to exit...");
			Console.ReadKey();
			return true;
		}
	}
	
}