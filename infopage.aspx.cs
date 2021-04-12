using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using MySql.Data.MySqlClient;

public partial class infopage : System.Web.UI.Page
{
    private MySqlConnection SQLcon = new MySqlConnection(MySQLConnectionManager.GetConnectionString());
    private bool isLogin = true;

    //----------------------------------------------------
    // ● 加载后执行
    //----------------------------------------------------
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Request.Cookies["username"] == null || Request.Cookies["username"].Value == null)
        {
            isLogin = false;
            Response.Redirect("~/login.aspx");
        }
        this.Pwdtxb.Attributes["value"] = this.Pwdtxb.Text;
        this.NewPwdtxb.Attributes["value"] = this.NewPwdtxb.Text;
        if (!IsPostBack)
        {
            if (isLogin)
                GetUserData(Request.Cookies["username"].Value);
        }
        else
        {
            FinalCheck();
            Page.Validate();
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
        if (isLogin)
        {
            CartPanel.Visible = true;
            CartPanel.ShowCartItems();
        }
        else
            Response.Redirect("~/login.aspx");
    }

    //----------------------------------------------------
    // ● 查看我的订单
    //----------------------------------------------------
    protected void MyOrderFormBtn_Click(object sender, EventArgs e)
    {
        if (isLogin)
            Response.Redirect("~/orderform.aspx");
        else
            Response.Redirect("~/login.aspx");
    }

    //----------------------------------------------------
    // ● 获取用户信息
    //----------------------------------------------------
    private void GetUserData(string user)
    {
        if (user != null && user != "")
        {
            string order = "SELECT id, username, point, avatarpath, idcard, phonenum FROM users WHERE username = @username;";
            try
            {
                SQLcon.Open();
                MySqlCommand SQLcmd = new MySqlCommand(order, SQLcon);
                SQLcmd.Parameters.Add(new MySqlParameter("@username", user));
                MySqlDataReader reader = SQLcmd.ExecuteReader();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        UIDlbl.Text = reader.GetInt32(0).ToString();
                        Usernamelbl.Text = reader.GetString(1);
                        UserCredit.Text = reader.GetInt32(2).ToString();
                        UserImg.ImageUrl = reader.GetString(3);
                        UserIDcardlbl.Text = reader.GetString(4);
                        UserPhonetxb.Text = reader.GetString(5);
                    }
                }
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
        else
            Response.Redirect("~/login.aspx");
    }

    //----------------------------------------------------
    // ● 上传图片
    //----------------------------------------------------
    protected void UploadImg(object sender, EventArgs e)
    {
        bool fileOk = false;
        if (ImgUploader.HasFile)//验证是否包含文件
        {
            //取得文件的扩展名,并转换成小写
            string fileExtension = Path.GetExtension(ImgUploader.FileName).ToLower();
            //验证上传文件是否图片格式
            fileOk = IsImage(fileExtension);
            if (fileOk)
            {
                //对上传文件的大小进行检测，限定文件最大不超过8M
                if (ImgUploader.PostedFile.ContentLength < 8192000)
                {
                    string filepath = "/book_pic/";
                    if (Directory.Exists(Server.MapPath(filepath)) == false)//如果不存在就创建file文件夹
                    {
                        Directory.CreateDirectory(Server.MapPath(filepath));
                    }
                    string virpath = filepath + CreatePasswordHash(ImgUploader.FileName, 4) + fileExtension;//这是存到服务器上的虚拟路径
                    string mappath = Server.MapPath(virpath);//转换成服务器上的物理路径
                    ImgUploader.PostedFile.SaveAs(mappath);//保存图片

                    UserImg.ImageUrl = virpath;
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
    // ● 随机图片路径
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
    // ● 确认按钮——提交修改
    //----------------------------------------------------
    protected void OKBtn_Click(object sender, EventArgs e)
    {
        if (Page.IsValid && Pwdtxb.Text != "" && PwdErrorMsg.Visible == false)
        {

            bool flag = false;
            try
            {
                SQLcon.Open();
                string order = "";
                if (NewPwdtxb.Text != "")
                    order = "UPDATE users SET password=@password, avatarpath=@path, phonenum=@phone WHERE id = @id;";
                else
                    order = "UPDATE users SET avatarpath=@path, phonenum=@phone WHERE id = @id;";
                MySqlCommand SQLcmd = new MySqlCommand(order, SQLcon);
                if (NewPwdtxb.Text != "")
                    SQLcmd.Parameters.Add(new MySqlParameter("@password", NewPwdtxb.Text));
                SQLcmd.Parameters.Add(new MySqlParameter("@path", UserImg.ImageUrl));
                SQLcmd.Parameters.Add(new MySqlParameter("@phone", UserPhonetxb.Text));
                SQLcmd.Parameters.Add(new MySqlParameter("@id", UIDlbl.Text));
                if (SQLcmd.ExecuteNonQuery() != 0)
                {
                    flag = true;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                SQLcon.Close();
                if (flag)
                    this.Page.ClientScript.RegisterStartupScript(this.Page.GetType(), "message", "<script language='javascript' defer>alert('" + "修改完成" + "'); location = 'bingxixi.aspx'</script>");
            }
        }
        else
            PwdErrorMsg.Visible = true;
    }

    //----------------------------------------------------
    // ● 验证原密码正确性
    //----------------------------------------------------
    protected void FinalCheck()
    {
        if (Pwdtxb.Text == "")
            return;
        string order = "SELECT password FROM users WHERE id = @id;";
        try
        {
            string pwd = "";
            SQLcon.Open();
            MySqlCommand SQLcmd = new MySqlCommand(order, SQLcon);
            SQLcmd.Parameters.Add(new MySqlParameter("@id", UIDlbl.Text));
            MySqlDataReader reader = SQLcmd.ExecuteReader();
            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    pwd = reader.GetString(0);
                }
                if (pwd == Pwdtxb.Text)
                    PwdErrorMsg.Visible = false;
                else
                    PwdErrorMsg.Visible = true;
            }
            else
                PwdErrorMsg.Visible = true;
        }
        catch (Exception ex)
        {
            PwdErrorMsg.Visible = true;
            throw ex;
        }
        finally
        {
            SQLcon.Close();
        }
    }
}