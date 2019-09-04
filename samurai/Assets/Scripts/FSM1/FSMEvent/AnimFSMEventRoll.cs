using UnityEngine;
using Phenix.Unity.Collection;

public class AnimFSMEventRoll : AnimFSMEvent
{
    public static Pool<AnimFSMEventRoll> pool = new Pool<AnimFSMEventRoll>(10, Reset);

    public Vector3 direction;
    public Agent1 toTarget;

    public AnimFSMEventRoll()
    {
        EventCode = (int)AnimFSMEventCode.ROLL;
        FSMStateTypeToTransfer = (int)AnimFSMStateType.ROLL;
        Reset();
    }

    public override void Reset()
    {
        base.Reset();
        direction = Vector3.zero;
        toTarget = null;
    }

    public override void Release()
    {
        pool.Collect(this);
    }
}