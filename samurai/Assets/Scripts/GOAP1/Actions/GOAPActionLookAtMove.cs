using System.Collections.Generic;
using Phenix.Unity.AI;

public class GOAPActionLookAtMove : GOAPActionBase
{
    AnimFSMEventMoveRotate _eventRotate;

    public GOAPActionLookAtMove(GOAPActionType1 actionType, Agent1 agent,
        List<WorldStateBitData> WSPrecondition, List<WorldStateBitDataAction> WSEffect) 
        : base((int)actionType, agent, WSPrecondition, WSEffect)
    {

    }

    public override void Reset()
    {
        base.Reset();
        _eventRotate = null;        
    }

    public override void OnEnter()
    {
        SendEvent();
    }

    public override void OnExit(Phenix.Unity.AI.WorldState ws)
    {
        if (_eventRotate != null)
        {
            _eventRotate.Release();
            _eventRotate = null;
        }
        base.OnExit(ws);
    }

    public override bool IsFinished()
    {
        return _eventRotate.IsFinished;
    }

    void SendEvent()
    {
        _eventRotate = AnimFSMEventMoveRotate.pool.Get();
        _eventRotate.target = Agent.BlackBoard.desiredTarget;
        Agent.FSMComponent.SendEvent(_eventRotate);
    }
}