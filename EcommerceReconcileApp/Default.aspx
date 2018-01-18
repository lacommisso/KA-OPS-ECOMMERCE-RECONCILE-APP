<%@ Page Title="Home Page" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="EcommerceReconcileApp._Default" %>
<script runat="server">

    void Upload_Click(Object sender, EventArgs e)
    {

        AxDepositSpan.InnerHtml = " ";
        BTTransactionSpan.InnerHtml = " ";
        BTDisbursementsSpan.InnerHtml = " ";

        //Verify that AX Deposits file has been specified and upload it
        if ((AXDepositeFileId.PostedFile != null) && ((AXDepositeFileId.PostedFile.FileName != null) && (AXDepositeFileId.PostedFile.FileName != "") )  ) {
            try {
                bool success = UploadToStorage(BlobStorage.IMPORT_AX_DEPOSITS_CONTAINER_NAME, AXDepositeFileId.PostedFile);
                if (success)
                {
                    UpdateAxDeposit_gridview();
                }
                else
                    AxDepositSpan.InnerHtml = "Upload Failed!";
            }
            catch (Exception ex) {
                string exStr = " ";
                if (ex != null)
                    exStr =ex.ToString();
                AxDepositSpan.InnerHtml = "Error uploading file <b>" +
                   AXDepositeFileId.PostedFile.FileName + "</b><br>" + exStr;
            }
        }

        //Verify that BT transactions file has been specified and upload it
        if ((btTransactionfileId.PostedFile != null) && ((btTransactionfileId.PostedFile.FileName != null) && (btTransactionfileId.PostedFile.FileName != "") ) ) {
            try {
                bool success = UploadToStorage(BlobStorage.IMPORT_BT_TRANSACTIONS_CONTAINER_NAME, btTransactionfileId.PostedFile);
                if (success)
                {
                    this.UpdateBtTransaction_gridview();
                }
                else
                    BTTransactionSpan.InnerHtml = "Upload Failed!";
            }
            catch (Exception ex) {
                string exStr = " ";
                if (ex != null)
                    exStr =ex.ToString();
                BTTransactionSpan.InnerHtml = "Error uploading file <b>" +
                   btTransactionfileId.PostedFile.FileName + "</b><br>" + exStr;
            }
        }

        //Verify that BT disbursements file has been specified and upload it
        if ((BTDisbursementsFileId.PostedFile != null) && ((BTDisbursementsFileId.PostedFile.FileName != null) && (BTDisbursementsFileId.PostedFile.FileName != "") ) ) {
            try {
                bool success = UploadToStorage(BlobStorage.IMPORT_BT_DISBURSEMENTS_CONTAINER_NAME, BTDisbursementsFileId.PostedFile);
                if (success)
                {
                    this.UpdateBTDisbursements_gridview();
                }
                else
                    BTDisbursementsSpan.InnerHtml = "Upload Failed!";
            }
            catch (Exception ex) {
                string exStr = " ";
                if (ex != null)
                    exStr =ex.ToString();
                BTDisbursementsSpan.InnerHtml = "Error uploading file <b>" +
                   BTDisbursementsFileId.PostedFile.FileName + "</b><br>" + exStr;
            }
        }

    }


