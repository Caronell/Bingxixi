<%@ Control Language="C#" AutoEventWireup="true" CodeFile="BookView.ascx.cs" Inherits="BookView" %>

<div class="book_item">
    <asp:ImageButton ID="bookimg" runat="server" CssClass="book_img" ImageUrl="~/book_pic/1.jpg" PostBackUrl="~/bookpage.aspx" />
    <div class="book_info">
        <div class="book_name">
            <asp:Label ID="realBookname" runat="server" Text="Label" Visible="False"></asp:Label>
            <asp:HyperLink ID="bookname" runat="server" CssClass="bname" NavigateUrl="~/bookpage.aspx" Text="我是猫"></asp:HyperLink>
        </div>
        <div class="book_intro">
            <asp:Label ID="bookauthor" runat="server" Text="作者：〔日〕夏目漱石著汤丽珍译"></asp:Label>
        </div>
        <div class="book_intro" style="margin-top:5px;">
            <asp:Label ID="bookpublish" runat="server" Text="出版社：时代文艺出版社　　　出版时间：2019-11-13"></asp:Label>
        </div>
        <div class="book_intro_main">
            <asp:Label ID="bookintro" runat="server" Text="《我是猫》文如其名，话语权全在一只傲娇、毒舌的萌猫手里，管它是不是衣食父母、家财万贯，在本猫面前，皆剥去其体面衣裳，原形毕露：英语教师苦沙弥每日也是一读书就打瞌睡、美学家迷亭耍人大业日渐红火、大学士寒月恋情一波三折、哲学家独仙把人渡进精神病院、资本家金田阴人不断奇招百出……"></asp:Label>
        </div>
        <div class="book_price">
            <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                <ContentTemplate>
                    <asp:Button ID="addToCartBtn" runat="server" Text="加入购物车" CssClass="addtocart_btn" OnClick="Addtocart_Click" />
                </ContentTemplate>
                <Triggers>
                    <asp:AsyncPostBackTrigger ControlID="addToCartBtn" />
                </Triggers>
            </asp:UpdatePanel>
            <asp:Label ID="bookprice" runat="server" Text="￥45.00" CssClass="price"></asp:Label>
            <asp:Label ID="Label1" runat="server" Text="价格：" CssClass="price_tip"></asp:Label>
        </div>
    </div>
</div>
