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
	public partial class DataShowImportAxDeposits : System.Web.UI.Page
	{
		protected void Page_Load(object sender, EventArgs e)
		{
			if (!IsPostBack)
			{
				ShowData();
			}
		}

		protected void GridView_ImportAxDeposits_OnRowCommand(object sender, GridViewCommandEventArgs e)
		{
			if (e.CommandName != "DeleteRow") return;
			//DateTime dt = Convert.ToDateTime(e.CommandArgument);
			//Decimal d = Convert.ToDecimal(e.CommandArgument);
			// do something
			string[] arg = new string[2];
			arg = e.CommandArgument.ToString().Split(';');
			Table_Import_AX_Deposits.ImportAxDeposits.DeleteRow((string)e.CommandArgument);
			//Session["IdTemplate"] = arg[0];
			//Session["IdEntity"] = arg[1];
		}


		//Method for DataBinding  
		protected void ShowData()
		{
			DataTable dt = Table_Import_AX_Deposits.ImportAxDeposits.GetCurrentData();
			if (dt.Rows.Count > 0)
			{
				GridView_ImportAxDeposits.DataSource = dt;
				GridView_ImportAxDeposits.DataBind();
			}
		}

		protected void MainPageBtn_Click(object sender, EventArgs e)
		{
			Response.Redirect("Default.aspx");
		}
	}
}