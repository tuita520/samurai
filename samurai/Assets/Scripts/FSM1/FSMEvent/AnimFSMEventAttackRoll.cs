using UnityEngine;
using Phenix.Unity.Collection;

public class AnimFSMEventAttackRoll : AnimFSMEvent
{
    public static Pool<AnimFSMEventAttackRoll> pool = new Pool<AnimFSMEventAttackRoll>(10, Reset);

    public AnimAttackData animAttackData;
    public Agent1 target;

    public AnimFSMEventAttackRoll()
    {
        EventCode = (int)AnimFSMEventCode.ATTACK_ROLL;
        FSMStateTypeToTransfer = (int)AnimFSMStateType.ATTACK_ROLL;
        Reset();
    }

    public override void Reset()
    {
        base.Reset();
        animAttackData = null;
        target = null;
    }

    public override void Release()
    {
        pool.Collect(this);
    }
}