using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class UIData
{
    public UIType uiType;           // UI类型
    public GameObject prefab;
}

[CreateAssetMenu(fileName = "AssetUI", menuName = "Samurai/AssetUI")]
public class AssetUI : ScriptableObject
{
    public List<UIData> uiList = new List<UIData>();

    Dictionary<UIType, UIData> _uiList = new Dictionary<UIType, UIData>();

    private void OnEnable()
    {
        _uiList.Clear();
        foreach (var item in uiList)
        {
            if (item == null)
            {
                return;
            }
            _uiList.Add(item.uiType, item);
        }
    }

    public UIData Get(UIType uiType)
    {
        if (_uiList.ContainsKey(uiType) == false)
        {
            return null;
        }
        return _uiList[uiType];
    }
}