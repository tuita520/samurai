using System.Collections.Generic;
using Phenix.Unity.AI;

public class GOAPActionShowSword : GOAPActionBase
{
    AnimFSMEventShowHideSword _eventSword;

    public GOAPActionShowSword(Agent1 agent, FSMComponent fsm,
        List<WorldStateBitData> WSPrecondition, List<WorldStateBitDataAction> WSEffect)
        : base((int)GOAPActionType1.SHOW_SWORD, agent, fsm, WSPrecondition, WSEffect)
    {
        
    }


    public override void Reset()
    {
        base.Reset();
        _eventSword = null;
    }

    public override void OnEnter()
    {        
        SendEvent();
    }
    
    public override void OnExit(Phenix.Unity.AI.WorldState ws)
    {
        if (_eventSword != null)
        {
            _eventSword.Release();
            _eventSword = null;
        }
        base.OnExit(ws);
    }

    public override bool IsFinished()
    {
        return _eventSword.IsFinished;
    }

    void SendEvent()
    {
        _eventSword = AnimFSMEventShowHideSword.pool.Get();
        _eventSword.show = true;
        FSMComponent.SendEvent(_eventSword);
    }
}