using UnityEngine;
using Phenix.Unity.Collection;

public class AnimFSMEventMove : AnimFSMEvent
{
    public static Pool<AnimFSMEventMove> pool = new Pool<AnimFSMEventMove>(10, Reset);

    public Vector3 moveDir;

        
    public AnimFSMEventMove()
    {
        EventCode = (int)AnimFSMEventCode.MOVE;
        FSMStateTypeToTransfer = (int)AnimFSMStateType.MOVE;
        Reset();
    }

    public override void Reset()
    {
        base.Reset();
        moveDir = Vector3.zero;          
    }

    public override void Release()
    {
        pool.Collect(this);
    }
}