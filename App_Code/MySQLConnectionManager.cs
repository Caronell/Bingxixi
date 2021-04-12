using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;

/// <summary>
/// MySQLConnectionManager 的摘要说明
/// </summary>
public class MySQLConnectionManager
{
    private MySQLConnectionManager()
    {
        //
        // TODO: 在此处添加构造函数逻辑
        //
    }

    public static string GetConnectionString()
    {
        return ConfigurationManager.AppSettings["constr"];
    }
}