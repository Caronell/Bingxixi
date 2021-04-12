<%@ Page Language="C#" AutoEventWireup="true" CodeFile="bingxixi.aspx.cs" Inherits="_Default" %>

<%@ Register Src="BookView.ascx" TagName="BookView" TagPrefix="Bk" %>
<%@ Register Src="ShoppingCartPanel.ascx" TagName="ShoppingCartPanel" TagPrefix="Cart" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>并夕夕图书</title>
    <link rel="stylesheet" type="text/css" href="App_CSS/ShopPageCSS.css" />
    <link rel="stylesheet" type="text/css" href="App_CSS/CommonCSS.css" />
    <link rel="stylesheet" type="text/css" href="App_CSS/ScrollToTop.css" />
    <link rel="stylesheet" type="text/css" href="App_CSS/BookViewCSS.css" />
    <script type="text/javascript" src="App_JS/jquery-1.11.0.min.js"></script>
    <script type="text/javascript" src="App_JS/jquery-2.1.4-min.js"></script>
    <script type="text/javascript" src="App_JS/ScrollToTop.js"></script>
    <script type="text/javascript" src="App_JS/jquery-animate-css-rotate-scale.js"></script>
    <script type="text/javascript" src="App_JS/fly.js"></script>
    <script type="text/javascript" src="App_JS/ShopPageJS.js"></script>
