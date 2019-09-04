using System.Collections.Generic;
using Phenix.Unity.AI;

public class GOAPActionLookAt : GOAPActionBase
{
    AnimFSMEventRotate _eventRotate;

    public GOAPActionLookAt(Agent1 agent, FSMComponent fsm, 
        List<WorldStateBitData> WSPrecondition, List<WorldStateBitDataAction> WSEffect) 
        : base((int)GOAPActionType1.LOOK_AT_TARGET, agent, fsm, WSPrecondition, WSEffect)
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
        _eventRotate = AnimFSMEventRotate.pool.Get();
        _eventRotate.target = Agent.BlackBoard.desiredTarget;
        _eventRotate.rotationModifier = 0.5f;
        FSMComponent.SendEvent(_eventRotate);
    }
}