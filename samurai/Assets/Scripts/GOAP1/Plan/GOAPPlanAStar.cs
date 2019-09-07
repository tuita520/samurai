using System.Collections.Generic;
using UnityEngine;

public class GOAPPlanAStar : Phenix.Unity.AI.GOAPPlan
{
    AStarGOAP _astar = new AStarGOAP();
    
    public override void BuildPlan(Phenix.Unity.AI.GOAPGoal goal, 
        List<Phenix.Unity.AI.GOAPAction> actions)
    {
        if (goal == null || actions.Count == 0)
        {
            return;
        }

        _astar.goal = goal;
        _astar.actions = actions;

        GOAPAStarNode start = GOAPAStarNode.pool.Get();
        start.nodeWS = CurWorldState.Clone();
        start.goalType = goal.GOAPGoalType;
        start.adaptedAction = null;

        GOAPAStarNode finish = GOAPAStarNode.pool.Get();
        finish.nodeWS = CurWorldState.Clone();
        goal.ApplyEffect(finish.nodeWS);
        finish.goalType = goal.GOAPGoalType;
        finish.adaptedAction = null;

        List<GOAPAStarNode> nodePath;
        AStarGOAP.ResultCode retCode = _astar.FindPath(start, finish, out nodePath, 100);
        if (retCode == AStarGOAP.ResultCode.SUCCESS)
        {
            foreach (var node in nodePath)
            {
                if (node.adaptedAction == null) // 起点
                {
                    continue;
                }
                steps.Enqueue(node.adaptedAction);
            }
        }
        else
        {
            Debug.Log(retCode);
        }

        /*
        Dictionary<int, Phenix.Unity.AI.GOAPAction> actionsMap = new Dictionary<int, Phenix.Unity.AI.GOAPAction>();
        foreach (var item in actions)
        {
            actionsMap.Add(item.GOAPActionType, item);
        }        
        
        switch (goal.GOAPGoalType)
        {            
            case (int)GOAPGoalType1.IDLE:
                steps.Enqueue(actionsMap[(int)GOAPActionType1.IDLE]);
                break;
            case (int)GOAPGoalType1.ORDER_MOVE:
                steps.Enqueue(actionsMap[(int)GOAPActionType1.ORDER_MOVE]);
                break;
            case (int)GOAPGoalType1.ORDER_DODGE:
                steps.Enqueue(actionsMap[(int)GOAPActionType1.ORDER_ROLL]);
                break;
            case (int)GOAPGoalType1.ORDER_ATTACK:
                if (CurWorldState.Get((int)WorldStatePropType.WEAPON_IN_HAND) == false)
                {
                    if (CurWorldState.Get((int)WorldStatePropType.IN_IDLE) == false)
                    {
                        steps.Enqueue(actionsMap[(int)GOAPActionType1.IDLE]);
                    }
                    steps.Enqueue(actionsMap[(int)GOAPActionType1.SHOW_SWORD]);
                }
                steps.Enqueue(actionsMap[(int)GOAPActionType1.ORDER_ATTACK_MELEE]);
                break;
            case (int)GOAPGoalType1.CALM:
                if (CurWorldState.Get((int)WorldStatePropType.IN_IDLE) == false)
                {
                    steps.Enqueue(actionsMap[(int)GOAPActionType1.IDLE]);
                }
                steps.Enqueue(actionsMap[(int)GOAPActionType1.HIDE_SWORD]);                
                break;
            case (int)GOAPGoalType1.RETREAT:
                if (CurWorldState.Get((int)WorldStatePropType.WEAPON_IN_HAND) == false)
                {
                    if (CurWorldState.Get((int)WorldStatePropType.IN_IDLE) == false)
                    {
                        steps.Enqueue(actionsMap[(int)GOAPActionType1.IDLE]);
                    }
                    steps.Enqueue(actionsMap[(int)GOAPActionType1.SHOW_SWORD]);
                }
                steps.Enqueue(actionsMap[(int)GOAPActionType1.COMBAT_MOVE_BACKWARD]);
                break;
            case (int)GOAPGoalType1.PRESS:
                if (CurWorldState.Get((int)WorldStatePropType.WEAPON_IN_HAND) == false)
                {
                    if (CurWorldState.Get((int)WorldStatePropType.IN_IDLE) == false)
                    {
                        steps.Enqueue(actionsMap[(int)GOAPActionType1.IDLE]);
                    }
                    steps.Enqueue(actionsMap[(int)GOAPActionType1.SHOW_SWORD]);
                }
                steps.Enqueue(actionsMap[(int)GOAPActionType1.COMBAT_MOVE_FORWARD]);
                break;
            case (int)GOAPGoalType1.AVOID_LEFT:
                if (CurWorldState.Get((int)WorldStatePropType.WEAPON_IN_HAND) == false)
                {
                    if (CurWorldState.Get((int)WorldStatePropType.IN_IDLE) == false)
                    {
                        steps.Enqueue(actionsMap[(int)GOAPActionType1.IDLE]);
                    }
                    steps.Enqueue(actionsMap[(int)GOAPActionType1.SHOW_SWORD]);
                }
                steps.Enqueue(actionsMap[(int)GOAPActionType1.COMBAT_MOVE_LEFT]);
                break;
            case (int)GOAPGoalType1.AVOID_RIGHT:
                if (CurWorldState.Get((int)WorldStatePropType.WEAPON_IN_HAND) == false)
                {
                    if (CurWorldState.Get((int)WorldStatePropType.IN_IDLE) == false)
                    {
                        steps.Enqueue(actionsMap[(int)GOAPActionType1.IDLE]);
                    }
                    steps.Enqueue(actionsMap[(int)GOAPActionType1.SHOW_SWORD]);
                }
                steps.Enqueue(actionsMap[(int)GOAPActionType1.COMBAT_MOVE_RIGHT]);
                break;
            case (int)GOAPGoalType1.ALERT:
                if (CurWorldState.Get((int)WorldStatePropType.WEAPON_IN_HAND) == false)
                {
                    if (CurWorldState.Get((int)WorldStatePropType.IN_IDLE) == false)
                    {
                        steps.Enqueue(actionsMap[(int)GOAPActionType1.IDLE]);
                    }
                    steps.Enqueue(actionsMap[(int)GOAPActionType1.SHOW_SWORD]);
                }
                if (CurWorldState.Get((int)WorldStatePropType.LOOKING_AT_TARGET) == false)
                {
                    steps.Enqueue(actionsMap[(int)GOAPActionType1.LOOK_AT_TARGET]);
                }                     
                break;
            case (int)GOAPGoalType1.SHOW:
                if (CurWorldState.Get((int)WorldStatePropType.WEAPON_IN_HAND) == false)
                {
                    if (CurWorldState.Get((int)WorldStatePropType.IN_IDLE) == false)
                    {
                        steps.Enqueue(actionsMap[(int)GOAPActionType1.IDLE]);
                    }
                    steps.Enqueue(actionsMap[(int)GOAPActionType1.SHOW_SWORD]);
                }
                steps.Enqueue(actionsMap[(int)GOAPActionType1.PLAY_ANIM]);
                break;
            case (int)GOAPGoalType1.ATTACK_TARGET:
                if (CurWorldState.Get((int)WorldStatePropType.WEAPON_IN_HAND) == false)
                {
                    if (CurWorldState.Get((int)WorldStatePropType.IN_IDLE) == false)
                    {
                        steps.Enqueue(actionsMap[(int)GOAPActionType1.IDLE]);
                    }
                    steps.Enqueue(actionsMap[(int)GOAPActionType1.SHOW_SWORD]);
                }
                if (CurWorldState.Get((int)WorldStatePropType.LOOKING_AT_TARGET) == false)
                {
                    steps.Enqueue(actionsMap[(int)GOAPActionType1.LOOK_AT_TARGET]);
                }
                if (CurWorldState.Get((int)WorldStatePropType.IN_WEAPON_RANGE) == false)
                {
                    //steps.Enqueue(actionsMap[(int)GOAPActionType1.GOTO_MELEE_RANGE]);
                    steps.Enqueue(actionsMap[(int)GOAPActionType1.GOTO_TARGET]);
                }
                //steps.Enqueue(actionsMap[(int)GOAPActionType1.ATTACK_COUNTER]);
                //steps.Enqueue(actionsMap[(int)GOAPActionType1.ATTACK_WHIRL]);
                //steps.Enqueue(actionsMap[(int)GOAPActionType1.ATTACK_MELEE_MULTI_SWORDS]);                
                //steps.Enqueue(actionsMap[(int)GOAPActionType1.ATTACK_MELEE_SINGLE_SWORD]);
                steps.Enqueue(actionsMap[(int)GOAPActionType1.ATTACK_CROSS]);
                break;
            case (int)GOAPGoalType1.REACT_TO_DAMAGE:
                if (CurWorldState.Get((int)WorldStatePropType.WEAPON_IN_HAND) == false)
                {
                    if (CurWorldState.Get((int)WorldStatePropType.IN_IDLE) == false)
                    {
                        steps.Enqueue(actionsMap[(int)GOAPActionType1.IDLE]);
                    }
                    steps.Enqueue(actionsMap[(int)GOAPActionType1.SHOW_SWORD]);
                }
                steps.Enqueue(actionsMap[(int)GOAPActionType1.REACT_TO_DAMAGE]);
                break;
            case (int)GOAPGoalType1.BLOCK:
                if (CurWorldState.Get((int)WorldStatePropType.WEAPON_IN_HAND) == false)
                {
                    if (CurWorldState.Get((int)WorldStatePropType.IN_IDLE) == false)
                    {
                        steps.Enqueue(actionsMap[(int)GOAPActionType1.IDLE]);
                    }
                    steps.Enqueue(actionsMap[(int)GOAPActionType1.SHOW_SWORD]);
                }
                if (CurWorldState.Get((int)WorldStatePropType.LOOKING_AT_TARGET) == false)
                {
                    steps.Enqueue(actionsMap[(int)GOAPActionType1.LOOK_AT_TARGET]);
                }
                steps.Enqueue(actionsMap[(int)GOAPActionType1.BLOCK]);
                break;
            default:
                break;
        }        */
    }
}