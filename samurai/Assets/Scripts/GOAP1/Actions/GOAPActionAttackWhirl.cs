using System.Collections.Generic;
using Phenix.Unity.AI;

public class GOAPActionAttackWhirl1 : GOAPActionBase
{
    private AnimFSMEventAttackWhirl _eventAttackWhirl;

    public GOAPActionAttackWhirl1(Agent1 agent, FSMComponent fsm, 
        List<WorldStateBitData> WSPrecondition, List<WorldStateBitDataAction> WSEffect)
        : base((int)GOAPActionType1.ATTACK_WHIRL, agent, fsm, WSPrecondition, WSEffect)
    {

    }

    public override void Reset()
    {
        base.Reset();
        _eventAttackWhirl = null;        
    }

    public override void OnEnter()
    {   
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
        FSMComponent.SendEvent(_eventAttackWhirl);
    }

    public override bool IsFinished()
    {
        return _eventAttackWhirl != null && _eventAttackWhirl.IsFinished;
    }
}