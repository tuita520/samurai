using UnityEngine;
using Phenix.Unity.Collection;
/*
public class AnimFSMEventGoToTarget : AnimFSMEvent
{
    public static Pool<AnimFSMEventGoToTarget> pool = new Pool<AnimFSMEventGoToTarget>(10, Reset);

    public Agent1 target;
    public MoveType moveType;
    public MotionType motionType;
        
    public AnimFSMEventGoToTarget()
    {
        EventCode = (int)AnimFSMEventCode.GOTO_TARGET;
        FSMStateTypeToTransfer = (int)AnimFSMStateType.GOTO_TARGET;
        Reset();
    }

    public override void Reset()
    {
        base.Reset();
        target = null;
        moveType = MoveType.NONE;
        motionType = MotionType.NONE;        
    }

    public override void Release()
    {
        pool.Collect(this);
    }
}*/