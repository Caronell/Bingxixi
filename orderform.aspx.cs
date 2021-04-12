using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class orderform : System.Web.UI.Page
{
    private MySqlConnection SQLcon = new MySqlConnection(MySQLConnectionManager.GetConnectionString());

    //----------------------------------------------------
    // ● 加载后执行
    //----------------------------------------------------
    protected void Page_Load(object sender, EventArgs e)
    {
        if(!IsPostBack)
        {
            
        }
        else
        {
            if (CartPanel.Visible == true)
            {
                CartPanel.ShowCartItems();
            }
        }
        if (Request.Cookies["username"] == null || Request.Cookies["username"].Value == null)
        {
            Response.Redirect("~/login.aspx");
        }
        else
        {
            MainPanel.Controls.Clear();
            AddOrderView();
        }
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
        CartPanel.Visible = true;
        CartPanel.ShowCartItems();
    }

    //----------------------------------------------------
    // ● 添加订单单元
    //----------------------------------------------------
    private void AddOrderView()
    {
        List<OrderForm> oflist = new List<OrderForm> { };
        string order = "SELECT id,username,books,date FROM orderform WHERE username = @nowuser;";
        try
        {
            SQLcon.Open();
            MySqlCommand SQLcmd = new MySqlCommand(order, SQLcon);
            SQLcmd.Parameters.Add(new MySqlParameter("@nowuser", Request.Cookies["username"].Value));
            MySqlDataReader reader = SQLcmd.ExecuteReader();
            if(reader.HasRows)
            {
                while(reader.Read())
                {
                    OrderForm of = new OrderForm();
                    of.ID = reader.GetInt32(0);
                    of.User = reader.GetString(1);
                    of.Books = reader.GetString(2);
                    of.Date = reader.GetDateTime(3);
                    oflist.Add(of);
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
        //解析字符串，倒序添加
        for (int i = oflist.Count - 1; i >= 0; i -= 1)
        {
            OrderForm tmp = oflist[i];
            List<ShoppingItem> itemlist = AnalyzeBookString(tmp.Books);
            OrderView newview = (OrderView)LoadControl("OrderView.ascx");
            newview.SetViewContent(tmp, itemlist);
            MainPanel.Controls.Add(newview);
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
            string path = tl[i + 3];
            ShoppingItem tmp = new ShoppingItem(name, price, cnt, path, -1);
            itemlist.Add(tmp);
        }
        return itemlist;
    }
}