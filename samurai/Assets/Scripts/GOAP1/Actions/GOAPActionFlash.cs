using UnityEngine;
using System.Collections.Generic;
using Phenix.Unity.AI;

public class GOAPActionFlash : GOAPActionBase
{
    AnimFSMEventFlash _eventFlash;
    
    public GOAPActionFlash(GOAPActionType1 actionType, Agent1 agent,
        List<WorldStateBitData> WSPrecondition, List<WorldStateBitDataAction> WSEffect)
        : base((int)actionType, agent, WSPrecondition, WSEffect)
    {
        
    }

    public override void Reset()
    {
        base.Reset();
        _eventFlash = null;     
    }

    public override void OnEnter()
    {
        SendEvent();
    }

    public override void OnExit(Phenix.Unity.AI.WorldState ws)
    {
        if (_eventFlash != null)
        {
            _eventFlash.Release();
            _eventFlash = null;
        }
        base.OnExit(ws);
    }

    public override bool IsFinished()
    {
        if (_eventFlash == null)
        {
            return true;
        }
        return _eventFlash.IsFinished;
    }

    void SendEvent()
    {
        _eventFlash = AnimFSMEventFlash.pool.Get();
        _eventFlash.moveType = MoveType.FORWARD;
        _eventFlash.motionType = MotionType.SPRINT;
        _eventFlash.finalPosition = GetAttackStart(Agent.BlackBoard.desiredTarget);
        _eventFlash.target = Agent.BlackBoard.desiredTarget;
        Agent.FSMComponent.SendEvent(_eventFlash);
    }

    Vector3 GetAttackStart(Agent1 target)
    {        
        if (target == null)
        {
            return Vector3.zero;
        }
        Vector3 dir = Vector3.zero;
        switch (Random.Range(0, 3))
        {
            case 0:
                dir = -target.Forward;
                break;
            case 1:
                dir = -target.Right;
                break;
            case 2:
                dir = target.Right;
                break;
            default:
                break;
        }
        
        return target.Position + dir * Agent.BlackBoard.weaponRange;
    }
}