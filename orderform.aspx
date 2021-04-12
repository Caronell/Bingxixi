<%@ Page Language="C#" AutoEventWireup="true" CodeFile="orderform.aspx.cs" Inherits="orderform" %>

<%@ Register Src="ShoppingCartPanel.ascx" TagName="ShoppingCartPanel" TagPrefix="Cart" %>
<%@ Register Src="OrderView.ascx" TagName="OrderView" TagPrefix="Ov" %>


<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title>我的订单</title>
    <link rel="stylesheet" type="text/css" href="App_CSS/CommonCSS.css" />
    <link rel="stylesheet" type="text/css" href="App_CSS/ScrollToTop.css" />
    <link rel="stylesheet" type="text/css" href="App_CSS/orderformCSS.css" />
    <script type="text/javascript" src="App_JS/jquery-1.11.0.min.js"></script>
    <script type="text/javascript" src="App_JS/jquery-2.1.4-min.js"></script>
    <script type="text/javascript" src="App_JS/ScrollToTop.js"></script>
    <script type="text/javascript" src="App_JS/jquery-animate-css-rotate-scale.js"></script>
</head>
<body>
    <form id="form1" runat="server">
        <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
        <Cart:ShoppingCartPanel ID="CartPanel" runat="server" Visible="false"/>
        <a href="#0" class="cd-top" title="返回顶部">Top</a>
        <div class="header_box">
            <div class="header_logo">
                <a href="bingxixi.aspx">并夕夕图书</a>
            </div>
            <div class="header_search_box">
                <div class="search">
                    <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                        <ContentTemplate>
                            <asp:TextBox ID="SearchText" runat="server" CssClass="SearchText" placeholder="三体" AutoPostBack="True" OnTextChanged="SearchText_TextChanged"></asp:TextBox>
                            <asp:Button ID="SearchBtn" runat="server" CssClass="SearchBtn" />
                        </ContentTemplate>
                        <Triggers>
                            <asp:PostBackTrigger ControlID="SearchBtn" />
                        </Triggers>
                    </asp:UpdatePanel>
                </div>
                <div class="hot_search">
                    <asp:Panel ID="HotContains" runat="server" CssClass="hot_contains">
                        <asp:Label ID="rs_title" runat="server" Text="热搜：" CssClass="hot_title"></asp:Label>
                        <asp:Label ID="rs1" runat="server" Text="三体：黑暗森林" CssClass="hot_item"></asp:Label>
                        <asp:Label ID="rs2" runat="server" Text="堂吉诃德" CssClass="hot_item"></asp:Label>
                        <asp:Label ID="rs3" runat="server" Text="地心游记" CssClass="hot_item"></asp:Label>
                    </asp:Panel>
                </div>
            </div>
            <div class="cart_box">
                <div class="cart_contains">
                    <asp:Button ID="cart_btn" runat="server" Text="购物车" CssClass="cart_btn" OnClick="Showcart_Click"/>
                    <asp:Button ID="order_btn" runat="server" Text="我的订单" CssClass="ordet_btn" />
                </div>
            </div>
        </div>
        <asp:Panel ID="MainPanel" runat="server" CssClass="main" ScrollBars="Auto">
            
        </asp:Panel>
        <div class="bottom">
            <div class="bottom_con">
                Copyleft (C) 不存在的并夕夕图书 9999-9999, All Rights Don't Reserved 京ICP证114514号 没有出版物经营许可证 新出发京批字第直1919号
            </div>
        </div>
    </form>
</body>
</html>
