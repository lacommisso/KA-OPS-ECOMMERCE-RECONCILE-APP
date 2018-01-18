using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

namespace EcommerceReconcileApp
{
	public partial class DataShowAxDeposits : System.Web.UI.Page
	{
		protected void Page_Load(object sender, EventArgs e)
		{
			if (!IsPostBack)
			{
				ShowData();
			}
		}

		protected void GridView_AxDeposits_OnRowCommand(object sender, GridViewCommandEventArgs e)
		{
			if (e.CommandName != "DeleteRow") return;
			// do something
			string[] arg = new string[2];
			arg = e.CommandArgument.ToString().Split(';');
			Table_AX_Deposits.AxDeposits.DeleteRow((string)e.CommandArgument);
			//Session["IdTemplate"] = arg[0];
			//Session["IdEntity"] = arg[1];
		}

		//Method for DataBinding  
		protected void ShowData()
		{
			DataTable dt = Table_AX_Deposits.AxDeposits.GetCurrentData();
			if (dt.Rows.Count > 0)
			{
				GridView_AxDeposits.DataSource = dt;
				GridView_AxDeposits.DataBind();
			}
		}

		protected void MainPageBtn_Click(object sender, EventArgs e)
		{
			Response.Redirect("Default.aspx");
		}
	}
}
