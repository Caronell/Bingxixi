using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// ShoppingCart 的摘要说明
/// </summary>
[Serializable]
public class ShoppingCart
{
    private List<ShoppingItem> Cart = null;

    //----------------------------------------------------
    // ● 构造函数
    //----------------------------------------------------
    public ShoppingCart()
    {
        Cart = new List<ShoppingItem> { };
    }

    //----------------------------------------------------
    // ● 按名称检索购物车内是否包含此物品
    //----------------------------------------------------
    public ShoppingItem GetShoppingItemByName(string name)
    {
        foreach(ShoppingItem tmp in Cart)
        {
            if(tmp.Name == name)
            {
                return tmp;
            }
        }
        return null;
    }

    //----------------------------------------------------
    // ● 添加物品
    //----------------------------------------------------
    public void Add(ShoppingItem tmp)
    {
        ShoppingItem si = GetShoppingItemByName(tmp.Name);
        if (si != null)
        {
            si.Count += tmp.Count;
        }
        else
        {
            Cart.Add(tmp);
        }
    }

    //----------------------------------------------------
    // ● 移除物品
    //----------------------------------------------------
    public void Remove(ShoppingItem tmp)
    {
        if(tmp != null && Cart.Contains(tmp))
        {
            Cart.Remove(tmp);
        }
    }

    //----------------------------------------------------
    // ● 计算总价
    //----------------------------------------------------
    public double GetTotalPrice()
    {
        double sum = 0;
        foreach(ShoppingItem tmp in Cart)
        {
            sum += tmp.Price * tmp.Count;
        }
        return sum;
    }

    //----------------------------------------------------
    // ● 获取购物车对象
    //----------------------------------------------------
    public List<ShoppingItem> GetAllItems()
    {
        return Cart;
    }
}