using UnityEngine;
using Phenix.Unity.Collection;

public class AnimFSMEventGoTo : AnimFSMEvent
{
    public static Pool<AnimFSMEventGoTo> pool = new Pool<AnimFSMEventGoTo>(10, Reset);

    public Vector3 finalPosition;
    public MoveType moveType;
    public MotionType motionType;
        
    public AnimFSMEventGoTo()
    {
        EventCode = (int)AnimFSMEventCode.GOTO;
        FSMStateTypeToTransfer = (int)AnimFSMStateType.GOTO;
        Reset();
    }

    public override void Reset()
    {
        base.Reset();
        finalPosition = Vector3.zero;
        moveType = MoveType.NONE;
        motionType = MotionType.NONE;        
    }

    public override void Release()
    {
        pool.Collect(this);
    }
}