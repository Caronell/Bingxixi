using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class bxxsystem : System.Web.UI.Page
{
    private MySqlConnection SQLcon = new MySqlConnection(MySQLConnectionManager.GetConnectionString());
    private bool isLogin = true;

    //----------------------------------------------------
    // ● 加载后执行
    //----------------------------------------------------
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            if (bookManageBox.Visible == true)
                BindBookData();
            if (orderFormBox.Visible == true)
                BindOrderData();
            if (userManageBox.Visible == true)
                BindUserData();
            FillYearList(DateTime.Now.Year);
            FillMonthList();
            FillDayList(DateTime.Now.Year, DateTime.Now.Month);
        }
        if (Request.Cookies["admin"] == null || Request.Cookies["admin"].Value == null)
        {
            isLogin = false;
            Response.Redirect("~/adminlogin.aspx");
        }
        else
        {
            isLogin = true;
            adminNum.Text = Request.Cookies["admin"].Value.ToString();
        }
    }

    //----------------------------------------------------
    // ● 点击图书管理
    //----------------------------------------------------
    protected void BookManageBtn_Click(object sender, EventArgs e)
    {
        ClearState();
        bookManageBox.Visible = true;
        BindBookData();
    }

    //----------------------------------------------------
    // ● 点击订单管理
    //----------------------------------------------------
    protected void OrderformBtn_Click(object sender, EventArgs e)
    {
        ClearState();
        orderFormBox.Visible = true;
        BindOrderData();
    }

    //----------------------------------------------------
    // ● 点击用户管理
    //----------------------------------------------------
    protected void UserManageBtn_Click(object sender, EventArgs e)
    {
        ClearState();
        userManageBox.Visible = true;
        BindUserData();
    }

    //----------------------------------------------------
    // ● 重置主面板的可见状态
    //----------------------------------------------------
    private void ClearState()
    {
        foreach (Control c in MainBox.Controls)
        {
            c.Visible = false;
        }
        SearchText.Text = "";
        OrderSearchText.Text = "";
    }

    //----------------------------------------------------
    // ● 绑定所有图书信息
    //----------------------------------------------------
    public void BindBookData()
    {
        string order = "";
        if (SearchText.Text != "")
        {
            switch (SearchType.SelectedIndex)
            {
                case 0:
                    order = "SELECT id,name,author,publisher,remain FROM bookshelf WHERE name LIKE CONCAT('%',@content,'%');";
                    break;
                case 1:
                    order = "SELECT id,name,author,publisher,remain FROM bookshelf WHERE id = @content;";
                    break;
                case 2:
                    order = "SELECT id,name,author,publisher,remain FROM bookshelf WHERE author LIKE CONCAT('%',@content,'%');";
                    break;
                case 3:
                    order = "SELECT id,name,author,publisher,remain FROM bookshelf WHERE publisher LIKE CONCAT('%',@content,'%');";
                    break;
                default:
                    order = "SELECT id,name,author,publisher,remain FROM bookshelf WHERE name LIKE CONCAT('%',@content,'%');";
                    break;
            }
        }
        else
        {
            order = "SELECT id,name,author,publisher,remain FROM bookshelf;";
        }
        DataSet ds = new DataSet();
        try
        {
            SQLcon.Open();
            MySqlCommand SQLcmd = new MySqlCommand(order, SQLcon);
            if (SearchText.Text != "")
            {
                SQLcmd.Parameters.Add(new MySqlParameter("@content", SearchText.Text));
            }
            MySqlDataAdapter sda = new MySqlDataAdapter(SQLcmd);
            sda.Fill(ds);
            bookGridView.DataSource = ds;
            bookGridView.DataBind();
        }
        catch (Exception e)
        {
            throw e;
        }
        finally
        {
            SQLcon.Close();
        }
    }

    //----------------------------------------------------
    // ● 图书信息分页事件
    //----------------------------------------------------
    protected void BookGridView_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        bookGridView.PageIndex = e.NewPageIndex;
        BindBookData();
    }

    //----------------------------------------------------
    // ● 为删除按钮添加确认事件
    //----------------------------------------------------
    protected void BookGridView_RowCreated(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            ((Button)e.Row.Cells[6].Controls[0]).Attributes.Add("onclick", "if(!window.confirm('确认要删除吗？')) return false;");
        }
    }

    //----------------------------------------------------
    // ● 图书信息命令事件
    //----------------------------------------------------
    protected void BookGridView_OnRowCommand(object sender, GridViewCommandEventArgs e)
    {
        int index = Convert.ToInt32(e.CommandArgument);
        int id = Convert.ToInt32(bookGridView.Rows[index].Cells[1].Text);
        if (e.CommandName == "DelBook")
        {
            //this.Page.ClientScript.RegisterStartupScript(this.Page.GetType(), "message", "<script language='javascript' defer>alert('" + id.ToString() + "');</script>");
            DeleteBook(id);
            BindBookData();
        }
        else if (e.CommandName == "EditBook")
        {
            bookGridPanel.Visible = false;
            bookDetailPanel.Visible = true;
            BookSubmit.Text = "确认修改";
            GetBookDetail(id);
        }
    }

    //----------------------------------------------------
    // ● 获取图书详细信息
    //----------------------------------------------------
    private void GetBookDetail(int id)
    {
        string order = "SELECT id, name, command, introduce, author, translator, type, publisher, pubdate, price, size, papertype, issuite, picturepath, remain FROM bookshelf WHERE id = @id;";
        try
        {
            SQLcon.Open();
            MySqlCommand SQLcmd = new MySqlCommand(order, SQLcon);
            SQLcmd.Parameters.Add(new MySqlParameter("@id", id));
            MySqlDataReader reader = SQLcmd.ExecuteReader();
            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    bookID.Text = reader.GetInt32(0).ToString();
                    bookName.Text = reader.GetString(1);
                    bookCommand.Text = reader.IsDBNull(2) ? "" : reader.GetString(2);
                    bookIntro.Text = reader.GetString(3);
                    bookAuthor.Text = reader.GetString(4);
                    bookTranslator.Text = reader.IsDBNull(5) ? "" : reader.GetString(5);
                    bookType.Text = reader.GetString(6);
                    bookPublisher.Text = reader.GetString(7);
                    bookPubyear.Text = reader.GetDateTime(8).Year.ToString();
                    bookPubmonth.Text = reader.GetDateTime(8).Month.ToString();
                    bookPubday.Text = reader.GetDateTime(8).Day.ToString();
                    bookPrice.Text = reader.GetFloat(9).ToString("f2");
                    bookSize.Text = reader.GetString(10);
                    bookPaper.Text = reader.GetString(11);
                    if (reader.GetInt32(12) == 0)
                        notsuite.Checked = true;
                    else
                        suite.Checked = true;
                    bookImg.ImageUrl = reader.IsDBNull(13) ? "" : reader.GetString(13);
                    bookRemain.Text = reader.GetInt32(14).ToString();
                }
            }
            reader.Close();
        }
        catch (Exception e)
        {
            throw e;
        }
        finally
        {
            SQLcon.Close();
        }
    }

    //----------------------------------------------------
    // ● 关闭图书详情界面
    //----------------------------------------------------
    protected void CloseDetailPanel(object sender, EventArgs e)
    {
        bookDetailPanel.Visible = false;
        bookGridPanel.Visible = true;
        BindBookData();
    }

    //----------------------------------------------------
    // ● 删除图书
    //----------------------------------------------------
    private void DeleteBook(int id)
    {
        string order = "DELETE FROM bookshelf WHERE id = @id";
        try
        {
            SQLcon.Open();
            MySqlCommand SQLcmd = new MySqlCommand(order, SQLcon);
            SQLcmd.Parameters.Add(new MySqlParameter("@id", id));
            SQLcmd.ExecuteNonQuery();
        }
        catch (Exception ex)
        {
            throw ex;
        }
        finally
        {
            SQLcon.Close();
        }
    }

    //----------------------------------------------------
    // ● 搜索图书
    //----------------------------------------------------
    protected void SearchBtn_Click(object sender, EventArgs e)
    {
        DataSet ds = new DataSet();
        string order = "";
        switch (SearchType.SelectedIndex)
        {
            case 0:
                order = "SELECT id,name,author,publisher,remain FROM bookshelf WHERE name LIKE CONCAT('%',@content,'%');";
                break;
            case 1:
                order = "SELECT id,name,author,publisher,remain FROM bookshelf WHERE id = @content;";
                break;
            case 2:
                order = "SELECT id,name,author,publisher,remain FROM bookshelf WHERE author LIKE CONCAT('%',@content,'%');";
                break;
            case 3:
                order = "SELECT id,name,author,publisher,remain FROM bookshelf WHERE publisher LIKE CONCAT('%',@content,'%');";
                break;
            default:
                order = "SELECT id,name,author,publisher,remain FROM bookshelf WHERE name LIKE CONCAT('%',@content,'%');";
                break;
        }
        try
        {
            SQLcon.Open();
            MySqlCommand SQLcmd = new MySqlCommand(order, SQLcon);
            SQLcmd.Parameters.Add(new MySqlParameter("@content", SearchText.Text));
            MySqlDataAdapter sda = new MySqlDataAdapter(SQLcmd);
            sda.Fill(ds);
            bookGridView.DataSource = ds;
            bookGridView.DataBind();
        }
        catch (Exception ex)
        {
            throw ex;
        }
        finally
        {
            SQLcon.Close();
        }
    }

    //----------------------------------------------------
    // ● 添加图书
    //----------------------------------------------------
    protected void AddBookBtn_Click(object sender, EventArgs e)
    {
        bookID.Text = "";
        bookName.Text = "";
        bookCommand.Text = "";
        bookIntro.Text = "";
        bookAuthor.Text = "";
        bookTranslator.Text = "";
        bookType.Text = "";
        bookPublisher.Text = "";
        bookPubyear.SelectedIndex = 0;
        bookPubmonth.SelectedIndex = 0;
        bookPubday.SelectedIndex = 0;
        bookPrice.Text = "";
        bookSize.Text = "";
        bookPaper.Text = "";
        bookImg.ImageUrl = "~/book_pic/book_default.jpg";
        bookRemain.Text = "";
        bookGridPanel.Visible = false;
        bookDetailPanel.Visible = true;
        BookSubmit.Text = "确认添加";
        notsuite.Checked = true;
    }

    //----------------------------------------------------
    // ● 上传图片
    //----------------------------------------------------
    protected void UploadImg(object sender, EventArgs e)
    {
        bool fileOk = false;
        if (BookImgUpload.HasFile)//验证是否包含文件
        {
            //取得文件的扩展名,并转换成小写
            string fileExtension = Path.GetExtension(BookImgUpload.FileName).ToLower();
            //验证上传文件是否图片格式
            fileOk = IsImage(fileExtension);
            if (fileOk)
            {
                //对上传文件的大小进行检测，限定文件最大不超过8M
                if (BookImgUpload.PostedFile.ContentLength < 8192000)
                {
                    string filepath = "/book_pic/";
                    if (Directory.Exists(Server.MapPath(filepath)) == false)//如果不存在就创建file文件夹
                    {
                        Directory.CreateDirectory(Server.MapPath(filepath));
                    }
                    string virpath = filepath + CreatePasswordHash(BookImgUpload.FileName, 4) + fileExtension;//这是存到服务器上的虚拟路径
                    string mappath = Server.MapPath(virpath);//转换成服务器上的物理路径
                    BookImgUpload.PostedFile.SaveAs(mappath);//保存图片

                    bookImg.ImageUrl = virpath;
                    //清空提示
                    uploadErrorMsg.Text = "";
                }
                else
                {
                    uploadErrorMsg.Text = "文件大小超出8M！请重新选择！";
                }
            }
            else
            {
                uploadErrorMsg.Text = "要上传的文件类型不对！请重新选择！";
            }
        }
        else
        {
            uploadErrorMsg.Text = "请选择要上传的图片！";
        }
    }

    //----------------------------------------------------
    // ● 验证是否为图片
    //----------------------------------------------------
    protected bool IsImage(string str)
    {
        bool isimage = false;
        string thestr = str.ToLower();
        //限定只能上传jpg和png图片
        string[] allowExtension = { ".jpg", ".png" };
        //对上传的文件的类型进行一个个匹对
        for (int i = 0; i < allowExtension.Length; i++)
        {
            if (thestr == allowExtension[i])
            {
                isimage = true;
                break;
            }
        }
        return isimage;
    }

    //----------------------------------------------------
    // ● 加密图片路径
    //----------------------------------------------------
    protected string CreatePasswordHash(string pwd, int saltLenght)
    {
        string strSalt = CreateSalt(saltLenght);
        //把密码和Salt连起来
        string saltAndPwd = string.Concat(pwd, strSalt);
        //对密码进行哈希
        string hashenPwd = FormsAuthentication.HashPasswordForStoringInConfigFile(saltAndPwd, "sha1");
        //转为小写字符并截取前16个字符串
        hashenPwd = hashenPwd.ToLower().Substring(0, 16);
        //返回哈希后的值
        return hashenPwd;
    }

    //----------------------------------------------------
    // ● 创建随机Salt值
    //----------------------------------------------------
    protected string CreateSalt(int saltLenght)
    {
        //生成一个加密的随机数
        RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider();
        byte[] buff = new byte[saltLenght];
        rng.GetBytes(buff);
        //返回一个Base64随机数的字符串
        return Convert.ToBase64String(buff);
    }

    //----------------------------------------------------
    // ● 提交图书修改
    //----------------------------------------------------
    protected void BookSubmitBtn_Click(object sender, EventArgs e)
    {
        bool isEdit = false;
        if (((Button)sender).Text == "确认修改")
            isEdit = true;
        string warn = "";
        string order = "";
        if (bookName.Text == "")
            warn += "书名，";
        if (bookAuthor.Text == "")
            warn += "作者，";
        if (bookType.Text == "")
            warn += "类型，";
        if (bookPrice.Text == "")
            warn += "价格，";
        if (bookRemain.Text == "")
            warn += "货余量，";
        if (isEdit)
        {
            order = "UPDATE bookshelf SET name=@name, command=@command, introduce=@intro, author=@author, translator=@trans, type=@type, publisher=@publisher, pubdate=@pubdate, price=@price, size=@size, papertype=@paper, issuite=@suit, picturepath=@image, remain=@remain WHERE id = @id;";
        }
        else
        {
            if (bookIntro.Text == "")
                warn += "图书简介，";
            if (bookPublisher.Text == "")
                warn += "出版社，";
            if (bookPubyear.Text == "" || bookPubmonth.Text == "" || bookPubday.Text == "")
                warn += "出版日期，";
            if (bookSize.Text == "")
                warn += "书本尺寸，";
            if (bookPaper.Text == "")
                warn += "纸型，";
            order = "INSERT INTO bookshelf(name, command, introduce, author, translator, type, publisher, pubdate, price, size, papertype, issuite, picturepath, remain) VALUES(@name, @command, @intro, @author, @trans, @type, @publisher, @pubdate, @price, @size, @paper, @suit, @image, @remain);";
        }
        if (warn != "")
        {
            warn += "不可为空。";
            this.Page.ClientScript.RegisterStartupScript(this.Page.GetType(), "message", "<script language='javascript' defer>alert('" + warn + "');</script>");
            return;
        }
        try
        {
            SQLcon.Open();
            MySqlCommand SQLcmd = new MySqlCommand(order, SQLcon);
            SQLcmd.Parameters.Add(new MySqlParameter("@name", bookName.Text));
            SQLcmd.Parameters.Add(new MySqlParameter("@command", bookCommand.Text));
            SQLcmd.Parameters.Add(new MySqlParameter("@intro", bookIntro.Text));
            SQLcmd.Parameters.Add(new MySqlParameter("@author", bookAuthor.Text));
            SQLcmd.Parameters.Add(new MySqlParameter("@trans", bookTranslator.Text));
            SQLcmd.Parameters.Add(new MySqlParameter("@type", bookType.Text));
            SQLcmd.Parameters.Add(new MySqlParameter("@publisher", bookPublisher.Text));
            SQLcmd.Parameters.Add(new MySqlParameter("@pubdate", new DateTime(Convert.ToInt32(bookPubyear.Text), Convert.ToInt32(bookPubmonth.Text), Convert.ToInt32(bookPubday.Text))));
            SQLcmd.Parameters.Add(new MySqlParameter("@price", bookPrice.Text));
            SQLcmd.Parameters.Add(new MySqlParameter("@size", bookSize.Text));
            SQLcmd.Parameters.Add(new MySqlParameter("@paper", bookPaper.Text));
            if (suite.Checked)
                SQLcmd.Parameters.Add(new MySqlParameter("@suit", "1"));
            else
                SQLcmd.Parameters.Add(new MySqlParameter("@suit", "0"));
            SQLcmd.Parameters.Add(new MySqlParameter("@image", bookImg.ImageUrl));
            SQLcmd.Parameters.Add(new MySqlParameter("@remain", bookRemain.Text));
            if (isEdit)
                SQLcmd.Parameters.Add(new MySqlParameter("@id", bookID.Text));
            if (SQLcmd.ExecuteNonQuery() != 0)
            {
                if (isEdit)
                    this.Page.ClientScript.RegisterStartupScript(this.Page.GetType(), "message", "<script language='javascript' defer>alert('" + "修改成功" + "');</script>");
                else
                    this.Page.ClientScript.RegisterStartupScript(this.Page.GetType(), "message", "<script language='javascript' defer>alert('" + "添加成功" + "');</script>");
            }
        }
        catch (Exception ex)
        {
            throw ex;
        }
        finally
        {
            SQLcon.Close();
            BindBookData();
        }
    }

    //----------------------------------------------------
    // ● 返回全部图书概览按钮
    //----------------------------------------------------
    protected void ReturnBtn_Click(object sender, EventArgs e)
    {
        SearchText.Text = "";
        BindBookData();
    }

    //----------------------------------------------------
    // ● 批量删除
    //----------------------------------------------------
    protected void DelAllBtn_Click(object sender, EventArgs e)
    {
        foreach (GridViewRow dgi in bookGridView.Rows)
        {
            CheckBox cb = (CheckBox)dgi.FindControl("cbSelect");
            if (cb.Checked)
            {
                int id = Convert.ToInt32(dgi.Cells[1].Text);
                DeleteBook(id);
            }
        }
        BindBookData();
        //this.Page.ClientScript.RegisterStartupScript(this.Page.GetType(), "message", "<script language='javascript' defer>alert('" + "ok" + "');</script>");
    }

    //----------------------------------------------------
    // ● 填充年份下拉列表
    //----------------------------------------------------
    protected void FillYearList(int start)
    {
        bookPubyear.Items.Clear();
        for (int i = start - 100; i <= start + 2; i++)
        {
            bookPubyear.Items.Add(i.ToString());
        }
    }

    //----------------------------------------------------
    // ● 填充月份列表
    //----------------------------------------------------
    protected void FillMonthList()
    {
        bookPubmonth.Items.Clear();
        for (int i = 1; i <= 12; i++)
        {
            bookPubmonth.Items.Add(i.ToString());
        }
    }

    //----------------------------------------------------
    // ● 填充天数列表
    //----------------------------------------------------
    protected void FillDayList(int y, int m)
    {
        bookPubday.Items.Clear();
        int d = DateTime.DaysInMonth(y, m);
        for (int i = 1; i <= d; i++)
        {
            bookPubday.Items.Add(i.ToString());
        }
    }

    //----------------------------------------------------
    // ● 选择年份后重置天数列表
    //----------------------------------------------------
    protected void YearList_SelectedIndexChanged(object sender, EventArgs e)
    {
        int year = Int32.Parse(bookPubyear.SelectedValue);
        int month = bookPubmonth.SelectedIndex + 1;
        FillDayList(year, month);
    }

    //----------------------------------------------------
    // ● 选择月份后重置天数列表
    //----------------------------------------------------
    protected void MonthList_SelectedIndexChanged(object sender, EventArgs e)
    {
        int year = Int32.Parse(bookPubyear.SelectedValue);
        int month = bookPubmonth.SelectedIndex + 1;
        FillDayList(year, month);
    }

