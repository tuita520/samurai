using UnityEngine;
using Phenix.Unity.Collection;

public class AnimFSMEventMoveRotate : AnimFSMEvent
{
    public static Pool<AnimFSMEventMoveRotate> pool = new Pool<AnimFSMEventMoveRotate>(10, Reset);

    public Vector3 direction;
    public Agent1 target;    

    public AnimFSMEventMoveRotate()
    {
        EventCode = (int)AnimFSMEventCode.MOVE_ROTATE;
        FSMStateTypeToTransfer = (int)AnimFSMStateType.MOVE_ROTATE;
        Reset();
    }

    public override void Reset()
    {
        base.Reset();
        target = null;
        direction = Vector3.zero;        
    }
    public override void Release()
    {
        pool.Collect(this);
    }
}