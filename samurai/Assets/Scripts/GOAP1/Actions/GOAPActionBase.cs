using System.Collections.Generic;
using Phenix.Unity.AI;

public abstract class GOAPActionBase : Phenix.Unity.AI.GOAPAction
{
    protected Agent1 Agent { get; private set; }
    protected FSMComponent FSMComponent { get; private set; }

    protected GOAPActionBase(int actionType, Agent1 agent, FSMComponent fsm, 
        List<WorldStateBitData> WSPrecondition, List<WorldStateBitDataAction> WSEffect) 
        : base(actionType, WSPrecondition, WSEffect)
    {
        Agent = agent;
        FSMComponent = fsm;
    }

    public override void Reset() { }
}