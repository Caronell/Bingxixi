<%@ Page Language="C#" AutoEventWireup="true" CodeFile="bookpage.aspx.cs" Inherits="bookpage" %>

<%@ Register Src="ShoppingCartPanel.ascx" TagName="ShoppingCartPanel" TagPrefix="Cart" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>图书详情</title>
    <link rel="stylesheet" type="text/css" href="App_CSS/CommonCSS.css" />
    <link rel="stylesheet" type="text/css" href="App_CSS/BookPageCSS.css" />
    <script type="text/javascript" src="App_JS/jquery-1.11.0.min.js"></script>
    <script type="text/javascript" src="App_JS/jquery-2.1.4-min.js"></script>
    <script type="text/javascript" src="App_JS/BookPageJS.js"></script>
</head>
<body onmouseover="preTimer()">
    <form id="form1" runat="server">
        <asp:Label ID="bookid" runat="server" Text="Label" Visible="False"></asp:Label>
        <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
        <Cart:ShoppingCartPanel ID="CartPanel" runat="server" Visible="false"/>
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
        <div class="main">
            <div class="breadcrumb">
                <asp:HyperLink ID="home" runat="server" CssClass="linktext" Text="并夕夕图书" Style="font-weight: bold; font-size: medium;" NavigateUrl="~/bingxixi.aspx"></asp:HyperLink>
                <asp:Label ID="Label1" runat="server" Text=">" CssClass="linktext" Style="margin: 0 10px;"></asp:Label>
                <asp:HyperLink ID="Booktype" runat="server" CssClass="linktext" Text="世界名著"></asp:HyperLink>
                <asp:Label ID="Label2" runat="server" Text=">" CssClass="linktext" Style="margin: 0 10px;"></asp:Label>
                <asp:Label ID="Bookname" runat="server" Text="我是猫" CssClass="linktext"></asp:Label>
            </div>
            <div style="width: 100%; clear: both; min-width: 1200px;">
                <div class="left_box">
                    <div class="middle_pic_box" onmouseover="showZoom(event)">
                        <asp:Image ID="middlePic" runat="server" ImageUrl="~/book_pic/1.jpg" CssClass="middle_pic" />
                        <div id="zoom_pup" class="zoom_up_bg" onmousemove="moveZoomPic(event)" onmouseout="hideZoom()" onmouseover="stopTimer()">
                        </div>
                    </div>
                    <asp:Panel ID="bigPicBox" runat="server" CssClass="big_pic_box" Style="display: none;">
                        <asp:Image ID="bigPic" runat="server" ImageUrl="~/book_pic/1.jpg" CssClass="big_pic" Width="800" Height="800" Style="left: 0; top: 0;" />
                    </asp:Panel>
                </div>
                <div class="mid_box">
                    <div class="book_info_box">
                        <div style="margin-bottom: 12px; overflow:hidden; text-overflow:clip;">
                            <asp:Label ID="bookTitle" runat="server" Text="我是猫" Font-Size="Larger" Font-Bold="True"></asp:Label>
                        </div>
                        <div style="width: 100%; text-indent: 2em; border-bottom: solid 1px gainsboro; padding-bottom: 10px;">
                            <asp:Label ID="bookIntro" runat="server" Text="《我是猫》文如其名，话语权全在一只傲娇、毒舌的萌猫手里，管它是不是衣食父母、家财万贯，在本猫面前，皆剥去其体面衣裳，原形毕露：英语教师苦沙弥每日也是一读书就打瞌睡、美学家迷亭耍人大业日渐红火、大学士寒月恋情一波三折、哲学家独仙把人渡进精神病院、资本家金田阴人不断奇招百出……" ForeColor="#666666" Font-Size="Small"></asp:Label>
                        </div>
                        <div class="book_details_box">
                            <div class="book_details">
                                <div class="details_col">
                                    <asp:Label ID="bookAuthor" runat="server" Text="作者：〔日〕夏目漱石 著 汤丽珍 译" Font-Size="small"></asp:Label>
                                </div>
                                <div class="details_col">
                                    <asp:Label ID="bookPublisher" runat="server" Text="出版社：时代文艺出版社" Font-Size="small"></asp:Label>
                                </div>
                                <div class="details_col">
                                    <asp:Label ID="bookDate" runat="server" Text="出版时间：2019年11月" Font-Size="small"></asp:Label>
                                </div>
                                <div class="details_col">
                                    <asp:Label ID="bookSize" runat="server" Text="开 本：32开" Font-Size="small"></asp:Label>
                                </div>
                                <div class="details_col">
                                    <asp:Label ID="bookPage" runat="server" Text="纸 张：胶版纸" Font-Size="small"></asp:Label>
                                </div>
                                <div class="details_col">
                                    <asp:Label ID="isSuite" runat="server" Text="是否套装：否" Font-Size="small"></asp:Label>
                                </div>
                            </div>
                            <div class="buy_box">
                                <div style="font-size: large; color: black; margin-top: 5px;">
                                    价格：
                                </div>
                                <asp:Label ID="bookPrice" runat="server" Text="￥45.00" Font-Size="Larger" Font-Bold="True" ForeColor="#CC0000" Style="margin-left: 50px;"></asp:Label>
                                <br />
                                <div style="font-size: large; color: orangered; margin-top: 20px;">
                                    购买数量：
                                </div>
                                <div style="margin-top: 15px; margin-left: 30px;">
                                    <button class="modi_btn" type="button" style="border-radius: 15px 0 0 15px;" onclick="removeOneBook()">–</button>
                                    <asp:TextBox ID="buyNumber" runat="server" CssClass="buy_num" Text="1" onkeypress="if (event.keyCode<48 || event.keyCode>57) event.returnValue=false;" onblur="checkzero()"></asp:TextBox>
                                    <button class="modi_btn" type="button" style="border-radius: 0 15px 15px 0;" onclick="addOneBook()">＋</button>
                                </div>
                                <div style="clear: both; padding-left: 50px; padding-top: 8px;">
                                    <asp:Label ID="bookRemain" runat="server" Text="（库存 50 本）" Font-Size="Small"></asp:Label>
                                    <asp:HiddenField ID="remainBookNum" runat="server" />
                                </div>
                            </div>
                            <div class="action_box">
                                <asp:Button ID="BuyNow" runat="server" Text="立即购买" CssClass="buynow_btn" />
                                <asp:Button ID="AddToCart" runat="server" Text="加入购物车" CssClass="addto_cart_btn" OnClick="Addtocart_Click" />
                            </div>
                        </div>
                    </div>
                </div>
                <div class="right_box">
                    <asp:Image ID="receiveMoney" runat="server" ImageUrl="~/App_Pic/pi.jpg" Width="200" Height="200" Style="margin-top: 50px;" />
                    <p style="width: 100%; text-align: center; font-size: small">结账不发货</p>
                </div>
            </div>
        </div>
        <div class="bottom" style="bottom: 0; position: absolute; width: 100%;">
            <div class="bottom_con">
                Copyleft (C) 不存在的并夕夕图书 9999-9999, All Rights Don't Reserved 京ICP证114514号 没有出版物经营许可证 新出发京批字第直1919号
            </div>
        </div>
    </form>
</body>
</html>
