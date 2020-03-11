<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="wfTransaction.aspx.cs" Inherits="OnlineBanking.wfTransaction" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <p>
    <br />
    <asp:Label ID="Label1" runat="server" Text="Account Number:  "></asp:Label>
    <asp:Label ID="lblAccountNumber" runat="server"></asp:Label>
</p>
<p>
    <asp:Label ID="Label2" runat="server" Text="Balance:  "></asp:Label>
    <asp:Label ID="lblBalance" runat="server"></asp:Label>
</p>
<p>
    <asp:Label ID="Label3" runat="server" Text="Transaction Type:  "></asp:Label>
    <asp:DropDownList ID="ddlTransactionType" runat="server" AutoPostBack="True" OnSelectedIndexChanged="ddlTransactionType_SelectedIndexChanged">
    </asp:DropDownList>
</p>
<p>
    <asp:Label ID="Label4" runat="server" Text="Amount:  "></asp:Label>
    <asp:TextBox ID="txtAmount" runat="server"></asp:TextBox>
    <asp:RangeValidator ID="rvAmount" runat="server" ControlToValidate="txtAmount" ErrorMessage="Amount must be greater than 0 and less than 10,000" MaximumValue="10000" MinimumValue="0.01" Type="Double"></asp:RangeValidator>
</p>
<p>
    <asp:Label ID="Label5" runat="server" Text="To:  "></asp:Label>
    <asp:DropDownList ID="ddlRecipient" runat="server" AutoPostBack="True">
    </asp:DropDownList>
</p>
<p>
    <asp:LinkButton ID="lbCompleteTransaction" runat="server" OnClick="lbCompleteTransaction_Click">Complete Transaction</asp:LinkButton>
    <asp:LinkButton ID="lbwfAccount" runat="server" OnClick="lbwfAccount_Click">Return to Account History</asp:LinkButton>
</p>
<p>
    &nbsp;</p>
<p>
    <asp:Label ID="lblError" runat="server" Visible="False"></asp:Label>
</p>
</asp:Content>
