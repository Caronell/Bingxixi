<%@ Control Language="C#" AutoEventWireup="true" CodeFile="ShoppingCartPanel.ascx.cs" Inherits="ShoppingCartPanel" %>

<asp:Panel ID="top_blackpanel" runat="server" CssClass="top_shadow" Visible="False"></asp:Panel>
<asp:Panel ID="cart_panel" runat="server" CssClass="top_panel" Visible="False">
    <asp:Button ID="CloseBtn" runat="server" Text="" CssClass="close_btn" OnClick="CloseCart_Click" />
    <div class="cart_caption">
        —— 购物车 ——
    </div>
    <div class="cart_table_title">
        <div class="cart_title_item" style="width:40%;">
            书名
        </div>
        <div class="cart_title_item" style="width:calc(15% - 3px);border-left:none;">
            单价
        </div>
        <div class="cart_title_item" style="width:calc(15% - 3px);border-left:none;">
            数量
        </div>
        <div class="cart_title_item" style="width:30%;border-left:none;">
            操作
        </div>
    </div>
    <asp:Panel ID="Panel1" runat="server" ScrollBars="Auto" CssClass="cart_table">
        <asp:Table ID="cart_Table" runat="server" style="width:100%;">

        </asp:Table>
    </asp:Panel>
    <asp:Label ID="TotPrice" runat="server" Text="总价：0元" CssClass="tot_price"></asp:Label>
    <asp:Button ID="BillSelect" runat="server" Text="结算所选项" CssClass="bill_select_btn" OnClick="BillSelectBtn_Click" />
    <asp:Button ID="BuynowBtn" runat="server" Text="结算全部" CssClass="bill_btn" OnClick="BillBtn_Click" />
    <asp:Button ID="Button1" runat="server" Text="删除所选项" CssClass="clear_select_btn" OnClick="ClearSelectBtn_Click" />
    <asp:Button ID="ClearAll" runat="server" Text="清空购物车" CssClass="clear_btn" OnClick="ClearCartBtn_Click" />
</asp:Panel>
