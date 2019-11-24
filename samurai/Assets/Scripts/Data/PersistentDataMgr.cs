using System.Collections.Generic;
using Phenix.Unity.Pattern;
using UnityEngine;
using System.Collections;

/// <summary>
/// 玩家持久化数据
/// </summary>
[System.Serializable]
public class PlayerPersistentData
{
    public int exp = 0;         // 经验
    public int money = 0;       // 金钱
    public int stageIdx = 0;    // 到达关卡序号
    public BitArray presentCharacters = new BitArray(15);
    public List<ItemDataInBag> items = new List<ItemDataInBag>();           // 物品
    public List<PlayerSkillData> skills = new List<PlayerSkillData>();      // 技能
}

public class PersistentDataMgr : Singleton<PersistentDataMgr>
{
    [SerializeField]
    float checkInterval = 10;    // 存盘检测时间间隔（秒）

    float _nextCheckTimer = 0;

    public bool Dirty { get; set; }

    private void Start()
    {
        Load();
        Dirty = false;
        _nextCheckTimer = Time.timeSinceLevelLoad + checkInterval;
    }

    private void Update()
    {
        if (Time.timeSinceLevelLoad < _nextCheckTimer)
        {
            return;
        }

        if (Dirty == false)
        {
            return;
        }

        Save();
    }

    public void Load()
    {
        PlayerPersistentData data = new PlayerPersistentData();
        // todo 从字符串通过json反序列化得到data，给itemmgr和skillmgr赋值
    }

    public void Save()
    {
        PlayerPersistentData data = new PlayerPersistentData();
        // todo itemmgr和skillmgr等等给data赋值，json序列化成字符串，unity持久化字符串
        Dirty = false;
    }
}
