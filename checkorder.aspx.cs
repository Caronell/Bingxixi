using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using MySql.Data.MySqlClient;

public partial class checkorder : System.Web.UI.Page
{
    private MySqlConnection SQLcon = new MySqlConnection(MySQLConnectionManager.GetConnectionString());
    private ShoppingCart billcart = new ShoppingCart();

    //----------------------------------------------------
    // ● 加载后执行
    //----------------------------------------------------
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Request.Cookies["username"] == null || Request.Cookies["username"].Value == null)
        {
            Response.Redirect("~/login.aspx");
        }
        if (Request.Cookies["orderlist"] == null || Request.Cookies["orderlist"].Value == null)
        {
            this.Page.ClientScript.RegisterStartupScript(this.Page.GetType(), "message", "<script language='javascript' defer>alert('" + "页面已过期，请重新下单。" + "');</script>");
            Response.Redirect("~/bingxixi.aspx");
        }
        GetOrderForm();
        if (!IsPostBack)
        {
            
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
    // ● 获取订单
    //----------------------------------------------------
    private void GetOrderForm()
    {
        if (Request.Cookies["orderlist"] != null && Request.Cookies["orderlist"].Value != null)
        {
            string result = Request.Cookies["orderlist"].Value;
            byte[] b = Convert.FromBase64String(result);
            MemoryStream ms = new MemoryStream(b, 0, b.Length);
            BinaryFormatter bf = new BinaryFormatter();
            billcart = bf.Deserialize(ms) as ShoppingCart;
            OrderCheckMain.SetViewContent(billcart.GetAllItems());
            Pricelbl.Text = billcart.GetTotalPrice().ToString("f2");
        }
    }

    //----------------------------------------------------
    // ● 取消并返回
    //----------------------------------------------------
    protected void CancelBtn_Click(object sender, EventArgs e)
    {
        DeleteOrderCookies();
        Response.Redirect("~/bingxixi.aspx");
    }

    //----------------------------------------------------
    // ● 确认下单
    //----------------------------------------------------
    protected void OKBtn_Click(object sender, EventArgs e)
    {
        bool reallybuy = false;
        string uname = Request.Cookies["username"].Value;
        string cartname = uname + "cart";
        ShoppingCart cart = null;
        if (Session[cartname] == null)
        {
            cart = new ShoppingCart();
            Session[cartname] = cart;
        }
        else
        {
            cart = (ShoppingCart)Session[cartname];
        }

        List<ShoppingItem> OutOfStockList = new List<ShoppingItem> { };
        string books = "";
        foreach (ShoppingItem tmp in billcart.GetAllItems())
        {
            if (IsEnough(tmp))
                books += tmp.Name + "," + tmp.Count + "," + tmp.Price + "," + tmp.Path + ",";
            else
                OutOfStockList.Add(tmp);
        }
        if (OutOfStockList.Count != 0)
        {
            string warn = "以下商品货源不足，已自动移除：";
            foreach (ShoppingItem item in OutOfStockList)
            {
                warn += "《" + item.Name + "》";
                warn += " 数量" + item.Count + "，";
            }
            OutOfStockList.Clear();
            this.Page.ClientScript.RegisterStartupScript(this.Page.GetType(), "message", "<script language='javascript' defer>alert('" + warn + "');</script>");
        }
        if (books != "")
        {
            books = books.Remove(books.Length - 1, 1);
            string order = "INSERT INTO orderform(username, books, date) VALUES(@username,@books,@date);";
            try
            {
                SQLcon.Open();
                MySqlCommand SQLcmd = new MySqlCommand(order, SQLcon);
                SQLcmd.Parameters.Add(new MySqlParameter("@username", Request.Cookies["username"].Value));
                SQLcmd.Parameters.Add(new MySqlParameter("@books", books));
                SQLcmd.Parameters.Add(new MySqlParameter("@date", DateTime.Now));
                if(SQLcmd.ExecuteNonQuery() != 0)
                {
                    DeleteOrderCookies();
                    foreach(ShoppingItem tmp in billcart.GetAllItems())
                    {
                        ReduceRemain(tmp.Id, tmp.Count);
                        ShoppingItem rel = cart.GetShoppingItemByName(tmp.Name);
                        cart.Remove(rel);
                    }
                    reallybuy = true;
                }
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
        if(reallybuy)
        {
            AddUserCredit(uname);
            billcart.GetAllItems().Clear();
            billcart = null;
            this.Page.ClientScript.RegisterStartupScript(this.Page.GetType(), "message", "<script language='javascript' defer>alert('" + "下单成功，感谢您的购买！" + "'); location = 'bingxixi.aspx'</script>");
        }
    }

    //----------------------------------------------------
    // ● 删除Cookies
    //----------------------------------------------------
    private void DeleteOrderCookies()
    {
        if (Request.Cookies["orderlist"] != null)
        {
            Response.Cookies["orderlist"].Expires = DateTime.Now.AddDays(-1);
        }
    }

    //----------------------------------------------------
    // ● 增加用户积分
    //----------------------------------------------------
    private void AddUserCredit(string uname)
    {
        int credit = 0;
        string order = "SELECT point FROM users WHERE username = @username;";
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
                }
            }
            reader.Close();
            credit += (int)billcart.GetTotalPrice() / 10;
            order = "UPDATE users SET point = @credit WHERE username = @username;";

            SQLcmd.CommandText = order;
            SQLcmd.Parameters.Add(new MySqlParameter("@credit", credit));
            SQLcmd.ExecuteNonQuery();
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
    // ● 扣除存货
    //----------------------------------------------------
    private void ReduceRemain(int id, int cnt)
    {
        string order = "SELECT remain FROM bookshelf WHERE id = @id;";
        int remain = 0;
        try
        {
            MySqlCommand SQLcmd = new MySqlCommand(order, SQLcon);
            SQLcmd.Parameters.Add(new MySqlParameter("@id", id));
            MySqlDataReader reader = SQLcmd.ExecuteReader();
            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    remain = reader.GetInt32(0);
                }
            }
            reader.Close();
            remain -= cnt;
            if (remain < 0)
                remain = 0;
            order = "UPDATE bookshelf SET remain=@remain WHERE id=@id;";
            SQLcmd.CommandText = order;
            SQLcmd.Parameters.Add(new MySqlParameter("@remain", remain));
            SQLcmd.ExecuteNonQuery();
        }
        catch (Exception e)
        {
            throw e;
        }
    }

    //----------------------------------------------------
    // ● 检查存货是否足够
    //----------------------------------------------------
    private bool IsEnough(ShoppingItem item)
    {
        string order = "SELECT remain FROM bookshelf WHERE id = @id;";
        bool flag = false;
        int remain = 0;
        try
        {
            SQLcon.Open();
            MySqlCommand SQLcmd = new MySqlCommand(order, SQLcon);
            SQLcmd.Parameters.Add(new MySqlParameter("@id", item.Id));
            MySqlDataReader reader = SQLcmd.ExecuteReader();
            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    remain = reader.GetInt32(0);
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
        if (item.Count <= remain)
            flag = true;
        return flag;
    }
}