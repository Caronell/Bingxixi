using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class bookpage : System.Web.UI.Page
{
    private MySqlConnection SQLcon = new MySqlConnection(MySQLConnectionManager.GetConnectionString());

    //----------------------------------------------------
    // ● 加载后执行
    //----------------------------------------------------
    protected void Page_Load(object sender, EventArgs e)
    {
        if(!IsPostBack)
        {
            GetBookInfo(Request["bookid"]);
        }
        else
        {
            if (CartPanel.Visible == true)
            {
                CartPanel.ShowCartItems();
            }
        }
    }

    //----------------------------------------------------
    // ● 获取书籍信息
    //----------------------------------------------------
    private void GetBookInfo(string id)
    {
        string order = "SELECT name,command,introduce,author,translator,publisher,pubdate,price,picturepath,size,papertype,issuite,remain,type,id FROM bookshelf WHERE id = @id;";
        Book bk = null;
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
                    bk = new Book();
                    bk.Name = reader.GetString(0);
                    bk.Command = reader.IsDBNull(1) ? "" : reader.GetString(1);
                    bk.Introduce = reader.GetString(2);
                    bk.Author = reader.GetString(3);
                    bk.Translator = reader.IsDBNull(4) ? "" : reader.GetString(4);
                    bk.Publisher = reader.GetString(5);
                    bk.Pubdate = reader.GetDateTime(6);
                    bk.Price = reader.GetFloat(7);
                    bk.Picturepath = reader.IsDBNull(8) ? "" : reader.GetString(8);
                    bk.Size = reader.GetString(9);
                    bk.Papertype = reader.GetString(10);
                    bk.Issuite = reader.GetInt32(11);
                    bk.Remain = reader.GetInt32(12);
                    bk.Type = reader.GetString(13);
                    bk.Id = reader.GetInt32(14);
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
        if(bk != null)
        {
            bookid.Text = bk.Id.ToString();
            Bookname.Text = bk.Name;
            Booktype.Text = bk.Type;
            middlePic.ImageUrl = bk.Picturepath;
            bigPic.ImageUrl = bk.Picturepath;
            bookTitle.Text = bk.Name;
            if(bk.Command != "")
                bookTitle.Text += "（" + bk.Command + "）";
            bookIntro.Text = bk.Introduce;
            bookAuthor.Text = "作者：" + bk.Author + " 著 ";
            if(bk.Translator != "")
                bookAuthor.Text += bk.Translator + " 译";
            bookPublisher.Text = "出版社：" + bk.Publisher;
            bookDate.Text = "出版时间：" + bk.Pubdate.ToLongDateString();
            bookSize.Text = "开 本：" + bk.Size;
            bookPage.Text = "纸 张：" + bk.Papertype;
            isSuite.Text = "是否套装：";
            if (bk.Issuite == 0)
                isSuite.Text += "否";
            else
                isSuite.Text += "是";
            bookPrice.Text = "￥" + bk.Price.ToString("f2");
            bookRemain.Text = "（库存 " + bk.Remain.ToString() + " 本）";
            remainBookNum.Value = bk.Remain.ToString();
            if (bk.Remain <= 0)
            {
                AddToCart.Enabled = false;
                AddToCart.Text = "已售罄";
                AddToCart.CssClass = "addto_cart_btn_disable";
                BuyNow.Enabled = false;
                BuyNow.Text = "已售罄";
                BuyNow.CssClass = "buynow_btn_disable";
            }
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
    // ● 添加物品到购物车
    //----------------------------------------------------
    protected void Addtocart_Click(object sender, EventArgs e)
    {
        if (Request.Cookies["username"] == null)
            Response.Redirect("~/login.aspx");
        else
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
            int count = Convert.ToInt32(buyNumber.Text);
            string name = "《" + Bookname.Text + "》";
            double price = Convert.ToDouble(bookPrice.Text.Substring(1));
            ShoppingItem newitem = new ShoppingItem(name, price, count, middlePic.ImageUrl,Convert.ToInt32(bookid.Text));
            cart.Add(newitem);
            buyNumber.Text = "1";
            this.Page.ClientScript.RegisterStartupScript(this.Page.GetType(), "message", "<script language='javascript' defer>alert('已添加');</script>");
        }
    }
}