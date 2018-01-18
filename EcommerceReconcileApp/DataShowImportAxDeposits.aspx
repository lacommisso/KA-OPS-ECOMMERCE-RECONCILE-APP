<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="DataShowImportAxDeposits.aspx.cs" Inherits="EcommerceReconcileApp.DataShowImportAxDeposits" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <asp:Button ID="MainPageBtn" runat="server" OnClick="MainPageBtn_Click" Text="Go To Main Page" />
        <p />
        <p />If no data displayed, ensure datafiles were imported successfully.
            <asp:GridView ID="GridView_ImportAxDeposits" runat="server" AutoGenerateColumns="false" CellPadding="6" OnRowCommand="GridView_ImportAxDeposits_OnRowCommand">  
            <Columns>  
                <asp:BoundField DataField="CollectionName" HeaderText="CollectionName" />  
                <asp:BoundField DataField="CustomerAccount" HeaderText="CustomerAccount" />  
                <asp:BoundField DataField="DepositDate" HeaderText="DepositDate" /> 
                <asp:BoundField DataField="DueDate" HeaderText="DueDate" />  
                <asp:BoundField DataField="PONumber" HeaderText="PONumber" />  
                <asp:BoundField DataField="Invoice" HeaderText="Invoice" />  
                <asp:BoundField DataField="AmountInTransactionCurrency" HeaderText="AmountInTransactionCurrency" />  
                <asp:BoundField DataField="Balance" HeaderText="Balance" />  
                <asp:BoundField DataField="Description" HeaderText="Description" />  
                <asp:TemplateField ShowHeader="False">
                    <ItemTemplate>
                        <asp:Button ID="DeleteImportAxDepositsBtn" runat="server" CausesValidation="false" OnClientClick="return confirm('Are you sure you want to delete this row?')" CommandName="DeleteRow"
                            Text="DeleteRow" CommandArgument='<%#Eval("CollectionName") + ";" +Eval("CustomerAccount")+ ";" +Eval("DepositDate")+ ";"+Eval("DueDate")+ ";" +Eval("PONumber")+ ";" +Eval("Invoice") +";" +Eval("AmountInTransactionCurrency")+";"  +Eval("Balance")+";"+Eval("Description")%>' />
                    </ItemTemplate>
                </asp:TemplateField>
            </Columns>  
            <HeaderStyle BackColor="#0066cc" Font-Bold="true" ForeColor="White" />  
            <RowStyle BackColor="#bfdfff" ForeColor="Black" />  
        </asp:GridView> 
    </div>
    </form>
</body>
</html>
