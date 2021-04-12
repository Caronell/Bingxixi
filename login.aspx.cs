using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using MySql.Data.MySqlClient;

public partial class _Default : System.Web.UI.Page
{
    private MySqlConnection SQLcon = new MySqlConnection(MySQLConnectionManager.GetConnectionString());

    //----------------------------------------------------
    // ● 加载后执行
    //----------------------------------------------------
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            robot.Value = "";
        }
        else
        {
            trans_box.Visible = false;
        }
    }

    //----------------------------------------------------
    // ● 点击登录
    //----------------------------------------------------
    protected void SubmitBtn_Click(object sender, EventArgs e)
    {
        if (CodeValid.Value != "ok")
        {
            CodeErrorText.Visible = true;
            return;
        }
        bool canlogin = false;
        string searchpwd = "";
        int status = 1;
        try
        {
            SQLcon.Open();
            string order = "SELECT password,islocked FROM users WHERE username = @username;";
            MySqlCommand SQLcmd = new MySqlCommand(order, SQLcon);
            SQLcmd.Parameters.Add(new MySqlParameter("@username", username.Text));
            MySqlDataReader reader = SQLcmd.ExecuteReader();
            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    searchpwd = reader.GetString(0);
                    status = reader.GetInt32(1);
                }
            }
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.Write(ex.Message);
        }
        finally
        {
            SQLcon.Close();
        }
        if (searchpwd == password.Text && status == 0)
            canlogin = true;
        if (canlogin)
        {
            HttpCookie usercookies = new HttpCookie("username", username.Text);
            usercookies.Expires = DateTime.Now.AddDays(7); //7天过期
            Response.Cookies.Add(usercookies);
            Response.Redirect("~/bingxixi.aspx");
        }
        else
        {
            if(status != 0)
                ErrorText.Text = "*该用户已被封禁";
            else
                ErrorText.Text = "*用户名或密码不正确";
        }
    }

    //----------------------------------------------------
    // ● 用户名后台验证
    //----------------------------------------------------
    protected void UsernameValidator_ServerValidate(object source, ServerValidateEventArgs args)
    {
        UpdateValidatorState();
        bool flag = true;
        if (NewUsername.Text == "")
        {
            RequiredFieldValidator1.IsValid = false;
            return;
        }
        try
        {
            SQLcon.Open();
            string order = "SELECT username FROM users WHERE username = @username;";
            MySqlCommand SQLcmd = new MySqlCommand(order, SQLcon);
            SQLcmd.Parameters.Add(new MySqlParameter("@username", NewUsername.Text));
            MySqlDataReader reader = SQLcmd.ExecuteReader();
            if (reader.HasRows)
            {
                flag = false;
            }
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.Write(ex.Message);
        }
        finally
        {
            SQLcon.Close();
        }
        if (flag)
            args.IsValid = true;
        else
        {
            args.IsValid = false;
        }
    }

    //----------------------------------------------------
    // ● 点击注册
    //----------------------------------------------------
    protected void RegisterBtn_Click(object sender, EventArgs e)
    {
        ReValidate();
        if (RegisterIsValid())
        {
            try
            {
                SQLcon.Open();
                string order = "INSERT INTO users(username, password, point, avatarpath, idcard, phonenum, islocked) VALUES(@username, @password, 0, '~/App_Pic/default_userimg.jpg', @idcard, @phone, 0);";
                MySqlCommand SQLcmd = new MySqlCommand(order, SQLcon);
                SQLcmd.Parameters.Add(new MySqlParameter("@username", NewUsername.Text));
                SQLcmd.Parameters.Add(new MySqlParameter("@password", NewPassword.Text));
                SQLcmd.Parameters.Add(new MySqlParameter("@idcard", IDnumber.Text));
                SQLcmd.Parameters.Add(new MySqlParameter("@phone", PhoneNum.Text));
                if(SQLcmd.ExecuteNonQuery() != 0)
                {
                    this.Page.ClientScript.RegisterStartupScript(this.Page.GetType(), "message", "<script language='javascript' defer>alert('" + "注册成功" + "');</script>");
                    Response.Redirect("Login.aspx");
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.Write(ex.Message);
            }
            finally
            {
                SQLcon.Close();
            }
        }
    }

    //----------------------------------------------------
    // ● 服务器端二次验证
    //----------------------------------------------------
    private void ReValidate()
    {
        this.NewPassword.Attributes["value"] = this.NewPassword.Text;
        this.RepeatPwd.Attributes["value"] = this.RepeatPwd.Text;
        if (NewUsername.Text == "")
        {
            UsernameValidator.IsValid = false;
        }
        if (NewPassword.Text.Length < 6 || NewPassword.Text == "")
        {
            PwdValidator.IsValid = false;
        }
        if (NewPassword.Text != RepeatPwd.Text)
            RepeatPwdValidator.IsValid = false;
        if (!Regex.Match(PhoneNum.Text, "^1([38]\\d|5[0-35-9]|7[3678])\\d{8}$").Success || PhoneNum.Text == "")
        {
            PhoneNumValidator.IsValid = false;
        }
        if (!CheckIDCard18(IDnumber.Text))
        {
            IDnumberValidator.IsValid = false;
        }
    }

    //----------------------------------------------------
    // ● 全部信息是否已合法
    //----------------------------------------------------
    private bool RegisterIsValid()
    {
        if (!UsernameValidator.IsValid)
        {
            return false;
        }
        if (!PwdValidator.IsValid)
        {
            return false;
        }
        if (!RepeatPwdValidator.IsValid)
        {
            return false;
        }
        if (!PhoneNumValidator.IsValid)
        {
            return false;
        }
        if (!IDnumberValidator.IsValid)
        {
            return false;
        }
        if (robot.Value != "ok")
        {
            return false;
        }
        return true;
    }

    //----------------------------------------------------
    // ● 通过CSS值判断验证状态是否合法
    //----------------------------------------------------
    private bool CSSValueToBool(string value)
    {
        if (value == "none" || value == "hidden")
            return true;
        else
            return false;
    }

    //----------------------------------------------------
    // ● 二次更新验证控件状态（抗丢失）
    //----------------------------------------------------
    private void UpdateValidatorState()
    {
        this.NewPassword.Attributes["value"] = this.NewPassword.Text;
        this.RepeatPwd.Attributes["value"] = this.RepeatPwd.Text;
        PwdValidator.IsValid = CSSValueToBool(passwordState.Value);
        PhoneNumValidator.IsValid = CSSValueToBool(phonenumState.Value);
        IDnumberValidator.IsValid = CSSValueToBool(IDnumState.Value);
    }

    //----------------------------------------------------
    // ● 身份证号码验证
    //----------------------------------------------------
    private static bool CheckIDCard18(string Id)
    {
        long n = 0;
        if (long.TryParse(Id.Remove(17), out n) == false || n < Math.Pow(10, 16) || long.TryParse(Id.Replace('x', '0').Replace('X', '0'), out n) == false)
        {
            return false;//数字验证
        }
        string address = "11x22x35x44x53x12x23x36x45x54x13x31x37x46x61x14x32x41x50x62x15x33x42x51x63x21x34x43x52x64x65x71x81x82x91";
        if (address.IndexOf(Id.Remove(2)) == -1)
        {
            return false;//省份验证
        }
        string birth = Id.Substring(6, 8).Insert(6, "-").Insert(4, "-");
        DateTime time = new DateTime();
        if (DateTime.TryParse(birth, out time) == false)
        {
            return false;//生日验证
        }
        string[] arrVarifyCode = ("1,0,x,9,8,7,6,5,4,3,2").Split(',');
        string[] Wi = ("7,9,10,5,8,4,2,1,6,3,7,9,10,5,8,4,2").Split(',');
        char[] Ai = Id.Remove(17).ToCharArray();
        int sum = 0;
        for (int i = 0; i < 17; i++)
        {
            sum += int.Parse(Wi[i]) * int.Parse(Ai[i].ToString());
        }
        int y = -1;
        Math.DivRem(sum, 11, out y);
        if (arrVarifyCode[y] != Id.Substring(17, 1).ToLower())
        {
            return false;//校验码验证
        }
        return true;//符合GB11643-1999标准
    }
}