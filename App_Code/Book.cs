using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Book 的摘要说明
/// </summary>
public class Book
{
    private int _Id;
    private string _Name;
    private string _Command;
    private string _Introduce;
    private string _Author;
    private string _Translator;
    private string _Type;
    private string _Publisher;
    private DateTime _Pubdate;
    private float _Price;
    private string _Size;
    private string _Papertype;
    private int _Issuite;
    private string _Picturepath;
    private int _Remain;

    public Book()
    {
        
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

    public string Name
    {
        get
        {
            return _Name;
        }

        set
        {
            _Name = value;
        }
    }

    public string Command
    {
        get
        {
            return _Command;
        }

        set
        {
            _Command = value;
        }
    }

    public string Introduce
    {
        get
        {
            return _Introduce;
        }

        set
        {
            _Introduce = value;
        }
    }

    public string Author
    {
        get
        {
            return _Author;
        }

        set
        {
            _Author = value;
        }
    }

    public string Translator
    {
        get
        {
            return _Translator;
        }

        set
        {
            _Translator = value;
        }
    }

    public string Type
    {
        get
        {
            return _Type;
        }

        set
        {
            _Type = value;
        }
    }

    public DateTime Pubdate
    {
        get
        {
            return _Pubdate;
        }

        set
        {
            _Pubdate = value;
        }
    }

    public float Price
    {
        get
        {
            return _Price;
        }

        set
        {
            _Price = value;
        }
    }

    public string Size
    {
        get
        {
            return _Size;
        }

        set
        {
            _Size = value;
        }
    }

    public string Papertype
    {
        get
        {
            return _Papertype;
        }

        set
        {
            _Papertype = value;
        }
    }

    public int Issuite
    {
        get
        {
            return _Issuite;
        }

        set
        {
            _Issuite = value;
        }
    }

    public string Picturepath
    {
        get
        {
            return _Picturepath;
        }

        set
        {
            _Picturepath = value;
        }
    }

    public int Remain
    {
        get
        {
            return _Remain;
        }

        set
        {
            _Remain = value;
        }
    }

    public string Publisher
    {
        get
        {
            return _Publisher;
        }

        set
        {
            _Publisher = value;
        }
    }
}