using System;
using System.Collections.Generic;

/// <summary>
/// 玩家技能数据
/// </summary>
[Serializable]
public class PlayerSkillData
{    
    public ComboType comboType;
    public int level = 0;               // 连招当前等级
}

public class PlayerSkillMgr
{
    Dictionary<ComboType, PlayerSkillData> _items = new Dictionary<ComboType, PlayerSkillData>();

}