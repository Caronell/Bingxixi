using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class BookView : System.Web.UI.UserControl
{
    private int id;

    //----------------------------------------------------
    // ● 加载后执行
    //----------------------------------------------------
    protected void Page_Load(object sender, EventArgs e)
    {

    }

    //----------------------------------------------------
    // ● 设置图书属性
    //----------------------------------------------------
    public void SetBookInfo(Book bk)
    {
        id = bk.Id;
        realBookname.Text = bk.Name;
        bookname.Text = bk.Name;
        if (bk.Command != "")
            bookname.Text += "（" + bk.Command + "）";
        bookauthor.Text = "作者：" + bk.Author + "著 ";
        if (bk.Translator != "")
            bookauthor.Text += bk.Translator + "译";
        bookpublish.Text = "出版社：" + bk.Publisher + "　　　出版时间：" + bk.Pubdate.ToShortDateString();
        bookintro.Text = bk.Introduce;
        bookprice.Text = "￥" + bk.Price.ToString("f2");
        bookimg.ImageUrl = bk.Picturepath;
        bookname.NavigateUrl = "~/bookpage.aspx?bookid=" + bk.Id.ToString();
        bookimg.PostBackUrl = "~/bookpage.aspx?bookid=" + bk.Id.ToString();
        if (bk.Remain <= 0)
        {
            addToCartBtn.Enabled = false;
            addToCartBtn.CssClass = "addtocart_btn_disable";
            addToCartBtn.Text = "已售罄";
        }
            
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
            string name = "《" + realBookname.Text + "》";
            double price = Convert.ToDouble(bookprice.Text.Substring(1));
            ShoppingItem newitem = new ShoppingItem(name, price, 1, bookimg.ImageUrl, id);
            cart.Add(newitem);
        }
    }
}