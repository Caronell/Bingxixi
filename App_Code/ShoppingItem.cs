using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// ShoppingItem 的摘要说明
/// </summary>
[Serializable]
public class ShoppingItem
{
    //----------------------------------------------------
    // ● 成员变量声明
    //----------------------------------------------------
    private string _Name;  //名称
    private double _Price; //价格
    private int _Count;    //数量
    private string _Path;  //图片路径
    private int _Id;       //ID

    //----------------------------------------------------
    // ● 构造函数
    //----------------------------------------------------
    public ShoppingItem(string n, double p, int c, string path, int id)
    {
        _Name = n;
        _Price = p;
        _Count = c;
        _Path = path;
        _Id = id;
    }

    //----------------------------------------------------
    // ● Name访问器
    //----------------------------------------------------
    public string Name
    {
        get
        {
            return _Name;
        }
    }

    //----------------------------------------------------
    // ● Price访问器
    //----------------------------------------------------
    public double Price
    {
        get
        {
            return _Price;
        }
    }

    //----------------------------------------------------
    // ● Count访问器
    //----------------------------------------------------
    public int Count
    {
        get
        {
            return _Count;
        }
        set
        {
            _Count = value;
        }
    }

    public string Path
    {
        get
        {
            return _Path;
        }

        set
        {
            _Path = value;
        }
    }

    public int Id
    {
        get
        {
            return _Id;
        }

        set
        {
            _Id = value;
        }
    }
}