using System.Collections.Generic;
using UnityEngine;
using Phenix.Unity.AI;

public class GOAPActionOrderMove : GOAPActionBase
{
    AnimFSMEventMove _eventMove;    

    public GOAPActionOrderMove(GOAPActionType1 actionType, Agent1 agent,
        List<WorldStateBitData> WSPrecondition, List<WorldStateBitDataAction> WSEffect)
        : base((int)actionType, agent, WSPrecondition, WSEffect)
    {
        
    }


    public override void Reset()
    {
        base.Reset();
        _eventMove = null;        
    }

    public override void OnEnter()
    {
        OrderData curOrder = Agent.PlayerOrder.GetCurOrder();
        SendEvent((curOrder as OrderDataMove).dir.normalized);
    }

    public override void OnUpdate()
    {
        if (_eventMove.IsFinished)
        {
            return;
        }

        OrderData nextOrder = Agent.PlayerOrder.GetNextOrder();
        if (nextOrder != null && nextOrder.orderType == (int)OrderType.MOVE)
        {            
            PlayerOrderPool.moves.Collect(Agent.PlayerOrder.Pop() as OrderDataMove);
            if (_eventMove != null)
            {
                _eventMove.Release();
            }
            SendEvent((nextOrder as OrderDataMove).dir.normalized);
        }
        else
        {
            _eventMove.IsFinished = true;
        }
    }    

    public override void OnExit(Phenix.Unity.AI.WorldState ws)
    {
        if (_eventMove != null)
        {
            _eventMove.Release();
            _eventMove = null;
        }
        base.OnExit(ws);
    }

    public override bool IsFinished()
    {
        return _eventMove != null && _eventMove.IsFinished;
    }

    void SendEvent(Vector3 dir)
    {
        _eventMove = AnimFSMEventMove.pool.Get();
        _eventMove.moveDir = dir;
        Agent.FSMComponent.SendEvent(_eventMove);
    }
}