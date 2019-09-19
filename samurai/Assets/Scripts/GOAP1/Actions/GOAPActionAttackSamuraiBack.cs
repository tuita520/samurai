using System.Collections.Generic;
using Phenix.Unity.AI;

public class GOAPActionAttackSamuraiBack : GOAPActionAttackSamurai
{

    public GOAPActionAttackSamuraiBack(GOAPActionType1 actionType, Agent1 agent,
        List<WorldStateBitData> WSPrecondition, List<WorldStateBitDataAction> WSEffect) 
        : base(actionType, agent, WSPrecondition, WSEffect)
    {        

    }
}