using Phenix.Unity.Pattern;
using UnityEngine;
using System.Collections.Generic;

// 模板数据类型
public enum AssetDataType
{
    NONE = -1,
    TIP,            // 提示（loading）
    POETRY,         // 诗歌（loading）
    FACE,           // 脸谱（人物志）
    STAGE,          // 关卡
    ITEM,           // 物品
    UI,             // UI预设体
}

[System.Serializable]
public class AssetData
{
    public AssetDataType assetDataType;
    public ScriptableObject assetObj;
}

/// <summary>
/// 模板数据管理
/// </summary>
public class AssetDataMgr : Singleton<AssetDataMgr>
{
    // 模板表格数据
    public List<AssetData> assets = new List<AssetData>();
    Dictionary<AssetDataType, ScriptableObject> _assets = new Dictionary<AssetDataType, ScriptableObject>();

    protected override void Awake()
    {
        base.Awake();
        foreach (var item in assets)
        {
            if (item == null)
            {
                continue;
            }
            _assets.Add(item.assetDataType, item.assetObj);
        }
    }

    public ScriptableObject Get(AssetDataType assetDataType)
    {
        return _assets[assetDataType];
    }
}