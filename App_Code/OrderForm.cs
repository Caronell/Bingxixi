using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// OrderForm 的摘要说明
/// </summary>
public class OrderForm
{
    private int _ID;
    private string _Books;
    private string _User;
    private DateTime _Date;

    public OrderForm()
    {
        
    }

    public int ID
    {
        get
        {
            return _ID;
        }

        set
        {
            _ID = value;
        }
    }

    public string Books
    {
        get
        {
            return _Books;
        }

        set
        {
            _Books = value;
        }
    }

    public string User
    {
        get
        {
            return _User;
        }

        set
        {
            _User = value;
        }
    }

    public DateTime Date
    {
        get
        {
            return _Date;
        }

        set
        {
            _Date = value;
        }
    }
}