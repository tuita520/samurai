using System.Collections.Generic;
using Phenix.Unity.AI;

public class GOAPActionAttackCross : GOAPActionBase
{
    AnimFSMEventAttackCross _eventCross;

    public GOAPActionAttackCross(Agent1 agent, FSMComponent fsm, 
        List<WorldStateBitData> WSPrecondition, List<WorldStateBitDataAction> WSEffect) 
        : base((int)GOAPActionType1.ATTACK_CROSS, agent, fsm, WSPrecondition, WSEffect)
    {        

    }

    public override void Reset()
    {
        base.Reset();
        _eventCross = null;
    }

    public override void OnEnter()
    {
        Agent.AnimSet.ResetComboProgress();
        SendEvent();
    }

    public override void OnExit(Phenix.Unity.AI.WorldState ws)
    {
        if (_eventCross != null)
        {
            _eventCross.Release();
            _eventCross = null;
        }
        base.OnExit(ws);
    }

    void SendEvent()
    {
        _eventCross = AnimFSMEventAttackCross.pool.Get();        

        if (Agent.BlackBoard.desiredTarget)
        {
            Agent.BlackBoard.desiredDirection = Agent.BlackBoard.desiredTarget.Position - Agent.Position;
            Agent.BlackBoard.desiredDirection.Normalize();
            _eventCross.attackDir = Agent.BlackBoard.desiredDirection;
        }
        else
        {
            _eventCross.attackDir = Agent.Forward;
        }

        _eventCross.animAttackData = Agent.AnimSet.ProcessCombo(ComboType.CROSS);        
        FSMComponent.SendEvent(_eventCross);        
    }

    public override bool IsFinished()
    {
        return (_eventCross != null && _eventCross.IsFinished);        
    }
}