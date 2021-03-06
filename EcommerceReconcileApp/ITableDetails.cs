﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using Microsoft.VisualBasic.FileIO;

namespace EcommerceReconcileApp
{
	public interface ITableDetails
	{
		string TableName{ get; }
		void TableDetails(ref DataColumnCollection columns, string tablename);

		DataTable GetCurrentData();

		void DeleteRow(string columnValues);
	}
}
