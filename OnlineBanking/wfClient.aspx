<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="wfClient.aspx.cs" Inherits="OnlineBanking.wfClient" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <p>
        <br />
        <asp:Label ID="lblClientName" runat="server"></asp:Label>
    </p>
    <p>
        <asp:GridView ID="gvClientAccounts" runat="server" AutoGenerateSelectButton="True" AutoGenerateColumns="False" OnSelectedIndexChanged="gvClientAccounts_SelectedIndexChanged" Width="393px">
            <Columns>
                <asp:BoundField DataField="AccountNumber" HeaderText="Account Number" />
                <asp:BoundField DataField="Notes" HeaderText="Account Notes" />
                <asp:BoundField DataField="Balance" DataFormatString="{0:c}" HeaderText="Balance">
                <ItemStyle HorizontalAlign="Right" />
                </asp:BoundField>
            </Columns>
        </asp:GridView>
    </p>
    <p>
        <asp:Label ID="lblErrorMessage" runat="server" Text="Label" Visible="False"></asp:Label>
    </p>
    <p>
    </p>
</asp:Content>
