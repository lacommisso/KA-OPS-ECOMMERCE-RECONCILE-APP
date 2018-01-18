<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ProcessImportedFiles.aspx.cs" Inherits="EcommerceReconcileApp.ProcessImportedFiles" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
    
        <textarea id="AxDepositsTextArea" cols="20" name="AxDepositsTextArea" placeholder="test" rows="2">AxDeposit Files to be processed.</textarea>
        <asp:GridView ID="GridView1" runat="server">
        </asp:GridView>

<p /><p />
        <asp:GridView ID="GridView_AxDeposits" runat="server" AutoGenerateColumns="false" CellPadding="6">  
            <Columns>  
                <asp:BoundField DataField="CollectionName" HeaderText="CollectionName" />  
                <asp:BoundField DataField="CustomerAccount" HeaderText="CustomerAccount" />  
                <asp:BoundField DataField="DepositDate" HeaderText="DepositDate" />  
                <asp:BoundField DataField="PONumber" HeaderText="PONumber" />  
                <asp:BoundField DataField="Invoice" HeaderText="Invoice" />  
                <asp:BoundField DataField="AmountInTransactionCurrency" HeaderText="AmountInTransactionCurrency" />  
                <asp:BoundField DataField="Balance" HeaderText="Balance" />  
                <asp:BoundField DataField="Description" HeaderText="Description" />  
            </Columns>  
            <HeaderStyle BackColor="#0066cc" Font-Bold="true" ForeColor="White" />  
            <RowStyle BackColor="#bfdfff" ForeColor="Black" />  
        </asp:GridView>  
    </div>  
    

    </form>
</body>
</html>

