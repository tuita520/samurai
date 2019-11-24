using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ItemAssetData
{
    public ItemType itemType;       // 类型
    public Sprite look;             // 模样
    public string name;             // 名称
    public string brief;            // 简介
    public float val;               // 数值
    public int price;               // 价格（买入/卖出）
}

[CreateAssetMenu(fileName = "AssetItem", menuName = "Samurai/AssetItem")]
public class AssetItem : ScriptableObject
{
    public List<ItemAssetData> items = new List<ItemAssetData>();

    Dictionary<ItemType, ItemAssetData> _items = new Dictionary<ItemType, ItemAssetData>();

    private void OnEnable()
    {
        _items.Clear();
        foreach (var item in items)
        {
            if (item == null)
            {
                return;
            }
            _items.Add(item.itemType, item);
        }
    }

    public ItemAssetData Get(ItemType itemType)
    {
        if (_items.ContainsKey(itemType) == false)
        {
            return null;
        }
        return _items[itemType];
    }
}