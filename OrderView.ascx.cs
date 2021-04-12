using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class OrderView : System.Web.UI.UserControl
{
    //----------------------------------------------------
    // ● 加载后执行
    //----------------------------------------------------
    protected void Page_Load(object sender, EventArgs e)
    {

    }

    //----------------------------------------------------
    // ● 设置内容-确认订单
    //----------------------------------------------------
    public void SetViewContent(List<ShoppingItem> sl)
    {
        ExportBtn.Visible = false;
        OrderTable.Rows.Clear();
        orderDate.Text = "下单日期：" + DateTime.Now.ToLongDateString() + " " + DateTime.Now.ToLongTimeString();
        orderDate.Font.Bold = false;
        orderID.Text = "";
        double totprice = 0;
        int totcnt = 0;
        foreach (ShoppingItem tmp in sl)
        {
            totprice += tmp.Price * tmp.Count;
            totcnt += tmp.Count;

            TableRow tr = new TableRow();
            tr.CssClass = "order_row";

            TableCell tc1 = new TableCell();
            tc1.CssClass = "img_cell";
            Image bookimg = new Image();
            bookimg.ImageUrl = tmp.Path;
            bookimg.CssClass = "book_img";
            tc1.Controls.Add(bookimg);

            TableCell tc2 = new TableCell();
            tc2.CssClass = "bookname_cell";
            tc2.Text = tmp.Name;

            TableCell tc3 = new TableCell();
            tc3.CssClass = "bookcost_cell";
            Label costlbl = new Label();
            costlbl.CssClass = "book_price";
            costlbl.Text = "单价 ￥" + tmp.Price.ToString("f2");
            Label cntlbl = new Label();
            cntlbl.CssClass = "book_count";
            cntlbl.Text = "数量 ×" + tmp.Count;
            tc3.Controls.Add(costlbl);
            tc3.Controls.Add(cntlbl);

            TableCell tc4 = new TableCell();
            tc4.CssClass = "single_totcost_cell";
            tc4.Text = "单项总价：￥" + (tmp.Price * tmp.Count).ToString("f2");

            tr.Cells.Add(tc1);
            tr.Cells.Add(tc2);
            tr.Cells.Add(tc3);
            tr.Cells.Add(tc4);

            OrderTable.Rows.Add(tr);
        }
        totmoney.Text = "总价：￥" + totprice.ToString("f2");
        totcount.Text = "共 " + totcnt + " 件商品";
    }

    //----------------------------------------------------
    // ● 设置内容-历史订单
    //----------------------------------------------------
    public void SetViewContent(OrderForm of, List<ShoppingItem> sl)
    {
        OrderTable.Rows.Clear();
        orderDate.Text = "订单日期：" + of.Date.ToLongDateString() + " " + of.Date.ToLongTimeString ();
        orderDate.Font.Bold = false;
        orderID.Text = "订单号：" + of.ID.ToString();
        double totprice = 0;
        int totcnt = 0;
        foreach(ShoppingItem tmp in sl)
        {
            totprice += tmp.Price * tmp.Count;
            totcnt += tmp.Count;

            TableRow tr = new TableRow();
            tr.CssClass = "order_row";

            TableCell tc1 = new TableCell();
            tc1.CssClass = "img_cell";
            Image bookimg = new Image();
            bookimg.ImageUrl = tmp.Path;
            bookimg.CssClass = "book_img";
            tc1.Controls.Add(bookimg);

            TableCell tc2 = new TableCell();
            tc2.CssClass = "bookname_cell";
            tc2.Text = tmp.Name;
            
            TableCell tc3 = new TableCell();
            tc3.CssClass = "bookcost_cell";
            Label costlbl = new Label();
            costlbl.CssClass = "book_price";
            costlbl.Text = "单价 ￥" + tmp.Price.ToString("f2");
            Label cntlbl = new Label();
            cntlbl.CssClass = "book_count";
            cntlbl.Text = "数量 ×" + tmp.Count;
            tc3.Controls.Add(costlbl);
            tc3.Controls.Add(cntlbl);

            TableCell tc4 = new TableCell();
            tc4.CssClass = "single_totcost_cell";
            tc4.Text = "单项总价：￥" + (tmp.Price * tmp.Count).ToString("f2");

            tr.Cells.Add(tc1);
            tr.Cells.Add(tc2);
            tr.Cells.Add(tc3);
            tr.Cells.Add(tc4);

            OrderTable.Rows.Add(tr);
        }
        totmoney.Text = "总价：￥" + totprice.ToString("f2");
        totcount.Text = "共 " + totcnt + " 件商品";
    }

    //----------------------------------------------------
    // ● 导出订单信息
    //----------------------------------------------------
    protected void ExportBtn_Click(object sender, EventArgs e)
    {
        string str = "";
        str += "导出日期：" + DateTime.Now.ToLongDateString() + " " + DateTime.Now.ToLongTimeString() + "\r\n";
        str += orderDate.Text + " " + orderID.Text + "\r\n";
        str += "消费详情：\r\n";
        foreach(TableRow tr in OrderTable.Rows)
        {
            str += tr.Cells[1].Text;
            str += ((Label)tr.Cells[2].Controls[0]).Text;
            str += ((Label)tr.Cells[2].Controls[1]).Text;
            str += tr.Cells[3].Text;
            str += "\r\n";
        }
        str += totcount.Text + totmoney.Text;
        Response.Clear();
        Response.Buffer = true;
        Response.Charset = "GB2312";
        Response.ContentEncoding = System.Text.Encoding.UTF8;
        Response.AddHeader("Content-Disposition", "attachment;filename=" + Server.UrlEncode("消费明细" + orderDate.Text + ".txt"));
        Response.ContentType = "text/plain";
        Response.Write(str.ToString());
        Response.End();
    }
}