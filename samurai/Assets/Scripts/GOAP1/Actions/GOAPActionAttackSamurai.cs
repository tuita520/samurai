using System.Collections.Generic;
using UnityEngine;
using Phenix.Unity.AI;

public class GOAPActionAttackSamurai : GOAPActionAttackMeleeMultiSwords
{

    public GOAPActionAttackSamurai(GOAPActionType1 actionType, Agent1 agent,
        List<WorldStateBitData> WSPrecondition, List<WorldStateBitDataAction> WSEffect) 
        : base(actionType, agent, WSPrecondition, WSEffect)
    {        

    }
        
    protected override ComboType GetComboType()
    {
        ComboType[] combos = new ComboType[] { ComboType.RAISE_WAVE, ComboType.HALF_MOON, ComboType.CLOUD_CUT,
            ComboType.WALKING_DEATH, ComboType.CRASH_GENERAL, ComboType.FLYING_DRAGON};

        return combos[Random.Range(0, combos.Length)];
    }
}