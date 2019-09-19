using UnityEngine;
using System.Collections.Generic;
using Phenix.Unity.AI;

public class GOAPActionCombatMoveRight : GOAPActionBase
{
    AnimFSMEventCombatMove _eventCombatMove;
    Vector3 _finalPos;

    public GOAPActionCombatMoveRight(GOAPActionType1 actionType, Agent1 agent, 
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
        return _eventCombatMove.IsFinished;
    }

    void SendEvent()
    {
        _eventCombatMove = AnimFSMEventCombatMove.pool.Get();
        _eventCombatMove.moveType = MoveType.RIGHTWARD;
        _eventCombatMove.motionType = MotionType.WALK;
        _eventCombatMove.target = Agent.BlackBoard.desiredTarget;
        _eventCombatMove.totalMoveDistance = Random.Range(2f, 4f);
        _eventCombatMove.minDistanceToTarget = Agent.BlackBoard.DistanceToDesiredTarget * 0.7f;
        Agent.FSMComponent.SendEvent(_eventCombatMove);
    }
}