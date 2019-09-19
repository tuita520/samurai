using System.Collections.Generic;
using Phenix.Unity.AI;

public abstract class GOAPActionBase : Phenix.Unity.AI.GOAPAction
{
    protected Agent1 Agent { get; private set; }    

    protected GOAPActionBase(int actionType, Agent1 agent, 
        List<WorldStateBitData> WSPrecondition, List<WorldStateBitDataAction> WSEffect) 
        : base(actionType, WSPrecondition, WSEffect)
    {
        Agent = agent;        
    }

    public override void Reset() { }
}