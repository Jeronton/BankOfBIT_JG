<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="wfAccount.aspx.cs" Inherits="OnlineBanking.wfAccount" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <p>
        <br />
        <asp:Label ID="lblClientName" runat="server"></asp:Label>
    </p>
    <p>
        <asp:Label ID="Label1" runat="server" Text="Account Number: "></asp:Label>
        <asp:Label ID="lblAccountNumber" runat="server"></asp:Label>
        <asp:Label ID="Label2" runat="server" Text="Balance: "></asp:Label>
        <asp:Label ID="lblBalance" runat="server"></asp:Label>
    </p>
    <p>
        <asp:GridView ID="gvTransactions" runat="server" AutoGenerateColumns="False" Width="698px">
            <Columns>
                <asp:BoundField DataField="DateCreated" DataFormatString="{0:d}" HeaderText="Date" />
                <asp:BoundField DataField="TransactionType.Description" HeaderText="Transaction Type" />
                <asp:BoundField DataField="Deposit" DataFormatString="{0:c}" HeaderText="Amount In">
                <ItemStyle HorizontalAlign="Right" />
                </asp:BoundField>
                <asp:BoundField DataField="Withdrawal" DataFormatString="{0:c}" HeaderText="Amount Out">
                <ItemStyle HorizontalAlign="Right" />
                </asp:BoundField>
                <asp:BoundField DataField="Notes" HeaderText="Details" />
            </Columns>
        </asp:GridView>
    </p>
    <p>
        <asp:LinkButton ID="lbTransactions" runat="server" OnClick="lbTransactions_Click">Pay Bills and Transfer Funds </asp:LinkButton>
        <asp:LinkButton ID="lbAccountList" runat="server" OnClick="lbAccountList_Click">Return to Bank Account List</asp:LinkButton>
    </p>
    <p>
    </p>
    <p>
        <asp:Label ID="lblErrorMessage" runat="server" Visible="False"></asp:Label>
    </p>
</asp:Content>
