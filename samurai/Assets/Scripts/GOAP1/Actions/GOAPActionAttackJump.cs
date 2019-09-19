using System.Collections.Generic;
using UnityEngine;
using Phenix.Unity.AI;

public class GOAPActionAttackJump : GOAPActionAttackMeleeMultiSwords
{

    public GOAPActionAttackJump(GOAPActionType1 actionType, Agent1 agent,
        List<WorldStateBitData> WSPrecondition, List<WorldStateBitDataAction> WSEffect) 
        : base(actionType, agent, WSPrecondition, WSEffect)
    {        

    }
        
    protected override ComboType GetComboType()
    {
        return ComboType.JUMP_KILL;
    }
}