<%@ Control Language="C#" AutoEventWireup="true" CodeFile="OrderView.ascx.cs" Inherits="OrderView" %>

<asp:Panel ID="Panel1" runat="server" CssClass="order_item_box">
    <div class="date_box">
        <asp:Label ID="orderDate" runat="server" Text="Label" style="float:left; margin-right: 20px;"></asp:Label>
        <asp:Label ID="orderID" runat="server" Text="Label" style="float:left;"></asp:Label>
        <asp:Button ID="ExportBtn" runat="server" Text="" CssClass="export_btn" OnClick="ExportBtn_Click"/>
    </div>
    <asp:Table ID="OrderTable" runat="server" CssClass="order_table">
        
    </asp:Table>
    <div class="summary_box">
        <asp:Label ID="totmoney" runat="server" Text="Label" CssClass="summary_item" style="margin-left:10px;" Font-Bold="True"></asp:Label>
        <asp:Label ID="totcount" runat="server" Text="Label" CssClass="summary_item"></asp:Label>
    </div>
</asp:Panel>
