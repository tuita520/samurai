using UnityEngine;
using Phenix.Unity.Collection;

public class AnimFSMEventRotate : AnimFSMEvent
{
    public static Pool<AnimFSMEventRotate> pool = new Pool<AnimFSMEventRotate>(10, Reset);

    public Vector3 direction;
    public Agent1 target;
    public float rotationModifier;

    public AnimFSMEventRotate()
    {
        EventCode = (int)AnimFSMEventCode.ROTATE;
        FSMStateTypeToTransfer = (int)AnimFSMStateType.ROTATE;
        Reset();
    }

    public override void Reset()
    {
        base.Reset();
        target = null;
        direction = Vector3.zero;
        rotationModifier = 1;
    }
    public override void Release()
    {
        pool.Collect(this);
    }
}