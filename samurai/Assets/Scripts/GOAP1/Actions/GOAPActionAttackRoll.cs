using System.Collections.Generic;
using Phenix.Unity.AI;

public class GOAPActionAttackRoll : GOAPActionBase
{
    AnimFSMEventAttackRoll _eventAttackRoll;

    public GOAPActionAttackRoll(GOAPActionType1 actionType, Agent1 agent,
        List<WorldStateBitData> WSPrecondition, List<WorldStateBitDataAction> WSEffect)
        : base((int)actionType, agent, WSPrecondition, WSEffect)
    {
        
    }


    public override void Reset()
    {
        base.Reset();
        _eventAttackRoll = null;
    }

    public override void OnEnter()
    {        
        SendEvent();
    }
    
    public override void OnExit(Phenix.Unity.AI.WorldState ws)
    {
        if (_eventAttackRoll != null)
        {
            _eventAttackRoll.Release();
            _eventAttackRoll = null;
        }
        base.OnExit(ws);
    }

    public override bool IsFinished()
    {
        return _eventAttackRoll != null && _eventAttackRoll.IsFinished;
    }

    void SendEvent()
    {
        _eventAttackRoll = AnimFSMEventAttackRoll.pool.Get();
        _eventAttackRoll.target = Agent.BlackBoard.HasAttackTarget ? Agent.BlackBoard.desiredTarget : null;
        _eventAttackRoll.animAttackData = Agent.AnimSet.ProcessCombo(ComboType.ATTACK_ROLL);
        Agent.FSMComponent.SendEvent(_eventAttackRoll);
    }
}