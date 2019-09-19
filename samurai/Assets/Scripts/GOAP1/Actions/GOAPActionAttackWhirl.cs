using System.Collections.Generic;
using Phenix.Unity.AI;

public class GOAPActionAttackWhirl1 : GOAPActionBase
{
    private AnimFSMEventAttackWhirl _eventAttackWhirl;

    public GOAPActionAttackWhirl1(GOAPActionType1 actionType, Agent1 agent, 
        List<WorldStateBitData> WSPrecondition, List<WorldStateBitDataAction> WSEffect)
        : base((int)actionType, agent, WSPrecondition, WSEffect)
    {

    }

    public override void Reset()
    {
        base.Reset();
        _eventAttackWhirl = null;        
    }

    public override void OnEnter()
    {
        Agent.AnimSet.ResetComboProgress();
        SendEvent();
    }

    public override void OnExit(Phenix.Unity.AI.WorldState ws)
    {
        if (_eventAttackWhirl != null)
        {
            _eventAttackWhirl.Release();
            _eventAttackWhirl = null;
        }
        base.OnExit(ws);
    }

    void SendEvent()
    {
        _eventAttackWhirl = AnimFSMEventAttackWhirl.pool.Get();        
        _eventAttackWhirl.data = Agent.AnimSet.ProcessCombo(ComboType.WHIRL);
        _eventAttackWhirl.target = Agent.BlackBoard.HasAttackTarget ? Agent.BlackBoard.desiredTarget : null;
        Agent.FSMComponent.SendEvent(_eventAttackWhirl);
    }

    public override bool IsFinished()
    {
        return _eventAttackWhirl != null && _eventAttackWhirl.IsFinished;
    }
}