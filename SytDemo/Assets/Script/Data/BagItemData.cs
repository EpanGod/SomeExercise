using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public static class BagItemData
{
    private static List<Item> Items = new List<Item>();

    /// <summary>
    /// 添加物品
    /// </summary>
    /// <returns>物品数量</returns>
    public static int AddItem(Item item)
    {
        Items.Add(item);
        return Items.Count;
    }

    /// <summary>
    /// 删除物品
    /// </summary>
    /// <returns>物品数量</returns>
    public static int DeleteItem(Item item)
    {
        Items.Remove(item);
        return Items.Count;
    }

    /// <summary>
    /// 修改物品
    /// </summary>
    /// <returns>物品数量</returns>
    public static Item ChangeItem(int index, Item item)
    {
        Items[index] = item;
        return item;
    }

    /// <summary>
    /// 获取物品数量
    /// </summary>
    /// <returns>物品数量</returns>
    public static int GetNum()
    {
        return Items.Count;
    }

    // <summary>
    /// 获取物品
    /// </summary>
    /// <returns>物品数量</returns>
    public static Item GetItem(int index)
    {
        return Items[index];
    }
}

public class Item
{
    private string name;
    private int index;
    public Item(string name, int index)
    {
        this.name = name;
        this.index = index;
    }
    public string Name
    {
        set { name = value; }
        get { return name; }
    }
    public int Index
    {
        get { return index; }
    }
    public void destroy()
    {
        name = null;
        index = -1;
    }
}
