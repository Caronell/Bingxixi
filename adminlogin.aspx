<%@ Page Language="C#" AutoEventWireup="true" CodeFile="adminlogin.aspx.cs" Inherits="adminlogin" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>并夕夕图书后台管理系统</title>
    <link href="App_CSS/AdminLoginCSS.css" type="text/css" rel="stylesheet" />
</head>
<body>
    <form id="form1" runat="server" autocomplete="off">
        <div id="CenterAreaBg">
            <div id="CenterArea">
                <div id="LogoImg">
                    并夕夕图书后台OS
                </div>
                <div id="LoginInfo">
                    <table border="0" style="width: 100%;">
                        <tr>
                            <td class="Subject" style="width: 45%;">
                                工号
                            </td>
                            <td>
                                <asp:TextBox ID="loginName" runat="server" CssClass="TextField" AutoCompleteType="Disabled" MaxLength="10"></asp:TextBox>
                            </td>
                            <td rowspan="2" style="padding-left: 10px; padding-top:5px;">
                                <asp:ImageButton ID="login_btn" runat="server" ImageUrl="~/App_Pic/adminlogin/userLogin_button.gif" style="outline:none;" OnClick="LoginBtn_Click"/>
                            </td>
                        </tr>
                        <tr>
                            <td class="Subject">
                                密码
                            </td>
                            <td>
                                <asp:TextBox ID="password" runat="server" CssClass="TextField" AutoCompleteType="Disabled" TextMode="Password" MaxLength="20"></asp:TextBox>
                            </td>
                        </tr>
                    </table>
                    <asp:Label ID="warnmsg" runat="server" Text="*工号或密码错误" ForeColor="Red" style="float:right;" Visible="False" Font-Size="Small"></asp:Label>
                </div>
                <div id="CopyRight">
                    <a href="javascript:void(0)">&copy; 9102 版权没有 bingxixi</a>
                </div>
            </div>
        </div>
    </form>
</body>
</html>
