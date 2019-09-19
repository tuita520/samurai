using UnityEngine;
using System.Collections.Generic;
using Phenix.Unity.AI;

public class GOAPActionCombatMoveBackward : GOAPActionBase
{
    AnimFSMEventCombatMove _eventCombatMove;
    Vector3 _finalPos;

    public GOAPActionCombatMoveBackward(GOAPActionType1 actionType, Agent1 agent,
        List<WorldStateBitData> WSPrecondition, List<WorldStateBitDataAction> WSEffect)
        : base((int)actionType, agent, WSPrecondition, WSEffect)
    {
        
    }

    public override void Reset()
    {
        base.Reset();
        _eventCombatMove = null;
        _finalPos = Vector3.zero;
    }

    public override void OnEnter()
    {
        SendEvent();
    }

    public override void OnExit(Phenix.Unity.AI.WorldState ws)
    {
        if (_eventCombatMove != null)
        {
            _eventCombatMove.Release();
            _eventCombatMove = null;
        }
        base.OnExit(ws);
    }

    public override bool IsFinished()
    {
        if (_eventCombatMove == null)
        {
            return true;
        }
        return _eventCombatMove.IsFinished || Agent.BlackBoard.DesiredTargetInCombatRange == false;
    }

    void SendEvent()
    {
        _eventCombatMove = AnimFSMEventCombatMove.pool.Get();
        _eventCombatMove.moveType = MoveType.BACKWARD;
        _eventCombatMove.motionType = MotionType.WALK;
        _eventCombatMove.target = Agent.BlackBoard.desiredTarget;
        _eventCombatMove.totalMoveDistance = Random.Range(2f, 5f);
        _eventCombatMove.minDistanceToTarget = 0;
        Agent.FSMComponent.SendEvent(_eventCombatMove);
    }
}