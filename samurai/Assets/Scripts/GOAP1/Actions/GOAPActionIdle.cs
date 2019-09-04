using UnityEngine;
using System.Collections.Generic;
using Phenix.Unity.AI;

public class GOAPActionIdle : GOAPActionBase
{
    AnimFSMEventIdle _eventIdle;

    public GOAPActionIdle(Agent1 agent, FSMComponent fsm, 
        List<WorldStateBitData> WSPrecondition, List<WorldStateBitDataAction> WSEffect) 
        : base((int)GOAPActionType1.IDLE, agent, fsm, WSPrecondition, WSEffect)
    {

    }

    public override void Reset()
    {
        base.Reset();
        _eventIdle = null;
    }

    public override void OnEnter()
    {
        Reset();
        SendIdleEvent();
    }

    public override void OnExit(Phenix.Unity.AI.WorldState ws)
    {
        if (_eventIdle != null)
        {
            _eventIdle.Release();
            _eventIdle = null;
        }
        base.OnExit(ws);
    }    

    public override bool IsFinished()
    {
        return _eventIdle != null && _eventIdle.IsFinished;
    }

    void SendIdleEvent()
    {
        _eventIdle = AnimFSMEventIdle.pool.Get();
        FSMComponent.SendEvent(_eventIdle);
    }
}