</head>
<body>
    <form id="form1" runat="server">
        <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
        <Cart:ShoppingCartPanel ID="CartPanel" runat="server" Visible="false"/>
        <asp:Button ID="fixedCartBtn" runat="server" Text="" CssClass="fixed_cart_btn" OnClick="Showcart_Click" />
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
                    <asp:Button ID="cart_btn" runat="server" Text="购物车" CssClass="cart_btn" OnClick="Showcart_Click" />
                    <asp:Button ID="order_btn" runat="server" Text="我的订单" CssClass="ordet_btn" OnClick="MyOrderFormBtn_Click"/>
                </div>
            </div>
        </div>
        <div class="main">
            <div class="left_menu_box">
                <div class="menutitle">所有图书种类</div>
                <asp:Panel ID="LeftMenu" runat="server" CssClass="leftmenu">
                    <asp:Button ID="Test" runat="server" Text="科幻" CssClass="menuitem" />
                </asp:Panel>
            </div>
            <div class="middle_main_box">
                <div>
                    <%-- 经过测试，class名称带有ad大概率会被AdBlockPlus等插件拦截 --%>
                    <div id="box" class="big_commercial">
                        <ul class="list">
                            <li class="current" style="opacity: 1;">
                                <img src="commercial_pic/commad1.jpg" width="750" height="315" /></li>
                            <li style="opacity: 0;">
                                <img src="commercial_pic/commad2.jpg" width="750" height="315" /></li>
                            <li style="opacity: 0;">
                                <img src="commercial_pic/commad3.jpg" width="750" height="315" /></li>
                            <li style="opacity: 0;">
                                <img src="commercial_pic/commad4.jpg" width="750" height="315" /></li>
                            <li style="opacity: 0;">
                                <img src="commercial_pic/commad5.jpg" width="750" height="315" /></li>
                        </ul>
                        <ul class="count">
                            <li class="current">1</li>
                            <li class="">2</li>
                            <li class="">3</li>
                            <li class="">4</li>
                            <li class="">5</li>
                        </ul>
                    </div>
                    <div class="right_info_box">
                        <div class="infocard">
                            <div class="name">
                                <asp:Label ID="username" runat="server" Text="Caronell"></asp:Label>
                                <asp:Button ID="logoutBtn" runat="server" Text="退出登录" CssClass="logout_btn" OnClick="LogoutBtn_Click"/>
                                <asp:Button ID="loginBtn" runat="server" Text="点击登录" CssClass="logout_btn" OnClick="LoginBtn_Click"/>
                            </div>
                            <div class="avater_box">
                                <asp:Image ID="avater" runat="server" ImageUrl="~/App_Pic/default_userimg.jpg" CssClass="avater_pic" />
                            </div>
                            <div class="info">
                                <ul>
                                    <li>
                                        我的积分：
                                        <asp:Label ID="UserCredit" runat="server" Text="" Font-Bold="True" ForeColor="Red"></asp:Label>
                                    </li>
                                    <li>
                                        <asp:Button ID="EditUserDataBtn" runat="server" Text="修改个人信息" PostBackUrl="~/infopage.aspx" CssClass="edit_info_btn" />
                                    </li>
                                </ul>
                            </div>
                        </div>
                    </div>
                </div>
                <div class="book_list_box">
                    <div class="book_list_title">
                        <div class="title_tip">
                            每页最多显示10条
                        </div>
                        <div class="title_decorate"></div>
                        <div class="pager">
                            <asp:Button ID="prePage" runat="server" Text="«" CssClass="pagebtn" />
                            <asp:Label ID="pageNum" runat="server" Text="第 1 页" CssClass="pagetip"></asp:Label>
                            <asp:Button ID="nextPage" runat="server" Text="»" CssClass="pagebtn" />
                        </div>
                    </div>
                    <asp:Panel ID="BookList" runat="server" CssClass="book_list">
                        
                    </asp:Panel>
                    <div class="book_list_footer">
                        <div class="select_page">
                            <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                                <ContentTemplate>
                                    <asp:Button ID="jumpBtn" runat="server" Text="确定" CssClass="pagebtn" Style="color: black; float: right;" />
                                    <asp:Label ID="Label3" runat="server" Text="页" Font-Size="Small" Style="float: right; position: relative; margin-top: 15px;"></asp:Label>
                                    <asp:TextBox ID="targetPage" runat="server" CssClass="tarpage" onkeypress="if (event.keyCode<48 || event.keyCode>57) event.returnValue=false;" OnTextChanged="TargetPage_TextChanged" AutoPostBack="True"></asp:TextBox>
                                    <asp:Label ID="Label2" runat="server" Text="跳转到" Font-Size="Small" Style="float: right; position: relative; margin-top: 15px;"></asp:Label>
                                </ContentTemplate>
                                <Triggers>
                                    <asp:PostBackTrigger ControlID="jumpBtn" />
                                </Triggers>
                            </asp:UpdatePanel>
                        </div>
                        <div class="jump_to">
                            <asp:Button ID="prePageButtom" runat="server" Text="◀上一页" CssClass="pagebtn" />
                            <asp:Button ID="firstpage" runat="server" Text="1" CssClass="pagenumnow" Style="padding-right: 3px;" PostBackUrl="~/bingxixi.aspx?page=1" />
                            <asp:Panel ID="pagejump" runat="server" CssClass="pagejump">
                                <asp:Button ID="Button2" runat="server" Text="2" CssClass="pagenumbtn" />
                                <asp:Label ID="Label1" runat="server" Text="..." CssClass="pagenumelip"></asp:Label>
                                <asp:Button ID="Button3" runat="server" Text="16" CssClass="pagenumbtn" />
                                <asp:Button ID="endPage" runat="server" Text="17" CssClass="pagenumbtn" />
                            </asp:Panel>
                            <asp:Button ID="nextPageButtom" runat="server" Text="下一页▶" CssClass="pagebtn" Style="padding-left: 3px;" />
                        </div>
                    </div>
                </div>
            </div>
        </div>
        <div class="bottom">
            <div class="bottom_con">
                Copyleft (C) 不存在的并夕夕图书 9999-9999, All Rights Don't Reserved 京ICP证114514号 没有出版物经营许可证 新出发京批字第直1919号
            </div>
        </div>
        <script>
            var box = document.getElementById('box');
            var uls = document.getElementsByTagName('ul');
            var imgs = uls[0].getElementsByTagName('li');
            var btn = uls[1].getElementsByTagName('li');
            var i = index = 0;  //中间量，统一声明；
            var play = null;

            //图片切换
            function show(a)
            {   //方法定义的是当传入一个下标时，按钮和图片做出对的反应
                for (i = 0; i < btn.length; i++)
                {
                    btn[i].className = '';
                    btn[a].className = 'current';
                }
                for (i = 0; i < imgs.length; i++)
                {   //把图片的效果设置和按钮相同
                    imgs[i].style.opacity = 0;
                    imgs[a].style.opacity = 1;
                }
            }
            //切换按钮功能，响应对应图片
            for (i = 0; i < btn.length; i++)
            {
                btn[i].index = i;
                btn[i].onmouseover = function ()
                {
                    show(this.index);
                    clearInterval(play);
                }
            }
            //自动轮播方法
            function autoPlay()
            {
                play = setInterval(function ()
                {
                    index++;
                    index >= imgs.length && (index = 0);
                    show(index);
                }, 3000)
            }
            autoPlay();

            //div的鼠标移入移出事件
            box.onmouseover = function ()
            {
                clearInterval(play);
            };
            box.onmouseout = function ()
            {
                autoPlay();
            };
        </script>

        <script>
            $(document).ready(function ()
            {
                $('.addtocart_btn').click(function (event)
                {
                    var offset = $('.fixed_cart_btn').offset();
                    var scrollTop = $(document).scrollTop();
                    var img = $(this).parent().parent().parent().parent().children('input').attr('src');
                    var flyer = $('<img class="flyer_img" src="' + img + '">');
                    flyer.fly({
                        start: {
                            left: event.pageX,
                            top: event.pageY - scrollTop
                        },
                        end: {
                            left: offset.left + 30,
                            top: offset.top + 10 - scrollTop,
                            width: 0,
                            height: 0
                        },
                        onEnd: function ()
                        {
                            $("#fixedCartBtn").animate({ scale: '1.5' }, 100, function ()
                            {
                                $("#fixedCartBtn").animate({ scale: '1.0' }, 50);
                            });
                            this.destory();
                        }
                    });
                });
            });

            var prm = Sys.WebForms.PageRequestManager.getInstance();

            prm.add_endRequest(function ()
            {
                $('.addtocart_btn').click(function (event)
                {
                    var offset = $('.fixed_cart_btn').offset();
                    var scrollTop = $(document).scrollTop();
                    var img = $(this).parent().parent().parent().parent().children('input').attr('src');
                    var flyer = $('<img class="flyer_img" src="' + img + '">');
                    flyer.fly({
                        start: {
                            left: event.pageX,
                            top: event.pageY - scrollTop
                        },
                        end: {
                            left: offset.left + 30,
                            top: offset.top + 10 - scrollTop,
                            width: 0,
                            height: 0
                        },
                        onEnd: function ()
                        {
                            $("#fixedCartBtn").animate({ scale: '1.5' }, 100, function ()
                            {
                                $("#fixedCartBtn").animate({ scale: '1.0' }, 50);
                            });
                            this.destory();
                        }
                    });
                });
            });
        </script>

    </form>
</body>
</html>
