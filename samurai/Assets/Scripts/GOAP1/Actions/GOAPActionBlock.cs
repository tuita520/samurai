using System.Collections.Generic;
using Phenix.Unity.AI;

public class GOAPActionBlock1 : GOAPActionBase
{
    AnimFSMEventBlock _eventBlock;

    public GOAPActionBlock1(Agent1 agent, FSMComponent fsm, 
        List<WorldStateBitData> WSPrecondition, List<WorldStateBitDataAction> WSEffect) 
        : base((int)GOAPActionType1.BLOCK, agent, fsm, WSPrecondition, WSEffect)
    {

    }

    public override void Reset()
    {
        base.Reset();
        _eventBlock = null;
    }

    public override void OnEnter()
    {
        SendEvent();
    }

    public override void OnExit(Phenix.Unity.AI.WorldState ws)
    {
        if (_eventBlock != null)
        {
            _eventBlock.Release();
            _eventBlock = null;
        }
        base.OnExit(ws);
    }

    public override bool IsFinished()
    {
        return _eventBlock != null && _eventBlock.IsFinished;
    }

    void SendEvent()
    {
        _eventBlock = AnimFSMEventBlock.pool.Get();
        _eventBlock.attacker = Agent.BlackBoard.desiredTarget;
        _eventBlock.holdTime = Agent.BlackBoard.blockHoldTime;
        FSMComponent.SendEvent(_eventBlock);
    }
}