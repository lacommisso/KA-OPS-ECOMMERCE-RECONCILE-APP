<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="DataShowAxDeposits.aspx.cs" Inherits="EcommerceReconcileApp.DataShowAxDeposits" %>

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
        <p />Data from AX Deposits.
            <asp:GridView ID="GridView_AxDeposits" runat="server" AutoGenerateColumns="false" CellPadding="6" OnRowCommand="GridView_AxDeposits_OnRowCommand">  
            <Columns>  
                <asp:BoundField DataField="CollectionName" HeaderText="CollectionName" />  
                <asp:BoundField DataField="CustomerAccount" HeaderText="CustomerAccount" />  
                <asp:BoundField DataField="DepositDate" HeaderText="DepositDate" />  
                <asp:BoundField DataField="PONumber" HeaderText="PONumber" />  
                <asp:BoundField DataField="Invoice" HeaderText="Invoice" />  
                <asp:BoundField DataField="AmountInTransactionCurrency" HeaderText="AmountInTransactionCurrency" />  
                <asp:BoundField DataField="Balance" HeaderText="Balance" />  
                <asp:BoundField DataField="Description" HeaderText="Description" />  
                <asp:TemplateField ShowHeader="False">
                    <ItemTemplate>
                        <asp:Button ID="DeleteAxDepositsBtn" runat="server" CausesValidation="false" OnClientClick="return confirm('Are you sure you want to delete this row?')" CommandName="DeleteRow"
                            Text="DeleteRow" CommandArgument='<%#Eval("ID")%>' />
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
