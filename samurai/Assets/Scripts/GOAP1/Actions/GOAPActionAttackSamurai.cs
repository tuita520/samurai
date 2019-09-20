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

    public override void BeforeBuildPlan(Phenix.Unity.AI.GOAPGoal goal)
    {
        if (goal.GOAPGoalType == (int)GOAPGoalType1.ATTACK_TARGET)
        {
            if (Agent.BlackBoard.HasAttackTarget && Agent.BlackBoard.desiredTarget.BlackBoard.damageOnlyFromBack)
            {
                // 当前攻击目标只能从背后攻击
                AddWSPrecondition((int)WorldStatePropType.BEHIND_TARGET, true);
            }
            else
            {
                // 按机率决定正面或是背面进攻
                if (Random.Range(0, 2) == 0)
                {
                    AddWSPrecondition((int)WorldStatePropType.BEHIND_TARGET, true);
                }
                else
                {
                    RemoveWSPrecondition((int)WorldStatePropType.BEHIND_TARGET, true);
                }
            }
        }
    }
}