</script>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server" >

    <div >
    <div class="container">
        <h2>Ecommerce Reconciliation Application</h2>
        <p />
        (1) Upload required files<p />
        (2) Import files into the database<p />
        (3) Process imported data (bring into the main database tables)<p />
        <p class="lead">
        </p>
        <asp:Table GridLines="Both" BorderWidth="2" ID="Table1" runat="server"  BorderColor="Black" Width="1500px" BorderStyle="Solid" CellPadding="1" CellSpacing="1">
            <asp:TableHeaderRow BackColor="#3399FF">
                <asp:TableCell width="100px">File Type</asp:TableCell>
                <asp:TableCell width="200px">Upload</asp:TableCell>
                <asp:TableCell width="400px">Files Uploaded</asp:TableCell>
                <asp:TableCell width="400px">Import to DB</asp:TableCell>
                <asp:TableCell width="400px">Files Successfully Imported</asp:TableCell>
                <asp:TableCell width="200px">Initiate Reconciliation</asp:TableCell>
            </asp:TableHeaderRow>
            <asp:TableRow BackColor="#CCCCCC" >
                <asp:TableCell width="100px">AX Deposits </asp:TableCell>
                <asp:TableCell width="200px">
                    <input id="AXDepositeFileId" type="file" runat="Server" OnChange="AxFileChange"/>                 
                    <p></p>
                </asp:TableCell>
                <asp:TableCell width="400px"> 
                     <span id="AxDepositSpan" runat="Server" /> 
                    <p>
                        <br />
                        <asp:GridView ID="AxDepositGridView" runat="server" AutoGenerateColumns="True" ShowHeader="false" BorderStyle="None" EmptyDataText="No files uploaded." OnSelectedIndexChanged="AxDepositGridView_SelectedIndexChanged" GridLines="None">
                        </asp:GridView>
                    </p>    
                </asp:TableCell>
                <asp:TableCell width="400px">
                    <asp:Button ID="ImportAxDepositBtn" runat="server" OnClick="ImportAxDepositBtn_Click" Text="Import AxDeposits" />
                    <p />
                    <asp:Button ID="ViewImportAxDataBtn" runat="server" OnClick="ViewImportAxDataBtn_Click" Text="View Imported Data" />
                    <p />
                    <asp:Button ID="ViewAxDataBtn" runat="server" OnClick="ViewAxDataBtn_Click" Text="View AX Deposits" />
                </asp:TableCell>
                <asp:TableCell width="400px"><asp:Button ID="ProcessAxDepositBtn" runat="server" OnClick="ProcessAxDepositBtn_Click" Text="Process AxDeposits" /></asp:TableCell>
                <asp:TableCell width="200px"></asp:TableCell>
            </asp:TableRow>

            <asp:TableRow >
                <asp:TableCell  width="100px">BT Disbursed Transactions  </asp:TableCell>
                <asp:TableCell  width="200px">
                    <input id="btTransactionfileId" type="file" runat="Server" />
                    <p></p>
                </asp:TableCell>
                <asp:TableCell  width="400px"> 
                     <span id="BTTransactionSpan" runat="Server" /> 
                    <p>
                        <br />
                        <asp:GridView ID="btTransactionGridView" runat="server" AutoGenerateColumns="True" ShowHeader="false" BorderStyle="None" EmptyDataText="No files uploaded." OnSelectedIndexChanged="btTransactionGridView_SelectedIndexChanged"  GridLines="None">
                        </asp:GridView>
                    </p>    
                </asp:TableCell>
                <asp:TableCell width="400px"></asp:TableCell>
                <asp:TableCell width="400px"><asp:Button ID="ProcessBtTransaction" runat="server" OnClick="ProcessBtTransactionBtn_Click" Text="Process BtTransactions" /></asp:TableCell>
                <asp:TableCell width="200px"></asp:TableCell>
            </asp:TableRow>

            <asp:TableRow BackColor="#CCCCCC" >
                <asp:TableCell width="100px">BT Disbursements</asp:TableCell>
                <asp:TableCell width="200px">
                    <input id="BTDisbursementsFileId" type="file" runat="Server" />
                    <p></p>
                </asp:TableCell>
                <asp:TableCell width="400px">
                    <span id="BTDisbursementsSpan" runat="Server" /> 
                    <p>
                        <br />
                        <asp:GridView ID="BTDisbursementsGridView" runat="server" AutoGenerateColumns="True" ShowHeader="false" BorderStyle="None" EmptyDataText="No files uploaded." OnSelectedIndexChanged="BTDisbursementsGridView_SelectedIndexChanged" GridLines="None">
                        </asp:GridView>
                    </p>    
                </asp:TableCell>
                <asp:TableCell width="400px"></asp:TableCell>
                <asp:TableCell width="400px"><asp:Button ID="ProcessBtDisbursements" runat="server" OnClick="ProcessBtDisbursementsBtn_Click" Text="Process BtDisbursements" /></asp:TableCell>
                <asp:TableCell width="200px"></asp:TableCell>
            </asp:TableRow>

            <asp:TableRow  >
                <asp:TableCell width="100px"></asp:TableCell>
                <asp:TableCell width="200px">
                    <input id="UploadBtn" type="button" value="Upload ALL Files" 
                         runat="Server" onserverclick="Upload_Click" />
                </asp:TableCell>
                <asp:TableCell width="400px">
                    <asp:Button ID="DeleteUploadedBlobsBtn" runat="server" OnClick="DeleteUploadedBlobsBtn_Click" Text="DeleteUploadedBlobs" />
                </asp:TableCell>
                <asp:TableCell width="400px">
                    <asp:Button ID="ImportAllBtn" runat="server" OnClick="ImportAllFilesBtn_Click" Text="Import ALL Uploaded Files" />
                </asp:TableCell>
                <asp:TableCell width="400px"></asp:TableCell>
                <asp:TableCell width="200px"></asp:TableCell>
            </asp:TableRow>

        </asp:Table>


    </div>
    </div>

</asp:Content>
