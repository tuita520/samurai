using System.Collections.Generic;
using UnityEngine;
using Phenix.Unity.AI;

public class GOAPActionRollForDodge : GOAPActionOrderRoll
{
    public GOAPActionRollForDodge(GOAPActionType1 actionType, Agent1 agent,
        List<WorldStateBitData> WSPrecondition, List<WorldStateBitDataAction> WSEffect)
        : base(actionType, agent, WSPrecondition, WSEffect)
    {

    }

}