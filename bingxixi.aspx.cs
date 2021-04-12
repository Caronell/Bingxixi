using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Drawing;
using System.Configuration;
using MySql.Data.MySqlClient;

public partial class _Default : System.Web.UI.Page
{
    private MySqlConnection SQLcon = new MySqlConnection(MySQLConnectionManager.GetConnectionString());
    private int Nowpage;
    private int TotalPage;
    public bool isLogin = true;

    //----------------------------------------------------
    // ● 加载后执行
    //----------------------------------------------------
    protected void Page_Load(object sender, EventArgs e)
    {
        string limit = "";
        if (!IsPostBack)
        {
            Nowpage = 1;
            if (Request.Cookies["username"] != null && Request.Cookies["username"].Value != null)
            {
                GetUserInfo(Request.Cookies["username"].Value);
                username.Text = Request.Cookies["username"].Value;
                logoutBtn.Text = "退出登录";
                EditUserDataBtn.Visible = true;
                logoutBtn.Click += new EventHandler(LogoutBtn_Click);
            }
            else
            {
                username.Text = "未登录";
                EditUserDataBtn.Visible = false;
                isLogin = false;
            }
        }
        else
        {
            if (CartPanel.Visible == true)
            {
                CartPanel.ShowCartItems();
            }
        }
        if (username.Text == "未登录")
        {
            isLogin = false;
            logoutBtn.Visible = false;
            loginBtn.Visible = true;
        }
        else
        {
            isLogin = true;
            logoutBtn.Visible = true;
            loginBtn.Visible = false;
        }
        if (Request.QueryString["page"] != null)
            Nowpage = Convert.ToInt32(Request.QueryString["page"]);
        if (Request.QueryString["search"] != null)
            limit = Request.QueryString["search"];
        //防暴毙
        if (Nowpage <= 0)
            Nowpage = 1;
        pageNum.Text = "第 " + Nowpage.ToString() + " 页";
        FillBookshelf(Nowpage, limit);
        FillMenu();
        CalcPageNum(limit);
        SetPageJumpZone(TotalPage);
        SetPageBtnUrl();
    }

