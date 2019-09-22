using System.Collections.Generic;
using Phenix.Unity.AI;

public class GOAPActionAttackBow : GOAPActionBase
{
    AnimFSMEventAttackBow _eventAttack;

    public GOAPActionAttackBow(GOAPActionType1 actionType, Agent1 agent,
        List<WorldStateBitData> WSPrecondition, List<WorldStateBitDataAction> WSEffect)
        : base((int)actionType, agent, WSPrecondition, WSEffect)
    {

    }

    public override void Reset()
    {
        base.Reset();
        _eventAttack = null;
    }

    public override void OnEnter()
    {
        Agent.AnimSet.ResetComboProgress();
        SendEvent();
    }

    public override void OnExit(Phenix.Unity.AI.WorldState ws)
    {
        if (_eventAttack != null)
        {
            _eventAttack.Release();
            _eventAttack = null;
        }
        base.OnExit(ws);
    }

    void SendEvent()
    {        
        _eventAttack = AnimFSMEventAttackBow.pool.Get();
        _eventAttack.target = Agent.BlackBoard.HasAttackTarget ? Agent.BlackBoard.desiredTarget : null;     
        _eventAttack.attackDir = Agent.BlackBoard.desiredDirection;        
        _eventAttack.attackPhaseDone = false;
        _eventAttack.animAttackData = Agent.AnimSet.ProcessCombo(ComboType.BOW);
        Agent.FSMComponent.SendEvent(_eventAttack);
    }

    public override bool IsFinished()
    {
        return _eventAttack != null && _eventAttack.IsFinished;
    }
}