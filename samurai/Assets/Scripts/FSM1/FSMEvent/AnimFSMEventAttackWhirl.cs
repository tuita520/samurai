using UnityEngine.Events;
using Phenix.Unity.Collection;

public class AnimFSMEventAttackWhirl : AnimFSMEvent
{
    public static Pool<AnimFSMEventAttackWhirl> pool = new Pool<AnimFSMEventAttackWhirl>(10, Reset);

    public Agent1 target;
    public AnimAttackData data;
    
    public AnimFSMEventAttackWhirl()
    {
        EventCode = (int)AnimFSMEventCode.ATTACK_WHIRL;
        FSMStateTypeToTransfer = (int)AnimFSMStateType.ATTACK_WHIRL;
        Reset();
    }

    public override void Reset()
    {
        base.Reset();
        data = null;
        target = null;
    }

    public override void Release()
    {
        pool.Collect(this);
    }
}