//=============================================================

    //----------------------------------------------------
    // ● 绑定订单信息
    //----------------------------------------------------
    private void BindOrderData()
    {
        string order = "";
        if (OrderSearchText.Text != "")
        {
            switch (OrderSearchType.SelectedIndex)
            {
                case 0:
                    order = "SELECT id,username,date FROM orderform WHERE username LIKE CONCAT('%',@content,'%');";
                    break;
                case 1:
                    order = "SELECT id,username,date FROM orderform WHERE id = @content;";
                    break;
                default:
                    order = "SELECT id,username,date FROM orderform WHERE username LIKE CONCAT('%',@content,'%');";
                    break;
            }
        }
        else
        {
            order = "SELECT id,username,date FROM orderform;";
        }
        DataSet ds = new DataSet();
        try
        {
            SQLcon.Open();
            MySqlCommand SQLcmd = new MySqlCommand(order, SQLcon);
            if (OrderSearchText.Text != "")
            {
                SQLcmd.Parameters.Add(new MySqlParameter("@content", OrderSearchText.Text));
            }
            MySqlDataAdapter sda = new MySqlDataAdapter(SQLcmd);
            sda.Fill(ds);
            orderGridView.DataSource = ds;
            orderGridView.DataBind();
        }
        catch (Exception e)
        {
            throw e;
        }
        finally
        {
            SQLcon.Close();
        }
    }

    //----------------------------------------------------
    // ● 批量删除订单
    //----------------------------------------------------
    protected void DelAllOrder_Click(object sender, EventArgs e)
    {
        foreach (GridViewRow dgi in orderGridView.Rows)
        {
            CheckBox cb = (CheckBox)dgi.FindControl("cbSelect");
            if (cb.Checked)
            {
                int id = Convert.ToInt32(dgi.Cells[1].Text);
                DeleteOrder(id);
            }
        }
        BindOrderData();
    }

    //----------------------------------------------------
    // ● 查询订单
    //----------------------------------------------------
    protected void OrderSearchBtn_Click(object sender, EventArgs e)
    {
        DataSet ds = new DataSet();
        string order = "";
        switch (OrderSearchType.SelectedIndex)
        {
            case 0:
                order = "SELECT id,username,date FROM orderform WHERE username LIKE CONCAT('%',@content,'%');";
                break;
            case 1:
                order = "SELECT id,username,date FROM orderform WHERE id = @content;";
                break;
            default:
                order = "SELECT id,username,date FROM orderform WHERE username LIKE CONCAT('%',@content,'%');";
                break;
        }
        try
        {
            SQLcon.Open();
            MySqlCommand SQLcmd = new MySqlCommand(order, SQLcon);
            SQLcmd.Parameters.Add(new MySqlParameter("@content", OrderSearchText.Text));
            MySqlDataAdapter sda = new MySqlDataAdapter(SQLcmd);
            sda.Fill(ds);
            orderGridView.DataSource = ds;
            orderGridView.DataBind();
        }
        catch (Exception ex)
        {
            throw ex;
        }
        finally
        {
            SQLcon.Close();
        }
    }

    //----------------------------------------------------
    // ● 返回订单主界面
    //----------------------------------------------------
    protected void OrderReturnBtn_Click(object sender, EventArgs e)
    {
        OrderSearchText.Text = "";
        BindOrderData();
    }

    //----------------------------------------------------
    // ● 订单信息分页事件
    //----------------------------------------------------
    protected void OrderGridView_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        orderGridView.PageIndex = e.NewPageIndex;
        BindOrderData();
    }

    //----------------------------------------------------
    // ● 为订单页删除按钮添加确认事件
    //----------------------------------------------------
    protected void OrderGridView_RowCreated(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            ((Button)e.Row.Cells[4].Controls[0]).Attributes.Add("onclick", "if(!window.confirm('确认要删除吗？')) return false;");
        }
    }

    //----------------------------------------------------
    // ● 图书信息命令事件
    //----------------------------------------------------
    protected void OrderGridView_OnRowCommand(object sender, GridViewCommandEventArgs e)
    {
        int index = Convert.ToInt32(e.CommandArgument);
        int id = Convert.ToInt32(orderGridView.Rows[index].Cells[1].Text);
        if (e.CommandName == "DelOrder")
        {
            DeleteOrder(id);
            BindOrderData();
        }
        else if (e.CommandName == "CheckOrder")
        {
            GetOrderDetail(id);
        }
    }

    //----------------------------------------------------
    // ● 删除订单
    //----------------------------------------------------
    private void DeleteOrder(int id)
    {
        string order = "DELETE FROM orderform WHERE id = @id";
        try
        {
            SQLcon.Open();
            MySqlCommand SQLcmd = new MySqlCommand(order, SQLcon);
            SQLcmd.Parameters.Add(new MySqlParameter("@id", id));
            SQLcmd.ExecuteNonQuery();
        }
        catch (Exception ex)
        {
            throw ex;
        }
        finally
        {
            SQLcon.Close();
        }
    }

    //----------------------------------------------------
    // ● 关闭订单详情界面
    //----------------------------------------------------
    protected void CloseOrderDetailPanel(object sender, EventArgs e)
    {
        orderDetailPanel.Visible = false;
        orderGridPanel.Visible = true;
    }

    //----------------------------------------------------
    // ● 获取订单详情
    //----------------------------------------------------
    private void GetOrderDetail(int id)
    {
        orderGridPanel.Visible = false;
        orderDetailPanel.Visible = true;
        while (OrderDetailTable.Rows.Count > 1)
        {
            OrderDetailTable.Rows.Remove(OrderDetailTable.Rows[1]);
        }
        string order = "SELECT id,username,books,date FROM orderform WHERE id = @id;";
        string books = "";
        try
        {
            SQLcon.Open();
            MySqlCommand SQLcmd = new MySqlCommand(order, SQLcon);
            SQLcmd.Parameters.Add(new MySqlParameter("@id", id));
            MySqlDataReader reader = SQLcmd.ExecuteReader();
            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    OrderID.Text = "订单号：" + reader.GetInt32(0).ToString();
                    OrderDate.Text = "订单日期：" + reader.GetDateTime(3).ToLongDateString() + " " + reader.GetDateTime(3).ToLongTimeString();
                    books = reader.GetString(2);
                    OrderUser.Text = "下单用户：" + reader.GetString(1);
                }
            }
            reader.Close();
        }
        catch (Exception e)
        {
            throw e;
        }
        finally
        {
            SQLcon.Close();
            if (books != "")
            {
                double totprice = 0;
                int totcnt = 0;
                List<ShoppingItem> itemlist = AnalyzeBookString(books);
                foreach (ShoppingItem item in itemlist)
                {
                    TableRow tr = new TableRow();

                    TableCell tc1 = new TableCell();
                    tc1.Text = item.Name;

                    TableCell tc2 = new TableCell();
                    tc2.Text = "￥" + item.Price.ToString("f2");
                    tc2.HorizontalAlign = HorizontalAlign.Center;

                    TableCell tc3 = new TableCell();
                    totcnt += item.Count;
                    tc3.Text = item.Count.ToString();
                    tc3.HorizontalAlign = HorizontalAlign.Center;

                    TableCell tc4 = new TableCell();
                    totprice += item.Price * item.Count;
                    tc4.Text = "￥" + (item.Price * item.Count).ToString("f2");
                    tc4.HorizontalAlign = HorizontalAlign.Center;

                    tr.Cells.Add(tc1);
                    tr.Cells.Add(tc2);
                    tr.Cells.Add(tc3);
                    tr.Cells.Add(tc4);

                    OrderDetailTable.Rows.Add(tr);
                }
                TotalCount.Text = "共" + totcnt + "件商品，";
                TotalPrice.Text = "总价：￥" + totprice.ToString("f2");

                string str = "";
                str += "导出日期：" + DateTime.Now.ToLongDateString() + " " + DateTime.Now.ToLongTimeString() + "\r\n";
                str += OrderDate.Text + " " + OrderID.Text + " " + OrderUser.Text + "\r\n";
                str += "消费详情：\r\n";
                for (int i = 1; i < OrderDetailTable.Rows.Count; i += 1)
                {
                    TableRow tr = OrderDetailTable.Rows[i];
                    str += "书名：" + tr.Cells[0].Text + " ";
                    str += "单价：" + tr.Cells[1].Text + " ";
                    str += "数量：" + tr.Cells[2].Text + " ";
                    str += "单项总价：" + tr.Cells[3].Text;
                    str += "\r\n";
                }
                str += TotalCount.Text + TotalPrice.Text;
                ExportText.Value = str;
            }
        }
    }

    //----------------------------------------------------
    // ● 解析订单图书列表字符串
    //----------------------------------------------------
    private List<ShoppingItem> AnalyzeBookString(string str)
    {
        List<string> tl = new List<string> { };
        tl = str.Split(',').ToList();
        List<ShoppingItem> itemlist = new List<ShoppingItem> { };
        for (int i = 0; i < tl.Count; i += 4)
        {
            string name = tl[i];
            int cnt = Convert.ToInt32(tl[i + 1]);
            double price = Convert.ToDouble(tl[i + 2]);
            ShoppingItem tmp = new ShoppingItem(name, price, cnt, "", -1);
            itemlist.Add(tmp);
        }
        return itemlist;
    }

    //----------------------------------------------------
    // ● 导出订单信息
    //----------------------------------------------------
    protected void ExportOrderDetail(object sender, EventArgs e)
    {
        Response.Clear();
        Response.Buffer = true;
        Response.Charset = "GB2312";
        Response.ContentEncoding = System.Text.Encoding.UTF8;
        Response.AddHeader("Content-Disposition", "attachment;filename=" + Server.UrlEncode("消费明细" + OrderUser.Text + " " + OrderDate.Text + ".txt"));
        Response.ContentType = "text/plain";
        Response.Write(ExportText.Value.ToString());
        Response.End();
    }

