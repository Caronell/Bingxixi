using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using MySql.Data.MySqlClient;

public partial class adminlogin : System.Web.UI.Page
{
    private MySqlConnection SQLcon = new MySqlConnection(MySQLConnectionManager.GetConnectionString());

    //----------------------------------------------------
    // ● 加载后执行
    //----------------------------------------------------
    protected void Page_Load(object sender, EventArgs e)
    {

    }

    //----------------------------------------------------
    // ● 执行登录
    //----------------------------------------------------
    protected void LoginBtn_Click(object sender, EventArgs e)
    {
        string order = "SELECT password FROM admins WHERE jobnumber = @jobnum;";
        string pwd = "";
        bool flag = false;
        try
        {
            SQLcon.Open();
            MySqlCommand SQLcmd = new MySqlCommand(order, SQLcon);
            SQLcmd.Parameters.Add(new MySqlParameter("@jobnum", loginName.Text));
            MySqlDataReader reader = SQLcmd.ExecuteReader();
            if(reader.HasRows)
            {
                while(reader.Read())
                {
                    pwd = reader.GetString(0);
                }
                if (password.Text == pwd)
                    flag = true;
            }
            reader.Close();
        }
        catch (Exception ex)
        {
            throw ex;
        }
        finally
        {
            SQLcon.Close();
            if(flag)
            {
                HttpCookie admincookies = new HttpCookie("admin", loginName.Text);
                admincookies.Expires = DateTime.Now.AddDays(7); //7天过期
                Response.Cookies.Add(admincookies);
                Response.Redirect("~/bxxsystem.aspx");
            }
            else
            {
                warnmsg.Visible = true;
            }
        }
    }
}