<%@ Page Language="C#" AutoEventWireup="true" CodeFile="login.aspx.cs" Inherits="_Default" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>登录</title>
    <link rel="stylesheet" type="text/css" href="App_CSS/LoginPageCSS.css" />
    <link rel="stylesheet" type="text/css" href="App_CSS/verify.css" />
    <link rel="stylesheet" type="text/css" href="App_CSS/CommonCSS.css" />
    <script type="text/javascript" src="App_JS/jquery-1.11.0.min.js"></script>
    <script type="text/javascript" src="App_JS/jquery-2.1.4-min.js"></script>
    <script type="text/javascript" src="App_JS/LoginPageJS.js"></script>
    <script type="text/javascript" src="App_JS/verify.js"></script>
</head>

<body onload="Trans()">
    <form id="form1" runat="server" autocomplete="off">
        <asp:ScriptManager ID="ScriptManager1" runat="server" EnableHistory="False"></asp:ScriptManager>
        <asp:Panel ID="trans_box" runat="server">
            <div id="trans"></div>
        </asp:Panel>
        <header>
        </header>
        <div class="blur_box">
            <img src="App_Pic/LoginBG.jpg" />
        </div>
        <div id="mainbox" class="main">
            <div class="select_tab">
                <button id="loginbtn" class="tab_item" type="button" style="border-radius: 10px 0 0 0; border-bottom: none; background-color: rgba(247,244,237,1);" onclick="ChangePanel(this)">
                    用户登录
                </button>
                <button id="regstbtn" class="tab_item" type="button" onclick="ChangePanel(this)">
                    注册账号
                </button>
                <div class="tab_item_zhanwei">
                </div>
            </div>
            <asp:Panel ID="UserLoginPanel" runat="server" CssClass="login_panel">
                <asp:Panel ID="UserLoginPanel_Shade" runat="server">
                    <div class="login_title">
                        用户登录
                    </div>
                    <div class="input_info_box">
                        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                            <ContentTemplate>
                                <div class="info_line">
                                    <asp:TextBox ID="username" runat="server" Font-Size="Medium" CssClass="textbox" Style="background-image: url(App_Pic/user_icon.png);" AutoCompleteType="Disabled" placeholder="用户名或手机号码"></asp:TextBox>
                                </div>
                                <div class="error_text_box">
                                    <asp:Label ID="ErrorText" runat="server" Text="" Font-Size="Small" Font-Bold="False" ForeColor="#FF3300"></asp:Label>
                                </div>
                                <div class="info_line">
                                    <asp:TextBox ID="password" runat="server" Font-Size="Medium" TextMode="Password" CssClass="textbox" Style="background-image: url(App_Pic/pwd_icon.png);" AutoCompleteType="Disabled" ForeColor="Black" placeholder="密码"></asp:TextBox>
                                </div>
                            </ContentTemplate>
                            <Triggers>
                                <asp:PostBackTrigger ControlID="Submit" />
                            </Triggers>
                        </asp:UpdatePanel>
                        <asp:Button ID="FindPwdbtn" runat="server" Text="找回密码" CssClass="findpwd_btn" />
                        <div class="info_line" style="margin-top: 10%;">
                            <div id="mpanel1"></div>
                            <br />
                            <asp:Label ID="CodeErrorText" runat="server" Text="*验证码错误" Font-Size="Smaller" ForeColor="Red" Style="width: 6.2em;" Visible="False"></asp:Label>
                        </div>
                        <div class="info_line" style="margin-top: 8%;">
                            <asp:Button ID="Submit" runat="server" Text="登录" Width="100%" Font-Size="Medium" CssClass="login_btn" BorderStyle="None" OnClick="SubmitBtn_Click" />
                        </div>
                    </div>
                </asp:Panel>
            </asp:Panel>
            <asp:HiddenField ID="CodeValid" runat="server" />
            <asp:Panel ID="RegisterPanel" runat="server" CssClass="login_panel" Style="display: none; height: 468px; background-color: rgba(247,244,237,1); border-radius: 0 0 10px 10px;">
                <asp:Panel ID="RegisterPanel_Shade" runat="server">
                    <div class="login_title" style="font-style: italic; font-weight: bold;">
                        新用户注册
                    </div>
                    <asp:UpdatePanel ID="UpdatePanelZone" runat="server">
                        <ContentTemplate>
                            <div class="input_info_box" style="margin-top: 3%; width: 85%;">
                                <div class="regst_line">
                                    <div class="regst_tip">用户名：</div>
                                    <asp:TextBox ID="NewUsername" runat="server" CssClass="regst_input" Font-Size="Medium" AutoPostBack="True" ValidationGroup="NewUsername" CausesValidation="True" onfocus="recordValidatorState()" onblur="activeVisible(1)"></asp:TextBox>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ErrorMessage="*用户名不可为空" ControlToValidate="NewUsername" Font-Size="Small" ForeColor="Red" CssClass="requireFieldValidatorCSS" Display="Dynamic" Enabled="False"></asp:RequiredFieldValidator>
                                    <asp:CustomValidator ID="UsernameValidator" runat="server" ErrorMessage="*此用户名已被注册" Font-Size="Small" ControlToValidate="NewUsername" OnServerValidate="UsernameValidator_ServerValidate" ForeColor="Red" CssClass="regst_error_text" ValidationGroup="NewUsername" Display="Dynamic"></asp:CustomValidator>
                                </div>
                                <div class="regst_line">
                                    <div class="regst_tip">密码：</div>
                                    <asp:TextBox ID="NewPassword" runat="server" CssClass="regst_input" Font-Size="Medium" ValidationGroup="NewPassword" TextMode="Password" CausesValidation="True" placeholder="密码由6到18位数字、字母或下划线组成" onblur="activeVisible(2)"></asp:TextBox>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ErrorMessage="*密码不可为空" ForeColor="Red" Font-Size="Small" ControlToValidate="NewPassword" CssClass="requireFieldValidatorCSS" Display="Dynamic" Enabled="False"></asp:RequiredFieldValidator>
                                    <asp:RegularExpressionValidator ID="PwdValidator" runat="server" ErrorMessage="*密码由6到18位数字、字母或下划线组成" Font-Size="Small" ControlToValidate="NewPassword" ForeColor="Red" CssClass="regst_error_text" ValidationGroup="NewPassword" Display="Dynamic" ValidationExpression="^\w{6,18}$"></asp:RegularExpressionValidator>
                                </div>
                                <div class="regst_line">
                                    <div class="regst_tip">重复密码：</div>
                                    <asp:TextBox ID="RepeatPwd" runat="server" CssClass="regst_input" Font-Size="Medium" TextMode="Password" CausesValidation="True"></asp:TextBox>
                                    <asp:CompareValidator ID="RepeatPwdValidator" runat="server" ErrorMessage="*两次密码输入不一致" ControlToCompare="NewPassword" ControlToValidate="RepeatPwd" Font-Size="Small" ForeColor="Red" CssClass="regst_error_text" ValidationGroup="RepeatPwdValidator" Display="Dynamic"></asp:CompareValidator>
                                </div>
                                <div class="regst_line">
                                    <div class="regst_tip">手机号码：</div>
                                    <asp:TextBox ID="PhoneNum" runat="server" CssClass="regst_input" Font-Size="Medium" AutoPostBack="False" CausesValidation="True" onblur="activeVisible(3)"></asp:TextBox>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ErrorMessage="*手机号码不可为空" ControlToValidate="PhoneNum" ForeColor="Red" Font-Size="Small" CssClass="requireFieldValidatorCSS" Display="Dynamic" Enabled="False"></asp:RequiredFieldValidator>
                                    <asp:RegularExpressionValidator ID="PhoneNumValidator" runat="server" ErrorMessage="*请输入有效的手机号码" ControlToValidate="PhoneNum" ForeColor="Red" ValidationExpression="^1([38]\d|5[0-35-9]|7[3678])\d{8}$" Font-Size="Small" CssClass="regst_error_text" ValidationGroup="PhoneNum" Display="Dynamic"></asp:RegularExpressionValidator>
                                </div>
                                <div class="regst_line">
                                    <div class="regst_tip">身份证号：</div>
                                    <asp:TextBox ID="IDnumber" runat="server" CssClass="regst_input" Font-Size="Medium" AutoPostBack="False" CausesValidation="True" onblur="activeVisible(4)"></asp:TextBox>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" ErrorMessage="*身份证号不可为空" Font-Size="Small" ForeColor="Red" ControlToValidate="IDnumber" CssClass="requireFieldValidatorCSS" Display="Dynamic" Enabled="False"></asp:RequiredFieldValidator>
                                    <asp:RegularExpressionValidator ID="IDnumberValidator" runat="server" ErrorMessage="*请输入有效的身份证号" ControlToValidate="IDnumber" ForeColor="Red" ValidationExpression="\d{17}[\d|X]|\d{15}" Font-Size="Small" CssClass="regst_error_text" ValidationGroup="IDnumber" Display="Dynamic"></asp:RegularExpressionValidator>
                                </div>
                            </div>
                        </ContentTemplate>
                        <Triggers>
                            <asp:AsyncPostBackTrigger ControlID="Regist_Btn" />
                        </Triggers>
                    </asp:UpdatePanel>
                    <div class="identify">
                        <div id="mpanel4"></div>
                    </div>
                    <asp:Button ID="Regist_Btn" runat="server" Text="注册" Font-Size="Medium" CssClass="regst_btn" BorderStyle="None" OnClick="RegisterBtn_Click" />
                    <asp:HiddenField ID="robot" runat="server" />
                    <asp:HiddenField ID="passwordState" runat="server" />
                    <asp:HiddenField ID="phonenumState" runat="server" />
                    <asp:HiddenField ID="IDnumState" runat="server" />
                </asp:Panel>
            </asp:Panel>
        </div>

        <script type="text/javascript">
            $('#mpanel4').slideVerify(
                {
                    type: 2,    //类型
                    vOffset: 5, //误差量
                    vSpace: 5,  //间隔
                    imgName: ['1.jpg', '2.jpg'],
                    imgSize:
                    {
                        width: '100%',
                        height: '200px',
                    },
                    blockSize:
                    {
                        width: '40px',
                        height: '40px',
                    },
                    barSize:
                    {
                        width: '100%',
                        height: '40px',
                    },
                    ready: function () { },
                    success: function ()
                    {
                        document.getElementById("robot").value = "ok";
                    },
                    error: function ()
                    {
                        //alert('验证失败！');
                    }
                });

            $('#mpanel1').codeVerify({
                type: 1,
                width: '30%',
                height: '30px',
                fontSize: '16px',
                codeLength: 4,
                btnId: 'Submit',
                ready: function ()
                {
                },
                success: function ()
                {
                    document.getElementById("CodeValid").value = "ok";
                },
                error: function ()
                {
                    document.getElementById("CodeValid").value = "no";
                }
            });
        </script>
    </form>
</body>
</html>