//==============================================================

    //----------------------------------------------------
    // ● 绑定用户信息
    //----------------------------------------------------
    private void BindUserData()
    {
        string order = "";
        if (UserSearchText.Text != "")
        {
            switch (UserSearchType.SelectedIndex)
            {
                case 0:
                    order = "SELECT id,username,idcard,phonenum,islocked,(case islocked when 0 then '可用' else '封禁' end) as accountstatus FROM users WHERE username LIKE CONCAT('%',@content,'%');";
                    break;
                case 1:
                    order = "SELECT id,username,idcard,phonenum,islocked,(case islocked when 0 then '可用' else '封禁' end) as accountstatus FROM users WHERE id = @content;";
                    break;
                case 2:
                    order = "SELECT id,username,idcard,phonenum,islocked,(case islocked when 0 then '可用' else '封禁' end) as accountstatus FROM users WHERE idcard = @content;";
                    break;
                case 3:
                    order = "SELECT id,username,idcard,phonenum,islocked,(case islocked when 0 then '可用' else '封禁' end) as accountstatus FROM users WHERE phonenum = @content;";
                    break;
                default:
                    order = "SELECT id,username,idcard,phonenum,islocked,(case islocked when 0 then '可用' else '封禁' end) as accountstatus FROM users WHERE username LIKE CONCAT('%',@content,'%');";
                    break;
            }
        }
        else
        {
            order = "SELECT id,username,idcard,phonenum,islocked,(case islocked when 0 then '可用' else '封禁' end) as accountstatus FROM users;";
        }
        DataSet ds = new DataSet();
        try
        {
            SQLcon.Open();
            MySqlCommand SQLcmd = new MySqlCommand(order, SQLcon);
            if (UserSearchText.Text != "")
            {
                SQLcmd.Parameters.Add(new MySqlParameter("@content", UserSearchText.Text));
            }
            MySqlDataAdapter sda = new MySqlDataAdapter(SQLcmd);
            sda.Fill(ds);
            userGridView.DataSource = ds;
            userGridView.DataBind();
        }
        catch (Exception e)
        {
            throw e;
        }
        finally
        {
            SQLcon.Close();
        }
    }

    //----------------------------------------------------
    // ● 批量删除用户
    //----------------------------------------------------
    protected void DelAllUser_Click(object sender, EventArgs e)
    {
        foreach (GridViewRow dgi in userGridView.Rows)
        {
            CheckBox cb = (CheckBox)dgi.FindControl("cbSelect");
            if (cb.Checked)
            {
                int id = Convert.ToInt32(dgi.Cells[1].Text);
                DeleteUser(id);
            }
        }
        BindUserData();
    }

    //----------------------------------------------------
    // ● 查询用户
    //----------------------------------------------------
    protected void UserSearchBtn_Click(object sender, EventArgs e)
    {
        DataSet ds = new DataSet();
        string order = "";
        switch (UserSearchType.SelectedIndex)
        {
            case 0:
                order = "SELECT id,username,idcard,phonenum,islocked,(case islocked when 0 then '可用' else '封禁' end) as accountstatus FROM users WHERE username LIKE CONCAT('%',@content,'%');";
                break;
            case 1:
                order = "SELECT id,username,idcard,phonenum,islocked,(case islocked when 0 then '可用' else '封禁' end) as accountstatus FROM users WHERE id = @content;";
                break;
            case 2:
                order = "SELECT id,username,idcard,phonenum,islocked,(case islocked when 0 then '可用' else '封禁' end) as accountstatus FROM users WHERE idcard = @content;";
                break;
            case 3:
                order = "SELECT id,username,idcard,phonenum,islocked,(case islocked when 0 then '可用' else '封禁' end) as accountstatus FROM users WHERE phonenum = @content;";
                break;
            default:
                order = "SELECT id,username,idcard,phonenum,islocked,(case islocked when 0 then '可用' else '封禁' end) as accountstatus FROM users WHERE username LIKE CONCAT('%',@content,'%');";
                break;
        }
        try
        {
            SQLcon.Open();
            MySqlCommand SQLcmd = new MySqlCommand(order, SQLcon);
            SQLcmd.Parameters.Add(new MySqlParameter("@content", UserSearchText.Text));
            MySqlDataAdapter sda = new MySqlDataAdapter(SQLcmd);
            sda.Fill(ds);
            userGridView.DataSource = ds;
            userGridView.DataBind();
        }
        catch (Exception ex)
        {
            throw ex;
        }
        finally
        {
            SQLcon.Close();
        }
    }

    //----------------------------------------------------
    // ● 返回用户主界面
    //----------------------------------------------------
    protected void UserReturnBtn_Click(object sender, EventArgs e)
    {
        UserSearchText.Text = "";
        BindUserData();
    }

    //----------------------------------------------------
    // ● 用户信息分页事件
    //----------------------------------------------------
    protected void UserGridView_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        userGridView.PageIndex = e.NewPageIndex;
        BindUserData();
    }

    //----------------------------------------------------
    // ● 为用户页删除按钮添加确认事件
    //----------------------------------------------------
    protected void UserGridView_RowCreated(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            ((Button)e.Row.Cells[6].Controls[0]).Attributes.Add("onclick", "if(!window.confirm('确认要删除吗？')) return false;");
        }
    }

    //----------------------------------------------------
    // ● 用户信息命令事件
    //----------------------------------------------------
    protected void UserGridView_OnRowCommand(object sender, GridViewCommandEventArgs e)
    {
        int index = Convert.ToInt32(e.CommandArgument);
        int id = Convert.ToInt32(userGridView.Rows[index].Cells[1].Text);
        if (e.CommandName == "DelUser")
        {
            DeleteUser(id);
            BindUserData();
        }
        else if (e.CommandName == "EditUser")
        {
            GetUserDetail(id);
            BindUserData();
        }
    }

    //----------------------------------------------------
    // ● 删除用户
    //----------------------------------------------------
    private void DeleteUser(int id)
    {
        string order = "DELETE FROM users WHERE id = @id";
        try
        {
            SQLcon.Open();
            MySqlCommand SQLcmd = new MySqlCommand(order, SQLcon);
            SQLcmd.Parameters.Add(new MySqlParameter("@id", id));
            SQLcmd.ExecuteNonQuery();
        }
        catch (Exception ex)
        {
            throw ex;
        }
        finally
        {
            SQLcon.Close();
        }
    }

    //----------------------------------------------------
    // ● 关闭用户详情界面
    //----------------------------------------------------
    protected void CloseUserDetailPanel(object sender, EventArgs e)
    {
        userDetailPanel.Visible = false;
        userGridPanel.Visible = true;
    }

    //----------------------------------------------------
    // ● 获取用户详情
    //----------------------------------------------------
    private void GetUserDetail(int id)
    {
        userDetailPanel.Visible = true;
        userGridPanel.Visible = false;
        string order = "SELECT id, username, password, point, avatarpath, idcard, phonenum, islocked FROM users WHERE id = @id;";
        try
        {
            SQLcon.Open();
            MySqlCommand SQLcmd = new MySqlCommand(order, SQLcon);
            SQLcmd.Parameters.Add(new MySqlParameter("@id", id));
            MySqlDataReader reader = SQLcmd.ExecuteReader();
            if(reader.HasRows)
            {
                int flag = 0;
                while(reader.Read())
                {
                    UIDTxb.Text = reader.GetInt32(0).ToString();
                    UsernameTxb.Text = reader.GetString(1);
                    PasswordTxb.Text = reader.GetString(2);
                    CreditTxb.Text = reader.GetInt32(3).ToString();
                    UserImage.ImageUrl = reader.GetString(4);
                    IDcardTxb.Text = reader.GetString(5);
                    PhoneNumTxb.Text = reader.GetString(6);
                    flag = reader.GetInt32(7);
                }
                if(flag == 0)
                {
                    AccountStatus.Text = "可用";
                    LockUserBtn.Text = "封禁用户";
                    AccountStatus.ForeColor = System.Drawing.Color.ForestGreen;
                }
                else
                {
                    AccountStatus.Text = "封禁";
                    LockUserBtn.Text = "解封用户";
                    AccountStatus.ForeColor = System.Drawing.Color.Red;
                }
            }
            reader.Close();
        }
        catch (Exception e)
        {
            throw e;
        }
        finally
        {
            SQLcon.Close();
        }
    }

    //----------------------------------------------------
    // ● 重置用户头像
    //----------------------------------------------------
    protected void ReSetImg(object sender, EventArgs e)
    {
        UserImage.ImageUrl = "~/App_Pic/default_userimg.jpg";
    }

    //----------------------------------------------------
    // ● 更改帐号状态
    //----------------------------------------------------
    protected void ChangeLockStatus(object sender, EventArgs e)
    {
        if (AccountStatus.Text == "可用")
        {
            AccountStatus.Text = "封禁";
            LockUserBtn.Text = "解封用户";
            AccountStatus.ForeColor = System.Drawing.Color.Red;
        }
        else
        {
            AccountStatus.Text = "可用";
            LockUserBtn.Text = "封禁用户";
            AccountStatus.ForeColor = System.Drawing.Color.ForestGreen;
        }
    }

    //----------------------------------------------------
    // ● 重置用户用户名
    //----------------------------------------------------
    protected void ResetUsername(object sender, EventArgs e)
    {
        bool flag = false;
        int i = 0;
        string res = "";
        do
        {
            string str = "user" + UIDTxb.Text + i.ToString();
            string modiname = CreatePasswordHash(str, 4);
            string order = "SELECT id FROM users WHERE username = @name;";
            try
            {
                SQLcon.Open();
                MySqlCommand SQLcmd = new MySqlCommand(order, SQLcon);
                SQLcmd.Parameters.Add(new MySqlParameter("@name", modiname));
                MySqlDataReader reader = SQLcmd.ExecuteReader();
                if(!reader.HasRows)
                {
                    res = modiname;
                    flag = true;
                }
                reader.Close();
                i += 1;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                SQLcon.Close();
            }
        } while (flag == false);
        UsernameTxb.Text = res;
    }

    //----------------------------------------------------
    // ● 重置用户密码
    //----------------------------------------------------
    protected void ResetPassword(object sender, EventArgs e)
    {
        PasswordTxb.Text = "123456";
    }

    //----------------------------------------------------
    // ● 提交用户更改
    //----------------------------------------------------
    protected void UserSubmitBtn_Click(object sender, EventArgs e)
    {
        string warn = "";
        if (IDcardTxb.Text == "")
            warn += "身份证号，";
        if (PhoneNumTxb.Text == "")
            warn += "电话号码，";
        if (CreditTxb.Text == "")
            warn += "用户积分，";
        if (warn != "")
        {
            warn += "不能为空！";
            this.Page.ClientScript.RegisterStartupScript(this.Page.GetType(), "empty", "<script language='javascript' defer>alert('" + warn + "');</script>");
            return;
        }
        else
        {
            string status = "";
            if (AccountStatus.Text == "封禁")
                status = "1";
            else
                status = "0";
            string order = "UPDATE users SET username=@username, password=@password, point=@point, avatarpath=@path, idcard=@idcard, phonenum=@phone, islocked=@status WHERE id = @id;";
            try
            {
                SQLcon.Open();
                MySqlCommand SQLcmd = new MySqlCommand(order, SQLcon);
                SQLcmd.Parameters.Add(new MySqlParameter("@username", UsernameTxb.Text));
                SQLcmd.Parameters.Add(new MySqlParameter("@password", PasswordTxb.Text));
                SQLcmd.Parameters.Add(new MySqlParameter("@point", CreditTxb.Text));
                SQLcmd.Parameters.Add(new MySqlParameter("@path", UserImage.ImageUrl));
                SQLcmd.Parameters.Add(new MySqlParameter("@idcard", IDcardTxb.Text));
                SQLcmd.Parameters.Add(new MySqlParameter("@phone", PhoneNumTxb.Text));
                SQLcmd.Parameters.Add(new MySqlParameter("@status", status));
                SQLcmd.Parameters.Add(new MySqlParameter("@id", UIDTxb.Text));
                if (SQLcmd.ExecuteNonQuery() != 0)
                {
                    this.Page.ClientScript.RegisterStartupScript(this.Page.GetType(), "message", "<script language='javascript' defer>alert('" + "修改成功" + "');</script>");
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                SQLcon.Close();
                BindUserData();
            }
        }
    }

    //----------------------------------------------------
    // ● 退出登录
    //----------------------------------------------------
    protected void AdminLogout(object sender, EventArgs e)
    {
        if (Request.Cookies["admin"] != null)
        {
            Response.Cookies["admin"].Expires = DateTime.Now.AddDays(-1);
            isLogin = false;
            Response.Redirect("~/adminlogin.aspx");
        }
    }
}
