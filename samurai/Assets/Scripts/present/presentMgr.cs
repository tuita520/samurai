using System.Collections;
using Phenix.Unity.Pattern;

/// <summary>
/// 出场人物管理
/// </summary>
public class presentMgr : Singleton<presentMgr>
{
    BitArray _present = new BitArray(15);

    public bool IsPresent(AgentType agentType)
    {
        return false;
    }

    public void SetPresent(AgentType agentType)
    {
        _present.Set((int)agentType, true);
    }
}
