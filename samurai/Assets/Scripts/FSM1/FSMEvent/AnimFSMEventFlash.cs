using UnityEngine;
using Phenix.Unity.Collection;

public class AnimFSMEventFlash : AnimFSMEvent
{
    public static Pool<AnimFSMEventFlash> pool = new Pool<AnimFSMEventFlash>(10, Reset);

    public Agent1 target;
    public Vector3 finalPosition;
    public MoveType moveType;
    public MotionType motionType;
        
    public AnimFSMEventFlash()
    {
        EventCode = (int)AnimFSMEventCode.FLASH;
        FSMStateTypeToTransfer = (int)AnimFSMStateType.FLASH;
        Reset();
    }

    public override void Reset()
    {
        base.Reset();
        finalPosition = Vector3.zero;
        moveType = MoveType.NONE;
        motionType = MotionType.NONE;
        target = null;
    }

    public override void Release()
    {
        pool.Collect(this);
    }
}