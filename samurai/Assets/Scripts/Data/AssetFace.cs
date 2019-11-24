using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class FaceData
{
    public AgentType agentType;     // 类型
    public string name;             // 姓名
    public string homeland;         // 家乡
    public string nickname;         // 绰号
    public string brief;            // 简介

    public Sprite head;             // 头像    
    public GameObject displayPrefab;// 展示用的prefab

    public int stamina;             // 耐力
    public int speed;               // 身法
    public int sword;               // 剑术
    public int foxy;                // 狡黠
    public int defend;              // 防御
}

[CreateAssetMenu(fileName = "AssetFace", menuName = "Samurai/AssetFace")]
public class AssetFace : ScriptableObject
{
    public List<FaceData> faces = new List<FaceData>();

    Dictionary<AgentType, FaceData> _faces = new Dictionary<AgentType, FaceData>();

    private void OnEnable()
    {
        _faces.Clear();
        foreach (var item in faces)
        {
            if (item == null)
            {
                return;
            }
            _faces.Add(item.agentType, item);
        }
    }

    public FaceData Get(AgentType agentType)
    {
        if (_faces.ContainsKey(agentType) == false)
        {
            return null;
        }
        return _faces[agentType];
    }
}