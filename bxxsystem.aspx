<%@ Page Language="C#" AutoEventWireup="true" CodeFile="bxxsystem.aspx.cs" Inherits="bxxsystem" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>并夕夕图书后台管理系统</title>
    <link href="App_CSS/manageSystemCSS.css" type="text/css" rel="stylesheet" />
    <script type="text/javascript" src="App_JS/jquery-1.11.0.min.js"></script>
    <script type="text/javascript" src="App_JS/jquery-2.1.4-min.js"></script>
    <script type="text/javascript" src="App_JS/menu.js"></script>
</head>
<body>
    <form id="form1" runat="server" autocomplete="off">
        <div class="PageHead" style="margin: 0;">
            <div id="Head1">
                <div id="Logo">
                    <a id="msgLink" href="bxxsystem.aspx" style="color: #F1F9FE; font-size: 28px; font-family: Arial Black, Arial; text-decoration: none;">系统管理</a>
                </div>
                <div id="Head1Right">
                    <div id="Head1Right_UserName">
                        <img border="0" width="13" height="14" src="App_Pic/syspic/user.gif" />
                        您好，<asp:Label ID="adminNum" runat="server" Text="" Font-Bold="True"></asp:Label><b>管理员</b>
                    </div>
                    <div id="Head1Right_UserDept"></div>
                    <div id="Head1Right_Time"></div>
                </div>
                <div id="Head1Right_SystemButton">
                    <asp:ImageButton ID="LogoutBtn" runat="server" Width="78" Height="20" ImageUrl="~/App_Pic/syspic/logout.gif" AlternateText="退出系统" style="outline:none;" OnClick="AdminLogout" />
                </div>
            </div>
            <div id="Head2">
                <div id="Head2_Awoke">
                    <ul id="AwokeNum">
                        <li><a href="javascript:void(0)">
                            <img border="0" width="11" height="13" src="App_Pic/syspic/msg.gif" />
                            消息
						<span id="msg"></span>
                        </a>
                        </li>
                    </ul>
                </div>
                <div id="Head2_FunctionList">
                    <b>这是一条消息</b>
                </div>
            </div>
        </div>

        <div id="Menu">
            <ul id="MenuUl">
                <li class="level1">
                    <div onclick="menuClick(this);" class="level1Style">
                        <img src="App_Pic/syspic/FUNC20082.gif" class="Icon" />
                        系统管理
                    </div>
                    <ul style="display: none;" class="MenuLevel2">
                        <li class="level2">
                            <asp:Button ID="bookManageBtn" runat="server" Text="商品管理" CssClass="level2Style" OnClick="BookManageBtn_Click" />
                        </li>
                        <li class="level2">
                            <asp:Button ID="orderformBtn" runat="server" Text="订单管理" CssClass="level2Style" OnClick="OrderformBtn_Click" />
                        </li>
                        <li class="level2">
                            <asp:Button ID="userManageBtn" runat="server" Text="用户管理" CssClass="level2Style" OnClick="UserManageBtn_Click" />
                        </li>
                    </ul>
                </li>
            </ul>
        </div>

        <asp:Panel ID="MainBox" runat="server" CssClass="rightmain">
            <asp:Panel ID="bookManageBox" runat="server" Visible="False">
                <asp:Panel ID="bookGridPanel" runat="server">
                    <div style="width: 100%; height: 24px; margin-left: 15px;">
                        <asp:Button ID="DelAllBtn" runat="server" Text="批量删除" style="height: 24px; margin-left: -4px; outline:none;" OnClick="DelAllBtn_Click" BackColor="#FF3300" ForeColor="White" OnClientClick="if(!window.confirm('确认要删除吗？')) return false;"/>
                        <asp:DropDownList ID="SearchType" runat="server" Style="height: 24px; margin-left: 25px;">
                            <asp:ListItem Selected="True">按名称查询</asp:ListItem>
                            <asp:ListItem>按ID查询</asp:ListItem>
                            <asp:ListItem>按作者查询</asp:ListItem>
                            <asp:ListItem>按出版社查询</asp:ListItem>
                        </asp:DropDownList>
                        <asp:TextBox ID="SearchText" runat="server" CssClass="search_textbox"></asp:TextBox>
                        <asp:Button ID="SearchBtn" runat="server" Text="搜索" Style="height: 24px;" OnClick="SearchBtn_Click" />
                        <asp:Button ID="ReturnBtn" runat="server" Text="返回" Style="height: 24px;" OnClick="ReturnBtn_Click" />
                        <asp:Button ID="AddBookBtn" runat="server" Text="＋添加新书" BackColor="#33CC33" style="height: 24px; margin-left: 15px; outline:none;" OnClick="AddBookBtn_Click" />
                    </div>
                    <asp:GridView ID="bookGridView" runat="server" CssClass="mGrid"
                        AllowPaging="True"
                        PageSize="15"
                        OnPageIndexChanging="BookGridView_PageIndexChanging"
                        AutoGenerateColumns="False"
                        GridLines="None"
                        PagerStyle-CssClass="pgr"
                        AlternatingRowStyle-CssClass="alt"
                        OnRowCommand="BookGridView_OnRowCommand"
                        OnRowCreated="BookGridView_RowCreated">
                        <Columns>
                            <asp:TemplateField HeaderText="选择" ControlStyle-CssClass="grid_checkbox">
                                <ItemTemplate>
                                    <asp:CheckBox ID="cbSelect" runat="server" CssClass="grid_checkbox" />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:BoundField DataField="id" HeaderText="图书ID" />
                            <asp:BoundField DataField="name" HeaderText="书名" />
                            <asp:BoundField DataField="author" HeaderText="作者" />
                            <asp:BoundField DataField="publisher" HeaderText="出版社" />
                            <asp:BoundField DataField="remain" HeaderText="货余量" />
                            <asp:ButtonField ButtonType="Button" HeaderText="删除" Text="删除" ControlStyle-CssClass="grid_btn" CommandName="DelBook" />
                            <asp:ButtonField ButtonType="Button" HeaderText="编辑" Text="查看详情" ControlStyle-CssClass="grid_btn" CommandName="EditBook" />
                        </Columns>
                    </asp:GridView>
                </asp:Panel>
                <asp:Panel ID="bookDetailPanel" runat="server" Visible="False" CssClass="book_detail_panel">
                    <div class="return_header">
                        <asp:Button ID="CloseInfoBtn" runat="server" Text="返回" OnClick="CloseDetailPanel" Width="60px" Height="30px" />
                    </div>
                    <div class="detail_img_box">
                        <asp:Image ID="bookImg" runat="server" Width="220px" Height="220px" BorderWidth="1" BorderStyle="Solid" BorderColor="#999999" AlternateText="图书图片" ImageUrl="~/book_pic/book_default.jpg" />
                        <asp:FileUpload ID="BookImgUpload" runat="server" Width="170px" Style="outline: none;" />
                        <asp:Button ID="changeImgBtn" runat="server" Text="上传" Style="float: right; margin-right: 28px;" OnClick="UploadImg" />
                        <br />
                        <asp:Label ID="Label4" runat="server" Text="上传图片格式为.jpg, .png,大小小于8M" ForeColor="#999999" Font-Size="Small"></asp:Label>
                        <br />
                        <asp:Label ID="uploadErrorMsg" runat="server" Text="" ForeColor="Red"></asp:Label>
                    </div>
                    <div class="detail_info_box">
                        <table id="bookInfoTable" class="detail_table">
                            <tr>
                                <td style="width: 82px;">图书ID：</td>
                                <td colspan="7">
                                    <asp:TextBox ID="bookID" runat="server" Enabled="false" Width="100%"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td>书名：</td>
                                <td colspan="5">
                                    <asp:TextBox ID="bookName" runat="server" Style="width: calc(100% - 20px);"></asp:TextBox>
                                </td>
                                <td>类型：</td>
                                <td colspan="1">
                                    <asp:TextBox ID="bookType" runat="server" Width="100%"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td>推荐语：</td>
                                <td colspan="7">
                                    <asp:TextBox ID="bookCommand" runat="server" TextMode="MultiLine" Width="100%" Height="3em"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td>内容简介：</td>
                                <td colspan="7">
                                    <asp:TextBox ID="bookIntro" runat="server" TextMode="MultiLine" Width="100%" Height="7em"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td>作者：</td>
                                <td colspan="3">
                                    <asp:TextBox ID="bookAuthor" runat="server" Style="width: calc(100% - 20px);" onblur="NameFill(this)"></asp:TextBox>
                                </td>
                                <td>译者：</td>
                                <td colspan="3">
                                    <asp:TextBox ID="bookTranslator" runat="server" Width="100%" placeholder="若无译者请留空"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td>出版社：</td>
                                <td colspan="7">
                                    <asp:TextBox ID="bookPublisher" runat="server" Width="100%" onblur="PublisherFill(this)"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td style="width: 5em;">出版日期：</td>
                                <td style="width: 13em;">
                                    <asp:DropDownList ID="bookPubyear" runat="server" Width="4em" AutoPostBack="True" OnSelectedIndexChanged="YearList_SelectedIndexChanged"></asp:DropDownList>
                                    <asp:Label ID="Label1" runat="server" Text="年"></asp:Label>
                                    <asp:DropDownList ID="bookPubmonth" runat="server" Width="3em" AutoPostBack="True" OnSelectedIndexChanged="MonthList_SelectedIndexChanged"></asp:DropDownList>
                                    <asp:Label ID="Label2" runat="server" Text="月"></asp:Label>
                                    <asp:DropDownList ID="bookPubday" runat="server" Width="3em" AutoPostBack="True"></asp:DropDownList>
                                    <asp:Label ID="Label3" runat="server" Text="日"></asp:Label>
                                </td>
                                <td style="width: 5em;">价格(￥)：</td>
                                <td style="width: 6em;">
                                    <asp:TextBox ID="bookPrice" runat="server" Width="5em" onblur="clearNoNum(this)"></asp:TextBox>
                                </td>
                                <td style="width: 3em;">尺寸：</td>
                                <td style="width: 5em;">
                                    <asp:TextBox ID="bookSize" runat="server" Width="4em"></asp:TextBox>
                                </td>
                                <td style="width: 3em;">纸型：</td>
                                <td>
                                    <asp:TextBox ID="bookPaper" runat="server" Width="100%"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td>是否套装：</td>
                                <td>
                                    <asp:RadioButton ID="suite" runat="server" Text="是" GroupName="suitecheck" />
                                    <asp:RadioButton ID="notsuite" runat="server" Text="否" GroupName="suitecheck" />
                                </td>
                            </tr>
                            <tr>
                                <td>货余量：</td>
                                <td>
                                    <asp:TextBox ID="bookRemain" runat="server" Width="100%" onkeypress="if (event.keyCode<48 || event.keyCode>57) event.returnValue=false;" onblur="clearNoNum(this)"></asp:TextBox>
                                </td>
                            </tr>
                        </table>
                        <div class="submit_box">
                            <asp:Button ID="BookSubmit" runat="server" Text="提交修改" OnClick="BookSubmitBtn_Click" CssClass="detail_submit_btn" />
                            <asp:Button ID="Cancel" runat="server" Text="取消" OnClick="CloseDetailPanel" CssClass="detail_cancel_btn" />
                        </div>
                    </div>
                </asp:Panel>
            </asp:Panel>
            <asp:Panel ID="orderFormBox" runat="server" Visible="False">
                <asp:Panel ID="orderGridPanel" runat="server">
                    <div style="width: 100%; height: 24px; margin-left: 15px;">
                        <asp:Button ID="DelAllOrder" runat="server" Text="批量删除" style="height: 24px; margin-left: -4px; outline:none;" BackColor="#FF3300" ForeColor="White" OnClick="DelAllOrder_Click" OnClientClick="if(!window.confirm('确认要删除吗？')) return false;"/>
                        <asp:DropDownList ID="OrderSearchType" runat="server" Style="height: 24px; margin-left: 25px;">
                            <asp:ListItem Selected="True">按用户查询</asp:ListItem>
                            <asp:ListItem>按订单号查询</asp:ListItem>
                        </asp:DropDownList>
                        <asp:TextBox ID="OrderSearchText" runat="server" CssClass="search_textbox"></asp:TextBox>
                        <asp:Button ID="OrderSearchBtn" runat="server" Text="搜索" Style="height: 24px;" OnClick="OrderSearchBtn_Click" />
                        <asp:Button ID="OrderReturnBtn" runat="server" Text="返回" Style="height: 24px;" OnClick="OrderReturnBtn_Click" />
                    </div>
                    <asp:GridView ID="orderGridView" runat="server" CssClass="mGrid"
                        AllowPaging="True"
                        PageSize="15"
                        OnPageIndexChanging="OrderGridView_PageIndexChanging"
                        AutoGenerateColumns="False"
                        GridLines="None"
                        PagerStyle-CssClass="pgr"
                        AlternatingRowStyle-CssClass="alt"
                        OnRowCommand="OrderGridView_OnRowCommand"
                        OnRowCreated="OrderGridView_RowCreated">
                        <Columns>
                            <asp:TemplateField HeaderText="选择" ControlStyle-CssClass="grid_checkbox">
                                <ItemTemplate>
                                    <asp:CheckBox ID="cbSelect" runat="server" CssClass="grid_checkbox" />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:BoundField DataField="id" HeaderText="订单号" />
                            <asp:BoundField DataField="username" HeaderText="下单用户" />
                            <asp:BoundField DataField="date" HeaderText="日期" />
                            <asp:ButtonField ButtonType="Button" HeaderText="删除" Text="删除" ControlStyle-CssClass="grid_btn" CommandName="DelOrder" />
                            <asp:ButtonField ButtonType="Button" HeaderText="详情" Text="查看详情" ControlStyle-CssClass="grid_btn" CommandName="CheckOrder" />
                        </Columns>
                    </asp:GridView>
                </asp:Panel>
                <asp:Panel ID="orderDetailPanel" runat="server" Visible="False" CssClass="book_detail_panel">
                    <div class="return_header">
                        <asp:Button ID="Button1" runat="server" Text="返回" OnClick="CloseOrderDetailPanel" Width="60px" Height="30px" />
                        <div style="width: 70%; margin-left: 15%;">
                            <asp:Label ID="OrderID" runat="server" Text="" Font-Bold="True"></asp:Label>
                            <asp:Label ID="OrderDate" runat="server" Text="" style="margin-left:10px;"></asp:Label>
                            <asp:Table ID="OrderDetailTable" runat="server" style="width: 100%;" BorderWidth="1" BorderColor="#CCCCCC" BorderStyle="Solid" Font-Size="Small">
                                <asp:TableHeaderRow BackColor="#CCCCCC">
                                    <asp:TableHeaderCell>书名</asp:TableHeaderCell>
                                    <asp:TableHeaderCell>单价</asp:TableHeaderCell>
                                    <asp:TableHeaderCell>数量</asp:TableHeaderCell>
                                    <asp:TableHeaderCell>单项总价</asp:TableHeaderCell>
                                </asp:TableHeaderRow>
                            </asp:Table>
                            <asp:Label ID="OrderUser" runat="server" Text="" style="float:left;"></asp:Label>
                            <asp:HiddenField ID="ExportText" runat="server" />
                            <asp:Label ID="TotalPrice" runat="server" Text="" style="float:right;"></asp:Label>
                            <asp:Label ID="TotalCount" runat="server" Text="" style="float:right;"></asp:Label>
                            <asp:Button ID="ExportOrderBtn" runat="server" Text="导出" OnClick="ExportOrderDetail" Width="60px" Height="30px" Style="float:right;margin-right:10px;margin-top:5px;" />
                        </div>
                    </div>
                </asp:Panel>
            </asp:Panel>
            <asp:Panel ID="userManageBox" runat="server" Visible="False">
                <asp:Panel ID="userGridPanel" runat="server">
                    <div style="width: 100%; height: 24px; margin-left: 15px;">
                        <asp:Button ID="DelAllUser" runat="server" Text="批量删除" style="height: 24px; margin-left: -4px; outline:none;" OnClick="DelAllUser_Click" BackColor="#FF3300" ForeColor="White" OnClientClick="if(!window.confirm('确认要删除吗？')) return false;"/>
                        <asp:DropDownList ID="UserSearchType" runat="server" Style="height: 24px; margin-left: 25px;">
                            <asp:ListItem Selected="True">按用户名查询</asp:ListItem>
                            <asp:ListItem>按ID查询</asp:ListItem>
                            <asp:ListItem>按身份证号查询</asp:ListItem>
                            <asp:ListItem>按电话号码查询</asp:ListItem>
                        </asp:DropDownList>
                        <asp:TextBox ID="UserSearchText" runat="server" CssClass="search_textbox"></asp:TextBox>
                        <asp:Button ID="UserSearchBtn" runat="server" Text="搜索" Style="height: 24px;" OnClick="UserSearchBtn_Click" />
                        <asp:Button ID="UserReturnBtn" runat="server" Text="返回" Style="height: 24px;" OnClick="UserReturnBtn_Click" />
                    </div>
                    <asp:GridView ID="userGridView" runat="server" CssClass="mGrid"
                        AllowPaging="True"
                        PageSize="15"
                        OnPageIndexChanging="UserGridView_PageIndexChanging"
                        AutoGenerateColumns="False"
                        GridLines="None"
                        PagerStyle-CssClass="pgr"
                        AlternatingRowStyle-CssClass="alt"
                        OnRowCommand="UserGridView_OnRowCommand"
                        OnRowCreated="UserGridView_RowCreated">
                        <Columns>
                            <asp:TemplateField HeaderText="选择" ControlStyle-CssClass="grid_checkbox">
                                <ItemTemplate>
                                    <asp:CheckBox ID="cbSelect" runat="server" CssClass="grid_checkbox" />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:BoundField DataField="id" HeaderText="UID" />
                            <asp:BoundField DataField="username" HeaderText="用户名" />
                            <asp:BoundField DataField="idcard" HeaderText="身份证号" />
                            <asp:BoundField DataField="phonenum" HeaderText="电话号码" />
                            <asp:BoundField DataField="accountstatus" HeaderText="账号状态" />
                            <asp:ButtonField ButtonType="Button" HeaderText="删除" Text="删除" ControlStyle-CssClass="grid_btn" CommandName="DelUser" />
                            <asp:ButtonField ButtonType="Button" HeaderText="编辑" Text="查看详情" ControlStyle-CssClass="grid_btn" CommandName="EditUser" />
                        </Columns>
                    </asp:GridView>
                </asp:Panel>
                <asp:Panel ID="userDetailPanel" runat="server" Visible="False">
                    <div class="return_header">
                        <asp:Button ID="CloseUserDetailBtn" runat="server" Text="返回" OnClick="CloseUserDetailPanel" Width="60px" Height="30px" style="margin-left:20px;"/>
                    </div>
                    <div class="detail_img_box">
                        <asp:Image ID="UserImage" runat="server" Width="220px" Height="220px" BorderWidth="1" BorderStyle="Solid" BorderColor="#999999" AlternateText="用户头像" ImageUrl="~/App_Pic/default_userimg.jpg" />
                        <asp:Button ID="ReSetImgBtn" runat="server" Text="重置为默认头像" Style="float: right; margin-right: 28px;" OnClick="ReSetImg" />
                    </div>
                    <div class="detail_info_box">
                        <table id="userInfoTable" class="detail_table">
                            <tr>
                                <td style="width:82px;">UID：</td>
                                <td>
                                    <asp:Label ID="UIDTxb" runat="server" Text=""></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td>用户名：</td>
                                <td>
                                    <asp:TextBox ID="UsernameTxb" runat="server" Enabled="False" Width="200px" Height="24px"></asp:TextBox>
                                    <asp:Button ID="ResetUsernameBtn" runat="server" Text="重置用户名" Height="24px" OnClick="ResetUsername"/>
                                </td>
                            </tr>
                            <tr>
                                <td>密码：</td>
                                <td>
                                    <asp:TextBox ID="PasswordTxb" runat="server" Enabled="False" Width="200px" Height="24px"></asp:TextBox>
                                    <asp:Button ID="ResetPasswordBtn" runat="server" Text="重置密码" Height="24px" Width="5em" OnClick="ResetPassword"/>
                                </td>
                            </tr>
                            <tr>
                                <td>积分：</td>
                                <td>
                                    <asp:TextBox ID="CreditTxb" runat="server" Enabled="True" Width="200px" Height="24px" onkeypress="if (event.keyCode<48 || event.keyCode>57) event.returnValue=false;" onblur="clearNoNum(this)"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td>身份证号：</td>
                                <td>
                                    <asp:TextBox ID="IDcardTxb" runat="server" Enabled="True" Width="200px" MaxLength="18" Height="24px" onblur="checkIDCard(this)"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td>手机号码：</td>
                                <td>
                                    <asp:TextBox ID="PhoneNumTxb" runat="server" Enabled="True" Width="200px" MaxLength="11" Height="24px" onblur="checkPhone(this)"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td>账户状态：</td>
                                <td>
                                    <asp:Label ID="AccountStatus" runat="server" Text=""></asp:Label>
                                    <asp:Button ID="LockUserBtn" runat="server" Text="封禁用户" OnClick="ChangeLockStatus" Height="24px" style="margin-left: 10px;"/>
                                </td>
                            </tr>
                        </table>
                        <div class="submit_box">
                            <asp:Button ID="UserSubmit" runat="server" Text="提交修改" OnClick="UserSubmitBtn_Click" CssClass="detail_submit_btn" />
                            <asp:Button ID="UserCancel" runat="server" Text="取消" OnClick="CloseUserDetailPanel" CssClass="detail_cancel_btn" />
                        </div>
                    </div>
                </asp:Panel>
            </asp:Panel>
        </asp:Panel>
        <script type="text/javascript">
            function clearNoNum(obj)
            {
                obj.value = obj.value.replace(/[^\d.]/g, "");  //清除“数字”和“.”以外的字符
                obj.value = obj.value.replace(/\.{2,}/g, "."); //只保留第一个. 清除多余的
                obj.value = obj.value.replace(".", "$#$").replace(/\./g, "").replace("$#$", ".");
                obj.value = obj.value.replace(/^(\-)*(\d+)\.(\d\d).*$/, '$1$2.$3');//只能输入两个小数
                if (obj.value.indexOf(".") < 0 && obj.value != "")
                {
                    //以上已经过滤，此处控制的是如果没有小数点，首位不能为类似于 01、02的金额
                    obj.value = parseFloat(obj.value);
                }
                if (obj.value == "")
                    obj.value = "0";
                if (!obj.value || obj.value == '0' || obj.value == '0.0' || obj.value == '0.00')
                {
                    return;
                }
            }

            function NameFill(obj)
            {
                if (obj.value == "")
                    obj.value = "佚名";
            }

            function PublisherFill(obj)
            {
                if (obj.value == "")
                    obj.value = "无";
            }
        </script>
    </form>
</body>
</html>