    //----------------------------------------------------
    // ● 填充书架
    //----------------------------------------------------
    private void FillBookshelf(int page, string limit = "")
    {
        string order = "SELECT id,name,command,introduce,author,translator,publisher,pubdate,price,picturepath,remain FROM bookshelf " + limit + " LIMIT @start, 10;";
        BookList.Controls.Clear();
        try
        {
            SQLcon.Open();
            MySqlCommand SQLcmd = new MySqlCommand(order, SQLcon);
            SQLcmd.Parameters.Add(new MySqlParameter("@start", (page - 1) * 10));
            MySqlDataReader reader = SQLcmd.ExecuteReader();
            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    Book bk = new Book();
                    bk.Id = reader.GetInt32(0);
                    bk.Name = reader.GetString(1);
                    bk.Command = reader.IsDBNull(2) ? "" : reader.GetString(2);
                    bk.Introduce = reader.GetString(3);
                    bk.Author = reader.GetString(4);
                    bk.Translator = reader.IsDBNull(5) ? "" : reader.GetString(5);
                    bk.Publisher = reader.GetString(6);
                    bk.Pubdate = reader.GetDateTime(7);
                    bk.Price = reader.GetFloat(8);
                    bk.Picturepath = reader.IsDBNull(9) ? "" : reader.GetString(9);
                    bk.Remain = reader.GetInt32(10);
                    BookView bv = (BookView)LoadControl("BookView.ascx");
                    bv.SetBookInfo(bk);
                    BookList.Controls.Add(bv);
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
    // ● 填充分类
    //----------------------------------------------------
    private void FillMenu()
    {
        string order = "SELECT DISTINCT type FROM bookshelf;";
        LeftMenu.Controls.Clear();
        try
        {
            SQLcon.Open();
            MySqlCommand SQLcmd = new MySqlCommand(order, SQLcon);
            MySqlDataReader reader = SQLcmd.ExecuteReader();
            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    Button typebtn = new Button();
                    typebtn.Text = reader.GetString(0);
                    typebtn.CssClass = "menuitem";
                    typebtn.PostBackUrl = "~/bingxixi.aspx?search=" + "WHERE type='" + typebtn.Text + "'";
                    LeftMenu.Controls.Add(typebtn);
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
    // ● 计算页数
    //----------------------------------------------------
    private void CalcPageNum(string limit = "")
    {
        int totpage = 1;
        int tmp;
        string order = "SELECT COUNT(*) FROM bookshelf " + limit + ";";
        try
        {
            SQLcon.Open();
            MySqlCommand SQLcmd = new MySqlCommand(order, SQLcon);
            MySqlDataReader reader = SQLcmd.ExecuteReader();
            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    tmp = reader.GetInt32(0);
                    totpage = tmp / 10;
                    if (tmp % 10 != 0)
                        totpage += 1;
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
        TotalPage = totpage;
    }

    //----------------------------------------------------
    // ● 设置页面跳转区
    //----------------------------------------------------
    private void SetPageJumpZone(int totpage)
    {
        string search = "";
        if (Request.QueryString["search"] != null)
            search = "&" + Request.QueryString["search"];
        pagejump.Controls.Clear();
        bool hasForeElip = false;
        bool hasBackElip = false;
        if (Nowpage == 1)
            firstpage.CssClass = "pagenumnow";
        else
            firstpage.CssClass = "pagenumbtn";
        for (int i = 2; i <= totpage; i += 1)
        {
            if (Math.Abs(Nowpage - i) > 3)
            {
                if (i < Nowpage && !hasForeElip)
                {
                    Label fe = new Label();
                    fe.Text = "...";
                    fe.CssClass = "pagenumelip";
                    hasForeElip = true;
                    pagejump.Controls.Add(fe);
                }
                else if (i > Nowpage && !hasBackElip)
                {
                    Label be = new Label();
                    be.Text = "...";
                    be.CssClass = "pagenumelip";
                    hasBackElip = true;
                    pagejump.Controls.Add(be);
                }
                continue;
            }
            else
            {
                Button pb = new Button();
                pb.Text = i.ToString();
                if (i == Nowpage)
                {
                    pb.CssClass = "pagenumnow";
                }
                else
                {
                    pb.CssClass = "pagenumbtn";
                }
                pb.PostBackUrl = "~/bingxixi.aspx?page=" + i.ToString() + search;
                pagejump.Controls.Add(pb);
            }
        }
        if (Math.Abs(Nowpage - totpage) > 3)
        {
            Button pb = new Button();
            pb.Text = totpage.ToString();
            pb.CssClass = "pagenumbtn";
            pb.PostBackUrl = "~/bingxixi.aspx?page=" + totpage.ToString() + search;
            pagejump.Controls.Add(pb);
        }
        if (Nowpage == 1)
        {
            prePage.Style["visibility"] = "hidden";
            prePageButtom.Visible = false;
        }
        else
        {
            prePage.Style["visibility"] = "visible";
            prePageButtom.Visible = true;
        }
        if (Nowpage == totpage)
        {
            nextPage.Visible = false;
            nextPageButtom.Visible = false;
        }
        else
        {
            nextPage.Visible = true;
            nextPageButtom.Visible = true;
        }
    }

    //----------------------------------------------------
    // ● 设置上/下一页的url
    //----------------------------------------------------
    private void SetPageBtnUrl()
    {
        string search = "";
        if (Request.QueryString["search"] != null)
            search = "&" + Request.QueryString["search"];
        nextPage.PostBackUrl = "~/bingxixi.aspx?page=" + (Nowpage + 1).ToString() + search;
        nextPageButtom.PostBackUrl = "~/bingxixi.aspx?page=" + (Nowpage + 1).ToString() + search;
        prePage.PostBackUrl = "~/bingxixi.aspx?page=" + (Nowpage - 1).ToString() + search;
        prePageButtom.PostBackUrl = "~/bingxixi.aspx?page=" + (Nowpage - 1).ToString() + search;
    }

    //----------------------------------------------------
    // ● 更新目标url
    //----------------------------------------------------
    protected void TargetPage_TextChanged(object sender, EventArgs e)
    {
        if (targetPage.Text == "0" || targetPage.Text == "")
            targetPage.Text = "1";
        else if (Convert.ToInt32(targetPage.Text) > TotalPage)
            targetPage.Text = TotalPage.ToString();
        jumpBtn.PostBackUrl = "~/bingxixi.aspx?page=" + targetPage.Text;
        if (Request.QueryString["search"] != null)
            jumpBtn.PostBackUrl += "&search=" + Request.QueryString["search"];
    }

    //----------------------------------------------------
    // ● 更新搜索目标url
    //----------------------------------------------------
    protected void SearchText_TextChanged(object sender, EventArgs e)
    {
        SearchBtn.PostBackUrl = "~/bingxixi.aspx?search=WHERE name LIKE '%" + SearchText.Text + "%'";
    }

    //----------------------------------------------------
    // ● 点击购物车按钮
    //----------------------------------------------------
    protected void Showcart_Click(object sender, EventArgs e)
    {
        if(isLogin)
        {
            CartPanel.Visible = true;
            CartPanel.ShowCartItems();
        }
        else
            Response.Redirect("~/login.aspx");
    }

    //----------------------------------------------------
    // ● 登出
    //----------------------------------------------------
    protected void LogoutBtn_Click(object sender, EventArgs e)
    {
        if(Request.Cookies["username"] != null)
        {
            Response.Cookies["username"].Expires = DateTime.Now.AddDays(-1);
            isLogin = false;
            Response.Redirect("~/bingxixi.aspx");
        }
    }

    //----------------------------------------------------
    // ● 点击登录
    //----------------------------------------------------
    protected void LoginBtn_Click(object sender, EventArgs e)
    {
        Response.Redirect("~/login.aspx");
    }

    //----------------------------------------------------
    // ● 查看我的订单
    //----------------------------------------------------
    protected void MyOrderFormBtn_Click(object sender, EventArgs e)
    {
        if(isLogin)
            Response.Redirect("~/orderform.aspx");
        else
            Response.Redirect("~/login.aspx");
    }

    //----------------------------------------------------
    // ● 获取用户积分
    //----------------------------------------------------
    private void GetUserInfo(string uname)
    {
        string order = "SELECT point,avatarpath FROM users WHERE username = @username;";
        int credit = 0;
        string url = "";
        try
        {
            SQLcon.Open();
            MySqlCommand SQLcmd = new MySqlCommand(order, SQLcon);
            SQLcmd.Parameters.Add(new MySqlParameter("@username", uname));
            MySqlDataReader reader = SQLcmd.ExecuteReader();
            if(reader.HasRows)
            {
                while(reader.Read())
                {
                    credit = reader.GetInt32(0);
                    url = reader.GetString(1);
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
            UserCredit.Text = credit.ToString();
            avater.ImageUrl = url;
        }
    }
}