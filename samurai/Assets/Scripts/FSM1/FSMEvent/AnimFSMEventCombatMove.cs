using UnityEngine;
using Phenix.Unity.Collection;

public class AnimFSMEventCombatMove : AnimFSMEvent
{
    public static Pool<AnimFSMEventCombatMove> pool = new Pool<AnimFSMEventCombatMove>(10, Reset);

    public Agent1 target;
    public float totalMoveDistance;
    public float minDistanceToTarget;

    public MoveType moveType;
    public MotionType motionType;
    public override void Release()
    {
        pool.Collect(this);
    }

    public AnimFSMEventCombatMove()
    {
        EventCode = (int)AnimFSMEventCode.COMBAT_MOVE;
        FSMStateTypeToTransfer = (int)AnimFSMStateType.COMBAT_MOVE;
        Reset();
    }

    public override void Reset()
    {
        base.Reset();
        totalMoveDistance = 0;
        minDistanceToTarget = 0;
        moveType = MoveType.NONE;
        motionType = MotionType.WALK;
        target = null;
    }
}