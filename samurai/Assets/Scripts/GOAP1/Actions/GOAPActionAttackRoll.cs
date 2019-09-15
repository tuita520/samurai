using System.Collections.Generic;
using Phenix.Unity.AI;

public class GOAPActionAttackRoll : GOAPActionBase
{
    AnimFSMEventAttackRoll _eventAttackRoll;

    public GOAPActionAttackRoll(Agent1 agent, FSMComponent fsm,
        List<WorldStateBitData> WSPrecondition, List<WorldStateBitDataAction> WSEffect)
        : base((int)GOAPActionType1.ATTACK_ROLL, agent, fsm, WSPrecondition, WSEffect)
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
        _eventAttackRoll.target = Agent.BlackBoard.desiredTarget;
        _eventAttackRoll.animAttackData = Agent.AnimSet.ProcessCombo(ComboType.ATTACK_ROLL);
        FSMComponent.SendEvent(_eventAttackRoll);
    }
}