using UnityEngine;
using System.Collections.Generic;
using Phenix.Unity.AI;

public class GOAPActionPlayAnim : GOAPActionBase
{
    AnimFSMEventPlayAnim _eventPlayAnim;

    public GOAPActionPlayAnim(Agent1 agent, FSMComponent fsm, 
        List<WorldStateBitData> WSPrecondition, List<WorldStateBitDataAction> WSEffect) 
        : base((int)GOAPActionType1.PLAY_ANIM, agent, fsm, WSPrecondition, WSEffect)
    {

    }

    public override void Reset()
    {
        base.Reset();
        _eventPlayAnim = null;
    }

    public override void OnEnter()
    {
        SendEvent();
    }

    public override void OnExit(Phenix.Unity.AI.WorldState ws)
    {
        if (_eventPlayAnim != null)
        {
            _eventPlayAnim.Release();
            _eventPlayAnim = null;
        }
        base.OnExit(ws);
    }

    public override bool IsFinished()
    {
        return _eventPlayAnim != null && _eventPlayAnim.IsFinished;
    }

    void SendEvent()
    {
        _eventPlayAnim = AnimFSMEventPlayAnim.pool.Get();
        _eventPlayAnim.animName = Agent.AnimSet.GetIdleActionAnim(Agent.BlackBoard.weaponSelected, 
            Agent.BlackBoard.weaponState);
        _eventPlayAnim.lookAtTarget = true;
        FSMComponent.SendEvent(_eventPlayAnim);
    }
}