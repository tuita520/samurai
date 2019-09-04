using System.Collections.Generic;
using UnityEngine;
using Phenix.Unity.AI;

public class GOAPActionOrderRoll : GOAPActionBase
{
    AnimFSMEventRoll _eventRoll;

    public GOAPActionOrderRoll(Agent1 agent, FSMComponent fsm,
        List<WorldStateBitData> WSPrecondition, List<WorldStateBitDataAction> WSEffect)
        : base((int)GOAPActionType1.ORDER_ROLL, agent, fsm, WSPrecondition, WSEffect)
    {

    }


    public override void Reset()
    {
        base.Reset();
        _eventRoll = null;
    }

    public override void OnEnter()
    {        
        SendEvent();
    }

    public override void OnExit(Phenix.Unity.AI.WorldState ws)
    {
        if (_eventRoll != null)
        {
            _eventRoll.Release();
            _eventRoll = null;
        }
        base.OnExit(ws);
    }

    public override bool IsFinished()
    {
        return _eventRoll != null && _eventRoll.IsFinished;
    }

    void SendEvent()
    {
        _eventRoll = AnimFSMEventRoll.pool.Get();
        _eventRoll.direction = Agent.BlackBoard.desiredDirection;
        FSMComponent.SendEvent(_eventRoll);
    }
}