using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;

namespace EcommerceReconcileApp
{
	public partial class ProcessImportedFiles : System.Web.UI.Page
	{
		protected void Page_Load(object sender, EventArgs e)
		{
			if (!IsPostBack)
			{
				string[] filePaths = new string[] { "one", "two", "three" };
				List<ListItem> files = new List<ListItem>();
				foreach (string filePath in filePaths)
				{
					files.Add(new ListItem(filePath));
				}
				GridView1.DataSource = files;
				GridView1.DataBind();


				ShowData();
			}
		}


		//getting Connection String from Web.config file  
		string connectionStr = "Data Source=tcp:irisdwhserver.database.windows.net,1433;Initial Catalog=TestEcommerceReconciliationData;Integrated Security=False;User ID=irisadmin@irisdwhserver.database.windows.net;Password=Rbm101Development;Connect Timeout=30;Encrypt=True";
		//Method for DataBinding  
		protected void ShowData()
		{
			DataTable dt = new DataTable();
			SqlConnection con = new SqlConnection(connectionStr);
			SqlDataAdapter adapt = new SqlDataAdapter("select * from Import_AX_DEPOSITS", con);
			con.Open();
			adapt.Fill(dt);
			con.Close();
			if (dt.Rows.Count > 0)
			{
				GridView_AxDeposits.DataSource = dt;
				GridView_AxDeposits.DataBind();
			}
		}
	}
}