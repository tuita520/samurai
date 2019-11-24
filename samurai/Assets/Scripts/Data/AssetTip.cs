using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "AssetTip", menuName = "Samurai/AssetTip")]
public class AssetTip : ScriptableObject
{
    public List<string> tips = new List<string>();

    public string Get()
    {
        return tips[Random.Range(0, tips.Count)];
    }
}