using System;
using System.Collections.Generic;

/// <summary>
/// 物品类型
/// </summary>
[Serializable]
public enum ItemType
{
    NONE = -1,
    ADD_HP,         // 回血    
    ADD_POWER,      // 加攻击力（有时限）
    ADD_DEFEND,     // 加防御力（有时限）
    BUDDY1,         // 召唤伙伴1
    BUDDY2,         // 召唤伙伴2
    BUDDY3,         // 召唤伙伴3
}

/// <summary>
/// 背包物品数据
/// </summary>
[Serializable]
public class ItemDataInBag
{
    public int placeIdx;          // 背包位置
    public ItemType itemType;    
}

public class ItemMgr
{
    Dictionary<int, ItemDataInBag> _items = new Dictionary<int, ItemDataInBag>();

    public void Add(ItemDataInBag item)
    {
        if (HasItem(item.placeIdx))
        {
            return;
        }
        _items.Add(item.placeIdx, item);
    }

    public void Remove(int posIdx)
    {
        if (HasItem(posIdx) == false)
        {
            return;
        }
        _items.Remove(posIdx);
    }

    public void Exchange(int posIdx1, int posIdx2)
    {
        if (HasItem(posIdx1) == false)
        {
            return;
        }

        ItemDataInBag item1 = _items[posIdx1];
        if (HasItem(posIdx2) == false)
        {
            Remove(posIdx1);
            item1.placeIdx = posIdx2;
            Add(item1);
        }
        else
        {
            item1.placeIdx = posIdx2;
            ItemDataInBag item2 = _items[posIdx2];
            item2.placeIdx = posIdx1;
            _items[posIdx1] = item2;
            _items[posIdx2] = item1;            
        }
    }

    bool HasItem(int posIdx)
    {
        return _items.ContainsKey(posIdx);
    }

    public ItemDataInBag Get(int posIdx)
    {
        if (HasItem(posIdx) == false)
        {
            return null;
        }

        return _items[posIdx];
    }

    public void GetAllItems(ref List<ItemDataInBag> items)
    {
        items.Clear();
        foreach (var item in _items)
        {
            items.Add(item.Value);
        }
    }
}