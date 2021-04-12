<%@ Page Language="C#" AutoEventWireup="true" CodeFile="infopage.aspx.cs" Inherits="infopage" %>

<%@ Register Src="ShoppingCartPanel.ascx" TagName="ShoppingCartPanel" TagPrefix="Cart" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title>我的信息</title>
    <link rel="stylesheet" type="text/css" href="App_CSS/CommonCSS.css" />
    <link rel="stylesheet" type="text/css" href="App_CSS/InfoPageCSS.css" />
    <script type="text/javascript" src="App_JS/jquery-1.11.0.min.js"></script>
    <script type="text/javascript" src="App_JS/jquery-2.1.4-min.js"></script>
    <script type="text/javascript" src="App_JS/InfoPageJS.js"></script>
</head>
<body>
    <form id="form1" runat="server">
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
                    <asp:Button ID="cart_btn" runat="server" Text="购物车" CssClass="cart_btn" OnClick="Showcart_Click" />
                    <asp:Button ID="order_btn" runat="server" Text="我的订单" CssClass="ordet_btn" OnClick="MyOrderFormBtn_Click"/>
                </div>
            </div>
        </div>

        <div class="main_box">
            <div class="main">
                <div class="box_title">
                    ———— 修改账号信息 ————
                </div>
                <div class="left_img_box">
                    <asp:Image ID="UserImg" runat="server" CssClass="user_img" />
                    <asp:FileUpload ID="ImgUploader" runat="server" CssClass="uploader" />
                    <asp:Label ID="Label4" runat="server" Text="上传图片格式为.jpg, .png,大小小于8M" ForeColor="#999999" Font-Size="Small" style="margin-left:24px;"></asp:Label>
                    <asp:Button ID="changeImgBtn" runat="server" Text="立即上传" CssClass="upload_btn" OnClick="UploadImg"/>
                    <br />
                    <asp:Label ID="uploadErrorMsg" runat="server" Text="" ForeColor="Red" Font-Size="Small" style="margin-left:24px;margin-top:5px;"></asp:Label>
                </div>
                <div class="right_info_box">
                    <table style="border-collapse:separate; border-spacing: 10px;">
                        <tr>
                            <td style="width:82px">UID：</td>
                            <td>
                                <asp:Label ID="UIDlbl" runat="server" Text=""></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td>用户名：</td>
                            <td>
                                <asp:Label ID="Usernamelbl" runat="server" Text=""></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td>我的积分：</td>
                            <td>
                                <asp:Label ID="UserCredit" runat="server" Text="" ForeColor="Red" Font-Bold="True" style="float:left;"></asp:Label>
                                <div class="rule" title="积分规则：成功下单后，积分增加实付金额÷10，向下取整。">
                                    ?
                                </div>
                            </td>
                        </tr>
                        <tr>
                            <td>身份证号：</td>
                            <td>
                                <asp:Label ID="UserIDcardlbl" runat="server" Text=""></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td>电话号码：</td>
                            <td>
                                <asp:TextBox ID="UserPhonetxb" runat="server" MaxLength="11" Width="200px" CssClass="textbox"></asp:TextBox>
                                <asp:RegularExpressionValidator ID="PhoneValidator" runat="server" ErrorMessage="*请输入有效的手机号码" ControlToValidate="UserPhonetxb" ForeColor="Red" ValidationExpression="^1([38]\d|5[0-35-9]|7[3678])\d{8}$" Font-Size="Small" Display="Dynamic" ValidationGroup="info"></asp:RegularExpressionValidator>
                                <asp:RequiredFieldValidator ID="PhoneEmptyValidator" runat="server" ErrorMessage="*手机号不能为空" ControlToValidate="UserPhonetxb" ForeColor="Red" Font-Size="Small" Display="Dynamic" ValidationGroup="info"></asp:RequiredFieldValidator>
                            </td>
                        </tr>
                        <tr>
                            <td>新密码：</td>
                            <td>
                                <asp:TextBox ID="NewPwdtxb" runat="server" MaxLength="18" Width="200px" TextMode="Password" placeholder="如不需更改则留空" CssClass="textbox"></asp:TextBox>
                                <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" ErrorMessage="*密码只能由6~18位数字,字母,下划线组成" ControlToValidate="NewPwdtxb" ForeColor="Red" ValidationExpression="^\w{6,18}$" Font-Size="Small" Display="Dynamic" ValidationGroup="info"></asp:RegularExpressionValidator>
                            </td>
                        </tr>
                        <tr>
                            <td>原密码：</td>
                            <td>
                                <asp:TextBox ID="Pwdtxb" runat="server" MaxLength="20" Width="200px" TextMode="Password" placeholder="输入原密码以确认更改" CssClass="textbox" AutoPostBack="True"></asp:TextBox>
                                <asp:Label ID="PwdErrorMsg" runat="server" Text="*原密码输入有误" Visible="False" ForeColor="Red" Font-Size="Small"></asp:Label>
                            </td>
                        </tr>
                    </table>
                    <div class="button_zone">
                        <asp:Button ID="CancelBtn" runat="server" Text="取消并返回" CssClass="cancel_btn" PostBackUrl="~/bingxixi.aspx" />
                        <asp:Button ID="OKbtn" runat="server" Text="确认修改" CssClass="ok_btn" OnClick="OKBtn_Click" ValidationGroup="info" />
                    </div>
                </div>
            </div>
        </div>

        <div class="bottom" style="background-color: whitesmoke;">
            <div class="bottom_con" style="background-color: whitesmoke;">
                Copyleft (C) 不存在的并夕夕图书 9999-9999, All Rights Don't Reserved 京ICP证114514号 没有出版物经营许可证 新出发京批字第直1919号
            </div>
        </div>
    </form>
</body>
</html>
