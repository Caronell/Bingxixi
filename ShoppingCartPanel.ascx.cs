using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class ShoppingCartPanel : System.Web.UI.UserControl
{
    private MySqlConnection SQLcon = new MySqlConnection(MySQLConnectionManager.GetConnectionString());

    //----------------------------------------------------
    // ● 加载后执行
    //----------------------------------------------------
    protected void Page_Load(object sender, EventArgs e)
    {
        
    }
    
    //----------------------------------------------------
    // ● 展示购物车
    //----------------------------------------------------
    public void ShowCartItems()
    {
        ShoppingCart cart = null;
        string cartname = Request.Cookies["username"].Value + "cart";
        if (Session[cartname] == null)
        {
            cart = new ShoppingCart();
            Session[cartname] = cart;
        }
        else
        {
            cart = (ShoppingCart)Session[cartname];
        }
        cart_Table.Rows.Clear();
        foreach (ShoppingItem tmp in cart.GetAllItems())
        {
            TableRow newrow = new TableRow();
            newrow.CssClass = "cart_item_line";

            TableCell tc0 = new TableCell();
            CheckBox cb = new CheckBox();
            tc0.Controls.Add(cb);

            TableCell tc1 = new TableCell();
            tc1.CssClass = "cart_bookname_box";
            System.Web.UI.WebControls.Image bookimg = new System.Web.UI.WebControls.Image();
            bookimg.ImageUrl = tmp.Path;
            bookimg.CssClass = "cart_book_img";
            Label bookname = new Label();
            bookname.CssClass = "cart_book_name";
            bookname.Text = tmp.Name;
            tc1.Controls.Add(bookimg);
            tc1.Controls.Add(bookname);

            TableCell tc2 = new TableCell();
            tc2.CssClass = "cart_book_price_box";
            tc2.Text = "￥" + tmp.Price.ToString("f2");

            TableCell tc3 = new TableCell();
            tc3.CssClass = "cart_book_count_box";
            tc3.Text = tmp.Count.ToString();

            TableCell tc4 = new TableCell();
            tc4.CssClass = "cart_operation_box";
            Button rmvbtn = new Button();
            rmvbtn.ID = "r" + tmp.Name;
            rmvbtn.Text = "－";
            rmvbtn.CssClass = "cart_oper_btn_left";
            rmvbtn.Click += new EventHandler(RemoveOneItem);
            Button delbtn = new Button();
            delbtn.ID = "d" + tmp.Name;
            delbtn.Text = "删除";
            delbtn.CssClass = "cart_oper_btn_mid";
            delbtn.Click += new EventHandler(RemoveButton_Click);
            Button addbtn = new Button();
            addbtn.ID = "a" + tmp.Name;
            addbtn.Text = "＋";
            addbtn.CssClass = "cart_oper_btn_right";
            addbtn.Click += new EventHandler(AddOneItem);
            tc4.Controls.Add(rmvbtn);
            tc4.Controls.Add(delbtn);
            tc4.Controls.Add(addbtn);

            newrow.Cells.Add(tc0);
            newrow.Cells.Add(tc1);
            newrow.Cells.Add(tc2);
            newrow.Cells.Add(tc3);
            newrow.Cells.Add(tc4);

            cart_Table.Rows.Add(newrow);
        }
        TotPrice.Text = "总价：￥" + cart.GetTotalPrice().ToString("f2");
        top_blackpanel.Visible = true;
        cart_panel.Visible = true;
        this.Visible = true;
    }

    //----------------------------------------------------
    // ● 清空购物车
    //----------------------------------------------------
    protected void ClearCartBtn_Click(object sender, EventArgs e)
    {
        string cartname = Request.Cookies["username"].Value + "cart";
        Session.Remove(cartname);
        ShowCartItems();
    }

    //----------------------------------------------------
    // ● 关闭购物车
    //----------------------------------------------------
    protected void CloseCart_Click(object sender, EventArgs e)
    {
        top_blackpanel.Visible = false;
        cart_panel.Visible = false;
        this.Visible = false;
    }

    //----------------------------------------------------
    // ● 删除购物车里的某物品
    //----------------------------------------------------
    protected void RemoveButton_Click(object sender, EventArgs e)
    {
        string cartname = Request.Cookies["username"].Value + "cart";
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
        ShoppingItem tmp = cart.GetShoppingItemByName(((Button)sender).ID.Substring(1));
        if (tmp != null)
        {
            cart.Remove(tmp);
        }
        ShowCartItems();
    }

    //----------------------------------------------------
    // ● 加一个物品
    //----------------------------------------------------
    protected void AddOneItem(object sender, EventArgs e)
    {
        string cartname = Request.Cookies["username"].Value + "cart";
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
        ShoppingItem tmp = cart.GetShoppingItemByName(((Button)sender).ID.Substring(1));
        if (tmp != null)
        {
            tmp.Count += 1;
        }
        ShowCartItems();
    }

    //----------------------------------------------------
    // ● 减一个物品
    //----------------------------------------------------
    protected void RemoveOneItem(object sender, EventArgs e)
    {
        string cartname = Request.Cookies["username"].Value + "cart";
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
        ShoppingItem tmp = cart.GetShoppingItemByName(((Button)sender).ID.Substring(1));
        if (tmp != null)
        {
            tmp.Count -= 1;
            if (tmp.Count <= 0)
            {
                cart.Remove(tmp);
            }
        }
        ShowCartItems();
    }

    //----------------------------------------------------
    // ● 结算
    //----------------------------------------------------
    protected void BillBtn_Click(object sender, EventArgs e)
    {
        string cartname = Request.Cookies["username"].Value + "cart";
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
        if (cart.GetAllItems().Count > 0)
        {
            List<ShoppingItem> OutOfStockList = new List<ShoppingItem> { };
            foreach (ShoppingItem tmp in cart.GetAllItems())
            {
                if (!IsEnough(tmp))
                {
                    OutOfStockList.Add(tmp);
                }
            }
            //移除没货的
            foreach (ShoppingItem tmp in OutOfStockList)
            {
                ShoppingItem rel = cart.GetShoppingItemByName(tmp.Name);
                cart.Remove(rel);
            }

            //序列化存储购物车，使用Cookies传递
            BinaryFormatter bf = new BinaryFormatter();
            MemoryStream ms = new MemoryStream();
            bf.Serialize(ms, cart);
            byte[] result = new byte[ms.Length];
            result = ms.ToArray();
            string temp = Convert.ToBase64String(result);
            ms.Flush();
            ms.Close();
            HttpCookie orderlist = new HttpCookie("orderlist");
            orderlist.Expires = DateTime.Now.AddMinutes(30); //30分钟有效期
            orderlist.Value = temp;
            Response.Cookies.Add(orderlist);

            top_blackpanel.Visible = false;
            cart_panel.Visible = false;
            this.Visible = false;
            if (OutOfStockList.Count != 0)
            {
                string warn = "以下商品货源不足，已自动移除：";
                foreach (ShoppingItem item in OutOfStockList)
                {
                    warn += "《" + item.Name + "》";
                    warn += " 数量" + item.Count + "，";
                }
                OutOfStockList.Clear();
                this.Page.ClientScript.RegisterStartupScript(this.Page.GetType(), "message", "<script language='javascript' defer>alert('" + warn + "'); location = 'checkorder.aspx'</script>");
            }
            else
                Response.Redirect("~/checkorder.aspx");
        }
        //this.Page.ClientScript.RegisterStartupScript(this.Page.GetType(), "message", "<script language='javascript' defer>alert('" + this.Parent.Page.Request.Path + "');</script>");
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
            if(reader.HasRows)
            {
                while(reader.Read())
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

    //----------------------------------------------------
    // ● 结算所选项
    //----------------------------------------------------
    protected void BillSelectBtn_Click(object sender, EventArgs e)
    {
        string cartname = Request.Cookies["username"].Value + "cart";
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
        ShoppingCart tmpcart = new ShoppingCart();
        if (cart.GetAllItems().Count > 0)
        {
            List<ShoppingItem> OutOfStockList = new List<ShoppingItem> { };
            foreach (TableRow tr in cart_Table.Rows)
            {
                if (((CheckBox)(tr.Cells[0].Controls[0])).Checked)
                {
                    string name = ((Label)tr.Cells[1].Controls[1]).Text;
                    ShoppingItem tmp = cart.GetShoppingItemByName(name);
                    if (!IsEnough(tmp))
                    {
                        OutOfStockList.Add(tmp);
                    }
                    else
                    {
                        tmpcart.Add(tmp);
                    }
                }
            }
            if(tmpcart.GetAllItems().Count > 0)
            {
                //移除没货的
                foreach (ShoppingItem tmp in OutOfStockList)
                {
                    ShoppingItem rel = cart.GetShoppingItemByName(tmp.Name);
                    cart.Remove(rel);
                }

                //序列化存储购物车，使用Cookies传递
                BinaryFormatter bf = new BinaryFormatter();
                MemoryStream ms = new MemoryStream();
                bf.Serialize(ms, tmpcart);
                byte[] result = new byte[ms.Length];
                result = ms.ToArray();
                string temp = Convert.ToBase64String(result);
                ms.Flush();
                ms.Close();
                HttpCookie orderlist = new HttpCookie("orderlist");
                orderlist.Expires = DateTime.Now.AddMinutes(30); //30分钟有效期
                orderlist.Value = temp;
                Response.Cookies.Add(orderlist);

                top_blackpanel.Visible = false;
                cart_panel.Visible = false;
                this.Visible = false;
                if (OutOfStockList.Count != 0)
                {
                    string warn = "以下商品货源不足，已自动移除：";
                    foreach (ShoppingItem item in OutOfStockList)
                    {
                        warn += "《" + item.Name + "》";
                        warn += " 数量" + item.Count + "，";
                    }
                    OutOfStockList.Clear();
                    this.Page.ClientScript.RegisterStartupScript(this.Page.GetType(), "message", "<script language='javascript' defer>alert('" + warn + "'); location = 'checkorder.aspx'</script>");
                }
                else
                    Response.Redirect("~/checkorder.aspx");
            }
        }
    }

    //----------------------------------------------------
    // ● 删除所选项
    //----------------------------------------------------
    protected void ClearSelectBtn_Click(object sender, EventArgs e)
    {
        string cartname = Request.Cookies["username"].Value + "cart";
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
        foreach(TableRow tr in cart_Table.Rows)
        {
            if(((CheckBox)(tr.Cells[0].Controls[0])).Checked)
            {
                string name = ((Label)tr.Cells[1].Controls[1]).Text;
                ShoppingItem tmp = cart.GetShoppingItemByName(name);
                cart.Remove(tmp);
            }
        }
        ShowCartItems();
    }

}