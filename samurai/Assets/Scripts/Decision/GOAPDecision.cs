using System.Collections.Generic;
using UnityEngine;
using Phenix.Unity.AI;

public enum WorldStatePropType
{
    NONE = -1,
    ATTACK_TARGET           = 0,
    IN_COMBAT_RANGE         = 1,
    LOOKING_AT_TARGET       = 2,
    IN_WEAPON_RANGE         = 3,
    WEAPON_IN_HAND          = 4,
    ENOUGH_BERSERK          = 5,
    MOVE_TO_RIGHT           = 6, 
    MOVE_TO_LEFT            = 7, 
    REACT_TO_DAMAGE         = 8,
    WILL_PLAY_ANIM          = 9,
    ORDER_MOVE              = 10,
    ORDER_ATTACK            = 11,
    ORDER_ROLL              = 12,
    IN_IDLE                 = 13,
    FINISH_BLOCK            = 14,
    IN_BLOCK                = 15,
    TARGET_IN_KNOCK_DOWN    = 16,
    AHEAD_OF_TARGET         = 17,
    BEHIND_TARGET           = 18,
    /*AT_TARGET_POS,
    IN_DODGE,
    ALERTED,    
    AHEAD_OF_ENEMY,
    BEHIND_ENEMY,
    BOSS_IS_NEAR,    */
    MAX
}

public enum GOAPGoalType1
{
    NONE = -1,
    ALERT               = 0,
    AVOID_LEFT          = 1,
    AVOID_RIGHT         = 2,
    PRESS               = 3,
    RETREAT             = 4,
    ATTACK_TARGET       = 5,    
    SHOW                = 6,    
    REACT_TO_DAMAGE     = 7,
    IDLE                = 8,
    ORDER_MOVE          = 9,
    ORDER_ATTACK        = 10,
    ORDER_DODGE         = 11,        
    CALM                = 12,
    BLOCK               = 13,        
    /*GOTO,
    DODGE,  
    BOSS_ATTACK,    */
    COUNT,
}

public enum GOAPActionType1
{
    NONE = -1,
    GOTO_MELEE_RANGE               = 0,
    COMBAT_RUN_FORWARD             = 1,
    COMBAT_RUN_BACKWARD            = 2,
    COMBAT_MOVE_FORWARD            = 3,
    COMBAT_MOVE_BACKWARD           = 4,
    COMBAT_MOVE_LEFT               = 5,
    COMBAT_MOVE_RIGHT              = 6, 
    LOOK_AT_TARGET                 = 7, 
    SHOW_SWORD                     = 8,
    ATTACK_MELEE_MULTI_SWORDS      = 9,
    ATTACK_WHIRL                   = 10,
    REACT_TO_DAMAGE                = 11,
    PLAY_ANIM                      = 12,
    IDLE                           = 13,
    ORDER_MOVE                     = 14,
    ORDER_ATTACK_MELEE             = 15,
    ORDER_ROLL                     = 16,
    HIDE_SWORD                     = 17,
    BLOCK                          = 18, 
    ATTACK_COUNTER                 = 19,
    ATTACK_BERSERK                 = 20,
    ATTACK_CROSS                   = 21,
    ATTACK_ROLL                    = 22,
    GOTO_TARGET                    = 23,
    FLASH                          = 24,
    REACT_TO_DAMAGE_BOSS           = 25,
    ATTACK_MELEE_SINGLE_SWORD      = 26,
    LOOK_AT_TARGET_MOVE            = 27,
    CHASE                          = 28,
    ATTACK_SAMURAI                 = 29,
    ATTACK_JUMP                    = 30,
    ROLL_FOR_BACK_STRIKE           = 31,
    ROLL_FOR_DODGE                 = 32,
    ATTACK_BOW                     = 33,
    //ATTACK_MELEE_ONCE,
    //ATTACK_BOW,    
    //MOVE,
    //GOTO_POS,
    //ATTACKBERSERK,         
    //ATTACKBOSS,         
    //ATTACKROLL,         
    //ATTACKCOUNTER,      
    //COMBAT_RUN_FORWARD,
    //COMBAT_RUN_BACKWARD,        
    //ROLL_TO_TARGET,
    //INJURY_OROCHI,
    //INJURY,
    //DEATH,
    //KNOCK_DOWN,
    COUNT,
}

[System.Serializable]
public class WorldStatePropInfo
{
    public WorldStatePropType propType;
    public bool val = false;
}

[System.Serializable]
public class WorldStatePropInfoEx
{
    public WorldStatePropInfo propInfo;
    public bool autoReverse;
}

/*
 1.goto动作如果前面忽然有人挡住，左或右拐一下弯，再冲
 2.决策是是否面对敌人背后，模糊算法？
 3.剑之道。类似功夫的闯关游戏，最后一关打自己。每关开始有一行特效tip：剑之道，贵乎奇。剑之道，攻其不备出其不意。剑之道，胜人先胜己
   三把刀，三箭客，武田信玄，服部半藏，小boss，鬼冢一郎，你自己。三箭客用动作融合实现边跑边射箭
 4.goalRound（迂回，目的是到达敌人背面）,flash选择目的位置时在道场中央放置隐形物体，flash时目标位置距离中央距离不能超过半径
 5.goapdecision新增nextTarget，由selecttarget赋值，在goalbase.onexit（bool replaced为false）时给desiredtarget赋值。
  对玩家来说直接赋值给desiredtarget。
 6.plan.build考虑支持协程
 7.主角战斗中会收刀。估计是远离敌人
 8.boss几个技能moveDisntance都是0，该如何处理让它打中对手？ 
 9.整理各个state有关旋转的代码，目前有的用rotationSmooth，有的用时间。 
 10.代码中原来由mathfx调用的函数统一改成调用phenix库里的 
 11.samurai后退roll有时会短距离甚至原地滚动？ 
 12.整理溅血粒子和击中火花、格挡的星星。刀光、灰尘、拖尾、人影。UI相关。
 13.动态人影，镜头动画，镜头震动
*/
public class GOAPDecision : Decision
{
    [SerializeField]
    List<GOAPGoalType1> _goals = new List<GOAPGoalType1>();         // 目标集合

    [SerializeField]
    List<GOAPActionType1> _actions = new List<GOAPActionType1>();   // 行为集合

    GOAP _goap = null;

    public override void Reset()
    {        
        _goap.Reset();
    }

    protected override void Start()
    {
        base.Awake();
        
        List<Phenix.Unity.AI.GOAPGoal> goals = new List<Phenix.Unity.AI.GOAPGoal>();        
        foreach (var goalType in _goals)
        {            
            switch (goalType)
            {                
                case GOAPGoalType1.IDLE:
                    {
                        List<WorldStateBitDataGoal> goalBits = new List<WorldStateBitDataGoal>();
                        goalBits.Add(new WorldStateBitDataGoal((int)WorldStatePropType.IN_IDLE, true, false));
                        goals.Add(new GOAPGoalIdle(Agent, goalBits));
                    }
                    break;
                case GOAPGoalType1.ORDER_MOVE:
                    {
                        List<WorldStateBitDataGoal> goalBits = new List<WorldStateBitDataGoal>();
                        goalBits.Add(new WorldStateBitDataGoal((int)WorldStatePropType.ORDER_MOVE, false, false));
                        goals.Add(new GOAPGoalOrderMove(Agent, goalBits));
                    }
                    break;
                case GOAPGoalType1.ORDER_ATTACK:
                    {
                        List<WorldStateBitDataGoal> goalBits = new List<WorldStateBitDataGoal>();
                        goalBits.Add(new WorldStateBitDataGoal((int)WorldStatePropType.ORDER_ATTACK, false, false));
                        goals.Add(new GOAPGoalOrderAttack(Agent, goalBits));
                    }
                    break;
                case GOAPGoalType1.ORDER_DODGE:
                    {
                        List<WorldStateBitDataGoal> goalBits = new List<WorldStateBitDataGoal>();
                        goalBits.Add(new WorldStateBitDataGoal((int)WorldStatePropType.ORDER_ROLL, false, false));
                        goals.Add(new GOAPGoalOrderDodge(Agent, goalBits));
                    }
                    break;                
                case GOAPGoalType1.CALM:
                    {
                        List<WorldStateBitDataGoal> goalBits = new List<WorldStateBitDataGoal>();
                        goalBits.Add(new WorldStateBitDataGoal((int)WorldStatePropType.WEAPON_IN_HAND, false, false));
                        goals.Add(new GOAPGoalCalm(Agent, goalBits));
                    }
                    break;
                case GOAPGoalType1.RETREAT:
                    {
                        List<WorldStateBitDataGoal> goalBits = new List<WorldStateBitDataGoal>();
                        goalBits.Add(new WorldStateBitDataGoal((int)WorldStatePropType.IN_COMBAT_RANGE, false, false));
                        goals.Add(new GOAPGoalRetreat(Agent, goalBits));
                    }
                    break;
                case GOAPGoalType1.PRESS:
                    {
                        List<WorldStateBitDataGoal> goalBits = new List<WorldStateBitDataGoal>();
                        goalBits.Add(new WorldStateBitDataGoal((int)WorldStatePropType.IN_COMBAT_RANGE, true, false));
                        goals.Add(new GOAPGoalPress(Agent, goalBits));
                    }
                    break;
                case GOAPGoalType1.AVOID_LEFT:
                    {
                        List<WorldStateBitDataGoal> goalBits = new List<WorldStateBitDataGoal>();
                        goalBits.Add(new WorldStateBitDataGoal((int)WorldStatePropType.MOVE_TO_LEFT, true, true));
                        goals.Add(new GOAPGoalAvoidLeft(Agent, goalBits));
                    }
                    break;
                case GOAPGoalType1.AVOID_RIGHT:
                    {
                        List<WorldStateBitDataGoal> goalBits = new List<WorldStateBitDataGoal>();
                        goalBits.Add(new WorldStateBitDataGoal((int)WorldStatePropType.MOVE_TO_RIGHT, true, true));
                        goals.Add(new GOAPGoalAvoidRight(Agent, goalBits));
                    }
                    break;
                case GOAPGoalType1.ALERT:
                    {
                        List<WorldStateBitDataGoal> goalBits = new List<WorldStateBitDataGoal>();
                        goalBits.Add(new WorldStateBitDataGoal((int)WorldStatePropType.LOOKING_AT_TARGET, true, false));
                        goals.Add(new GOAPGoalAlert(Agent, goalBits));
                    }
                    break;
                case GOAPGoalType1.ATTACK_TARGET:
                    {
                        List<WorldStateBitDataGoal> goalBits = new List<WorldStateBitDataGoal>();
                        goalBits.Add(new WorldStateBitDataGoal((int)WorldStatePropType.ATTACK_TARGET, true, true));
                        goals.Add(new GOAPGoalAttack(Agent, goalBits));
                    }
                    break;
                case GOAPGoalType1.SHOW:
                    {
                        List<WorldStateBitDataGoal> goalBits = new List<WorldStateBitDataGoal>();
                        goalBits.Add(new WorldStateBitDataGoal((int)WorldStatePropType.WILL_PLAY_ANIM, false, false));
                        goals.Add(new GOAPGoalShow(Agent, goalBits));
                    }
                    break;
                case GOAPGoalType1.REACT_TO_DAMAGE:
                    {
                        List<WorldStateBitDataGoal> goalBits = new List<WorldStateBitDataGoal>();
                        goalBits.Add(new WorldStateBitDataGoal((int)WorldStatePropType.REACT_TO_DAMAGE, false, false));
                        goals.Add(new GOAPGoalReactToDamage1(Agent, goalBits));
                    }
                    break;
                case GOAPGoalType1.BLOCK:
                    {
                        List<WorldStateBitDataGoal> goalBits = new List<WorldStateBitDataGoal>();
                        goalBits.Add(new WorldStateBitDataGoal((int)WorldStatePropType.FINISH_BLOCK, true, true));
                        goals.Add(new GOAPGoalBlock1(Agent, goalBits));
                    }
                    break;
                default:
                    break;
            }
        }

        List<Phenix.Unity.AI.GOAPAction> actions = new List<Phenix.Unity.AI.GOAPAction>();
        foreach (var actionType in _actions)
        {
            switch (actionType)
            {
                case GOAPActionType1.SHOW_SWORD:
                    {
                        List<WorldStateBitData> preConditionBits = new List<WorldStateBitData>();
                        preConditionBits.Add(new WorldStateBitData((int)WorldStatePropType.IN_IDLE, true));

                        List<WorldStateBitDataAction> effectBits = new List<WorldStateBitDataAction>();
                        effectBits.Add(new WorldStateBitDataAction((int)WorldStatePropType.WEAPON_IN_HAND, true, false));

                        actions.Add(new GOAPActionShowSword(GOAPActionType1.SHOW_SWORD, Agent, preConditionBits, effectBits));
                    }
                    break;
                case GOAPActionType1.LOOK_AT_TARGET:
                    {
                        List<WorldStateBitData> preConditionBits = new List<WorldStateBitData>();
                        preConditionBits.Add(new WorldStateBitData((int)WorldStatePropType.WEAPON_IN_HAND, true));

                        List<WorldStateBitDataAction> effectBits = new List<WorldStateBitDataAction>();
                        effectBits.Add(new WorldStateBitDataAction((int)WorldStatePropType.LOOKING_AT_TARGET, true, false));

                        actions.Add(new GOAPActionLookAt(GOAPActionType1.LOOK_AT_TARGET, Agent, preConditionBits, effectBits));
                    }
                    break;
                case GOAPActionType1.IDLE:
                    {
                        List<WorldStateBitData> preConditionBits = new List<WorldStateBitData>();                        

                        List<WorldStateBitDataAction> effectBits = new List<WorldStateBitDataAction>();
                        effectBits.Add(new WorldStateBitDataAction((int)WorldStatePropType.IN_IDLE, true, false));

                        actions.Add(new GOAPActionIdle(GOAPActionType1.IDLE, Agent, preConditionBits, effectBits));
                    }
                    break;
                case GOAPActionType1.GOTO_MELEE_RANGE:
                    {
                        List<WorldStateBitData> preConditionBits = new List<WorldStateBitData>();
                        preConditionBits.Add(new WorldStateBitData((int)WorldStatePropType.WEAPON_IN_HAND, true));
                        preConditionBits.Add(new WorldStateBitData((int)WorldStatePropType.LOOKING_AT_TARGET, true));
                        preConditionBits.Add(new WorldStateBitData((int)WorldStatePropType.IN_COMBAT_RANGE, true));

                        List<WorldStateBitDataAction> effectBits = new List<WorldStateBitDataAction>();
                        effectBits.Add(new WorldStateBitDataAction((int)WorldStatePropType.IN_WEAPON_RANGE, true, false));

                        actions.Add(new GOAPActionGoToMeleeRange1(GOAPActionType1.GOTO_MELEE_RANGE, Agent, preConditionBits, effectBits));
                    }
                    break;
                case GOAPActionType1.ATTACK_MELEE_MULTI_SWORDS:
                    {
                        List<WorldStateBitData> preConditionBits = new List<WorldStateBitData>();
                        preConditionBits.Add(new WorldStateBitData((int)WorldStatePropType.WEAPON_IN_HAND, true));
                        preConditionBits.Add(new WorldStateBitData((int)WorldStatePropType.LOOKING_AT_TARGET, true));
                        preConditionBits.Add(new WorldStateBitData((int)WorldStatePropType.IN_WEAPON_RANGE, true));

                        List<WorldStateBitDataAction> effectBits = new List<WorldStateBitDataAction>();
                        effectBits.Add(new WorldStateBitDataAction((int)WorldStatePropType.ATTACK_TARGET, true, false));

                        actions.Add(new GOAPActionAttackMeleeMultiSwords(GOAPActionType1.ATTACK_MELEE_MULTI_SWORDS, Agent, preConditionBits, effectBits));
                    }
                    break;
                case GOAPActionType1.ATTACK_WHIRL:
                    {
                        List<WorldStateBitData> preConditionBits = new List<WorldStateBitData>();
                        preConditionBits.Add(new WorldStateBitData((int)WorldStatePropType.WEAPON_IN_HAND, true));
                        preConditionBits.Add(new WorldStateBitData((int)WorldStatePropType.LOOKING_AT_TARGET, true));
                        preConditionBits.Add(new WorldStateBitData((int)WorldStatePropType.IN_WEAPON_RANGE, true));
                        preConditionBits.Add(new WorldStateBitData((int)WorldStatePropType.ENOUGH_BERSERK, true));

                        List<WorldStateBitDataAction> effectBits = new List<WorldStateBitDataAction>();
                        effectBits.Add(new WorldStateBitDataAction((int)WorldStatePropType.ATTACK_TARGET, true, false));

                        actions.Add(new GOAPActionAttackWhirl1(GOAPActionType1.ATTACK_WHIRL, Agent, preConditionBits, effectBits));
                    }
                    break;                
                case GOAPActionType1.ORDER_MOVE:
                    {
                        List<WorldStateBitData> preConditionBits = new List<WorldStateBitData>();                        

                        List<WorldStateBitDataAction> effectBits = new List<WorldStateBitDataAction>();
                        effectBits.Add(new WorldStateBitDataAction((int)WorldStatePropType.ORDER_MOVE, false, false));

                        actions.Add(new GOAPActionOrderMove(GOAPActionType1.ORDER_MOVE, Agent, preConditionBits, effectBits));
                    }
                    break;
                case GOAPActionType1.ORDER_ATTACK_MELEE:
                    {
                        List<WorldStateBitData> preConditionBits = new List<WorldStateBitData>();
                        preConditionBits.Add(new WorldStateBitData((int)WorldStatePropType.WEAPON_IN_HAND, true));

                        List<WorldStateBitDataAction> effectBits = new List<WorldStateBitDataAction>();
                        effectBits.Add(new WorldStateBitDataAction((int)WorldStatePropType.ORDER_ATTACK, false, false));

                        actions.Add(new GOAPActionOrderAttackMelee(GOAPActionType1.ORDER_ATTACK_MELEE, Agent, preConditionBits, effectBits));
                    }
                    break;
                case GOAPActionType1.ORDER_ROLL:
                    {
                        List<WorldStateBitData> preConditionBits = new List<WorldStateBitData>();
                        preConditionBits.Add(new WorldStateBitData((int)WorldStatePropType.WEAPON_IN_HAND, true));

                        List<WorldStateBitDataAction> effectBits = new List<WorldStateBitDataAction>();
                        effectBits.Add(new WorldStateBitDataAction((int)WorldStatePropType.ORDER_ROLL, false, false));

                        actions.Add(new GOAPActionOrderRoll(GOAPActionType1.ORDER_ROLL, Agent, preConditionBits, effectBits));
                    }
                    break;
                case GOAPActionType1.HIDE_SWORD:
                    {
                        List<WorldStateBitData> preConditionBits = new List<WorldStateBitData>();
                        preConditionBits.Add(new WorldStateBitData((int)WorldStatePropType.IN_IDLE, true));

                        List<WorldStateBitDataAction> effectBits = new List<WorldStateBitDataAction>();
                        effectBits.Add(new WorldStateBitDataAction((int)WorldStatePropType.WEAPON_IN_HAND, false, false));

                        actions.Add(new GOAPActionHideSword(GOAPActionType1.HIDE_SWORD, Agent, preConditionBits, effectBits));
                    }
                    break;
                case GOAPActionType1.COMBAT_MOVE_BACKWARD:
                    {
                        List<WorldStateBitData> preConditionBits = new List<WorldStateBitData>();
                        preConditionBits.Add(new WorldStateBitData((int)WorldStatePropType.WEAPON_IN_HAND, true));
                        preConditionBits.Add(new WorldStateBitData((int)WorldStatePropType.LOOKING_AT_TARGET, true));

                        List<WorldStateBitDataAction> effectBits = new List<WorldStateBitDataAction>();
                        effectBits.Add(new WorldStateBitDataAction((int)WorldStatePropType.IN_COMBAT_RANGE, false, false));

                        actions.Add(new GOAPActionCombatMoveBackward(GOAPActionType1.COMBAT_MOVE_BACKWARD, Agent, preConditionBits, effectBits));
                    }
                    break;
                case GOAPActionType1.COMBAT_MOVE_FORWARD:
                    {
                        List<WorldStateBitData> preConditionBits = new List<WorldStateBitData>();
                        preConditionBits.Add(new WorldStateBitData((int)WorldStatePropType.WEAPON_IN_HAND, true));
                        preConditionBits.Add(new WorldStateBitData((int)WorldStatePropType.LOOKING_AT_TARGET, true));

                        List<WorldStateBitDataAction> effectBits = new List<WorldStateBitDataAction>();
                        effectBits.Add(new WorldStateBitDataAction((int)WorldStatePropType.IN_COMBAT_RANGE, true, false));

                        actions.Add(new GOAPActionCombatMoveForward(GOAPActionType1.COMBAT_MOVE_FORWARD, Agent, preConditionBits, effectBits));
                    }
                    break;
                case GOAPActionType1.COMBAT_MOVE_LEFT:
                    {
                        List<WorldStateBitData> preConditionBits = new List<WorldStateBitData>();
                        preConditionBits.Add(new WorldStateBitData((int)WorldStatePropType.WEAPON_IN_HAND, true));
                        preConditionBits.Add(new WorldStateBitData((int)WorldStatePropType.LOOKING_AT_TARGET, true));

                        List<WorldStateBitDataAction> effectBits = new List<WorldStateBitDataAction>();
                        effectBits.Add(new WorldStateBitDataAction((int)WorldStatePropType.MOVE_TO_LEFT, true, false));

                        actions.Add(new GOAPActionCombatMoveLeft(GOAPActionType1.COMBAT_MOVE_LEFT, Agent, preConditionBits, effectBits));
                    }
                    break;
                case GOAPActionType1.COMBAT_MOVE_RIGHT:
                    {
                        List<WorldStateBitData> preConditionBits = new List<WorldStateBitData>();
                        preConditionBits.Add(new WorldStateBitData((int)WorldStatePropType.WEAPON_IN_HAND, true));
                        preConditionBits.Add(new WorldStateBitData((int)WorldStatePropType.LOOKING_AT_TARGET, true));

                        List<WorldStateBitDataAction> effectBits = new List<WorldStateBitDataAction>();
                        effectBits.Add(new WorldStateBitDataAction((int)WorldStatePropType.MOVE_TO_RIGHT, true, false));

                        actions.Add(new GOAPActionCombatMoveRight(GOAPActionType1.COMBAT_MOVE_RIGHT, Agent, preConditionBits, effectBits));
                    }
                    break;
                case GOAPActionType1.PLAY_ANIM:
                    {
                        List<WorldStateBitData> preConditionBits = new List<WorldStateBitData>();
                        preConditionBits.Add(new WorldStateBitData((int)WorldStatePropType.WEAPON_IN_HAND, true));

                        List<WorldStateBitDataAction> effectBits = new List<WorldStateBitDataAction>();
                        effectBits.Add(new WorldStateBitDataAction((int)WorldStatePropType.WILL_PLAY_ANIM, false, true));

                        actions.Add(new GOAPActionPlayAnim(GOAPActionType1.PLAY_ANIM, Agent, preConditionBits, effectBits));
                    }
                    break;
                case GOAPActionType1.REACT_TO_DAMAGE:
                    {
                        List<WorldStateBitData> preConditionBits = new List<WorldStateBitData>();

                        List<WorldStateBitDataAction> effectBits = new List<WorldStateBitDataAction>();
                        effectBits.Add(new WorldStateBitDataAction((int)WorldStatePropType.REACT_TO_DAMAGE, false, false));

                        actions.Add(new GOAPActionReactToDamage(GOAPActionType1.REACT_TO_DAMAGE, Agent, preConditionBits, effectBits));
                    }
                    break;
                case GOAPActionType1.BLOCK:
                    {
                        List<WorldStateBitData> preConditionBits = new List<WorldStateBitData>();
                        preConditionBits.Add(new WorldStateBitData((int)WorldStatePropType.WEAPON_IN_HAND, true));
                        preConditionBits.Add(new WorldStateBitData((int)WorldStatePropType.LOOKING_AT_TARGET, true));

                        List<WorldStateBitDataAction> effectBits = new List<WorldStateBitDataAction>();
                        effectBits.Add(new WorldStateBitDataAction((int)WorldStatePropType.FINISH_BLOCK, true, false));

                        actions.Add(new GOAPActionBlock1(GOAPActionType1.BLOCK, Agent, preConditionBits, effectBits));
                    }
                    break;
                case GOAPActionType1.ATTACK_COUNTER:
                    {
                        List<WorldStateBitData> preConditionBits = new List<WorldStateBitData>();
                        preConditionBits.Add(new WorldStateBitData((int)WorldStatePropType.IN_BLOCK, true));
                        preConditionBits.Add(new WorldStateBitData((int)WorldStatePropType.ENOUGH_BERSERK, true));

                        List<WorldStateBitDataAction> effectBits = new List<WorldStateBitDataAction>();
                        effectBits.Add(new WorldStateBitDataAction((int)WorldStatePropType.ATTACK_TARGET, false, false));

                        actions.Add(new GOAPActionAttackCounter(GOAPActionType1.ATTACK_COUNTER, Agent, preConditionBits, effectBits));
                    }
                    break;
                case GOAPActionType1.ATTACK_BERSERK:
                    {
                        List<WorldStateBitData> preConditionBits = new List<WorldStateBitData>();
                        preConditionBits.Add(new WorldStateBitData((int)WorldStatePropType.WEAPON_IN_HAND, true));
                        preConditionBits.Add(new WorldStateBitData((int)WorldStatePropType.LOOKING_AT_TARGET, true));
                        preConditionBits.Add(new WorldStateBitData((int)WorldStatePropType.IN_WEAPON_RANGE, true));
                        preConditionBits.Add(new WorldStateBitData((int)WorldStatePropType.ENOUGH_BERSERK, true));

                        List<WorldStateBitDataAction> effectBits = new List<WorldStateBitDataAction>();
                        effectBits.Add(new WorldStateBitDataAction((int)WorldStatePropType.ATTACK_TARGET, true, false));

                        actions.Add(new GOAPActionAttackBerserk(GOAPActionType1.ATTACK_BERSERK, Agent, preConditionBits, effectBits));
                    }
                    break;
                case GOAPActionType1.ATTACK_CROSS:
                    {
                        List<WorldStateBitData> preConditionBits = new List<WorldStateBitData>();
                        preConditionBits.Add(new WorldStateBitData((int)WorldStatePropType.WEAPON_IN_HAND, true));
                        preConditionBits.Add(new WorldStateBitData((int)WorldStatePropType.LOOKING_AT_TARGET, true));
                        preConditionBits.Add(new WorldStateBitData((int)WorldStatePropType.IN_WEAPON_RANGE, true));

                        List<WorldStateBitDataAction> effectBits = new List<WorldStateBitDataAction>();
                        effectBits.Add(new WorldStateBitDataAction((int)WorldStatePropType.ATTACK_TARGET, true, false));

                        actions.Add(new GOAPActionAttackCross(GOAPActionType1.ATTACK_CROSS, Agent, preConditionBits, effectBits));
                    }
                    break;
                case GOAPActionType1.GOTO_TARGET:
                    {
                        List<WorldStateBitData> preConditionBits = new List<WorldStateBitData>();
                        preConditionBits.Add(new WorldStateBitData((int)WorldStatePropType.WEAPON_IN_HAND, true));
                        preConditionBits.Add(new WorldStateBitData((int)WorldStatePropType.LOOKING_AT_TARGET, true));

                        List<WorldStateBitDataAction> effectBits = new List<WorldStateBitDataAction>();
                        effectBits.Add(new WorldStateBitDataAction((int)WorldStatePropType.IN_WEAPON_RANGE, true, false));

                        actions.Add(new GOAPActionGoToTarget(GOAPActionType1.GOTO_TARGET, Agent, preConditionBits, effectBits));
                    }
                    break;
                case GOAPActionType1.FLASH:
                    {
                        List<WorldStateBitData> preConditionBits = new List<WorldStateBitData>();
                        preConditionBits.Add(new WorldStateBitData((int)WorldStatePropType.WEAPON_IN_HAND, true));
                        preConditionBits.Add(new WorldStateBitData((int)WorldStatePropType.LOOKING_AT_TARGET, true));
                        preConditionBits.Add(new WorldStateBitData((int)WorldStatePropType.IN_COMBAT_RANGE, true));
                        preConditionBits.Add(new WorldStateBitData((int)WorldStatePropType.ENOUGH_BERSERK, true));

                        List<WorldStateBitDataAction> effectBits = new List<WorldStateBitDataAction>();
                        effectBits.Add(new WorldStateBitDataAction((int)WorldStatePropType.IN_WEAPON_RANGE, true, false));

                        actions.Add(new GOAPActionFlash(GOAPActionType1.FLASH, Agent, preConditionBits, effectBits));
                    }
                    break;
                case GOAPActionType1.ATTACK_ROLL:
                    {
                        List<WorldStateBitData> preConditionBits = new List<WorldStateBitData>();
                        preConditionBits.Add(new WorldStateBitData((int)WorldStatePropType.WEAPON_IN_HAND, true));
                        preConditionBits.Add(new WorldStateBitData((int)WorldStatePropType.LOOKING_AT_TARGET, true));
                        preConditionBits.Add(new WorldStateBitData((int)WorldStatePropType.ENOUGH_BERSERK, true));

                        List<WorldStateBitDataAction> effectBits = new List<WorldStateBitDataAction>();
                        effectBits.Add(new WorldStateBitDataAction((int)WorldStatePropType.ATTACK_TARGET, true, false));

                        actions.Add(new GOAPActionAttackRoll(GOAPActionType1.ATTACK_ROLL, Agent, preConditionBits, effectBits));
                    }
                    break;
                case GOAPActionType1.REACT_TO_DAMAGE_BOSS:
                    {
                        List<WorldStateBitData> preConditionBits = new List<WorldStateBitData>();

                        List<WorldStateBitDataAction> effectBits = new List<WorldStateBitDataAction>();
                        effectBits.Add(new WorldStateBitDataAction((int)WorldStatePropType.REACT_TO_DAMAGE, false, false));

                        actions.Add(new GOAPActionReactToDamageBoss(GOAPActionType1.REACT_TO_DAMAGE_BOSS, Agent, preConditionBits, effectBits));
                    }
                    break;
                case GOAPActionType1.ATTACK_MELEE_SINGLE_SWORD:
                    {
                        List<WorldStateBitData> preConditionBits = new List<WorldStateBitData>();
                        preConditionBits.Add(new WorldStateBitData((int)WorldStatePropType.WEAPON_IN_HAND, true));
                        preConditionBits.Add(new WorldStateBitData((int)WorldStatePropType.LOOKING_AT_TARGET, true));
                        preConditionBits.Add(new WorldStateBitData((int)WorldStatePropType.IN_WEAPON_RANGE, true));

                        List<WorldStateBitDataAction> effectBits = new List<WorldStateBitDataAction>();
                        effectBits.Add(new WorldStateBitDataAction((int)WorldStatePropType.ATTACK_TARGET, true, false));

                        actions.Add(new GOAPActionAttackMeleeSingleSword(GOAPActionType1.ATTACK_MELEE_SINGLE_SWORD, Agent, preConditionBits, effectBits));
                    }
                    break;
                case GOAPActionType1.LOOK_AT_TARGET_MOVE:
                    {
                        List<WorldStateBitData> preConditionBits = new List<WorldStateBitData>();
                        preConditionBits.Add(new WorldStateBitData((int)WorldStatePropType.WEAPON_IN_HAND, true));

                        List<WorldStateBitDataAction> effectBits = new List<WorldStateBitDataAction>();
                        effectBits.Add(new WorldStateBitDataAction((int)WorldStatePropType.LOOKING_AT_TARGET, true, false));

                        actions.Add(new GOAPActionLookAtMove(GOAPActionType1.LOOK_AT_TARGET_MOVE, Agent, preConditionBits, effectBits));
                    }
                    break;
                case GOAPActionType1.CHASE:
                    {
                        List<WorldStateBitData> preConditionBits = new List<WorldStateBitData>();
                        preConditionBits.Add(new WorldStateBitData((int)WorldStatePropType.WEAPON_IN_HAND, true));
                        preConditionBits.Add(new WorldStateBitData((int)WorldStatePropType.LOOKING_AT_TARGET, true));

                        List<WorldStateBitDataAction> effectBits = new List<WorldStateBitDataAction>();
                        effectBits.Add(new WorldStateBitDataAction((int)WorldStatePropType.IN_COMBAT_RANGE, true, false));

                        actions.Add(new GOAPActionChase(GOAPActionType1.CHASE, Agent, preConditionBits, effectBits));
                    }
                    break;
                case GOAPActionType1.ATTACK_SAMURAI:
                    {
                        List<WorldStateBitData> preConditionBits = new List<WorldStateBitData>();
                        preConditionBits.Add(new WorldStateBitData((int)WorldStatePropType.WEAPON_IN_HAND, true));                        
                        preConditionBits.Add(new WorldStateBitData((int)WorldStatePropType.IN_COMBAT_RANGE, true));

                        List<WorldStateBitDataAction> effectBits = new List<WorldStateBitDataAction>();
                        effectBits.Add(new WorldStateBitDataAction((int)WorldStatePropType.ATTACK_TARGET, true, false));

                        actions.Add(new GOAPActionAttackSamurai(GOAPActionType1.ATTACK_SAMURAI, Agent, preConditionBits, effectBits));
                    }
                    break;
                case GOAPActionType1.ATTACK_JUMP:
                    {
                        List<WorldStateBitData> preConditionBits = new List<WorldStateBitData>();
                        preConditionBits.Add(new WorldStateBitData((int)WorldStatePropType.WEAPON_IN_HAND, true));                        
                        preConditionBits.Add(new WorldStateBitData((int)WorldStatePropType.IN_COMBAT_RANGE, true));
                        preConditionBits.Add(new WorldStateBitData((int)WorldStatePropType.TARGET_IN_KNOCK_DOWN, true));

                        List<WorldStateBitDataAction> effectBits = new List<WorldStateBitDataAction>();
                        effectBits.Add(new WorldStateBitDataAction((int)WorldStatePropType.ATTACK_TARGET, true, false));

                        actions.Add(new GOAPActionAttackJump(GOAPActionType1.ATTACK_JUMP, Agent, preConditionBits, effectBits));
                    }
                    break;
                case GOAPActionType1.ROLL_FOR_BACK_STRIKE:
                    {
                        List<WorldStateBitData> preConditionBits = new List<WorldStateBitData>();
                        preConditionBits.Add(new WorldStateBitData((int)WorldStatePropType.WEAPON_IN_HAND, true));
                        preConditionBits.Add(new WorldStateBitData((int)WorldStatePropType.IN_COMBAT_RANGE, true));                        

                        List<WorldStateBitDataAction> effectBits = new List<WorldStateBitDataAction>();
                        effectBits.Add(new WorldStateBitDataAction((int)WorldStatePropType.BEHIND_TARGET, true, false));

                        actions.Add(new GOAPActionRollForBackStrike(GOAPActionType1.ROLL_FOR_BACK_STRIKE, Agent, preConditionBits, effectBits));
                    }
                    break;
                case GOAPActionType1.ROLL_FOR_DODGE:
                    {
                        List<WorldStateBitData> preConditionBits = new List<WorldStateBitData>();
                        preConditionBits.Add(new WorldStateBitData((int)WorldStatePropType.WEAPON_IN_HAND, true));                        

                        List<WorldStateBitDataAction> effectBits = new List<WorldStateBitDataAction>();
                        effectBits.Add(new WorldStateBitDataAction((int)WorldStatePropType.IN_COMBAT_RANGE, false, false));

                        actions.Add(new GOAPActionRollForDodge(GOAPActionType1.ROLL_FOR_DODGE, Agent, preConditionBits, effectBits));
                    }
                    break;
                case GOAPActionType1.ATTACK_BOW:
                    {
                        List<WorldStateBitData> preConditionBits = new List<WorldStateBitData>();
                        preConditionBits.Add(new WorldStateBitData((int)WorldStatePropType.WEAPON_IN_HAND, true));
                        preConditionBits.Add(new WorldStateBitData((int)WorldStatePropType.IN_WEAPON_RANGE, true));
                        preConditionBits.Add(new WorldStateBitData((int)WorldStatePropType.LOOKING_AT_TARGET, true));

                        List<WorldStateBitDataAction> effectBits = new List<WorldStateBitDataAction>();
                        effectBits.Add(new WorldStateBitDataAction((int)WorldStatePropType.ATTACK_TARGET, true, false));

                        actions.Add(new GOAPActionAttackBow(GOAPActionType1.ATTACK_BOW, Agent, preConditionBits, effectBits));
                    }
                    break;
                default:
                    break;
            }
        }
        _goap = new GOAP(new Phenix.Unity.AI.WorldState((int)GOAPActionType1.COUNT + 1), 
            goals, actions, new GOAPPlanAStar());
    }

    // Update is called once per frame
    protected override void Update()
    {
        if (Agent.IsPlayer == false)
        {
            // AI
            UpdateTarget(Agent.BlackBoard.safeRange, Game1.Instance.AgentMgr.agents);
        }
        else
        {
            // player
            UpdateOrder();
        }
        UpdateWorldState();
        UpdateGOAP();
    }

    public void SetWorldState(WorldStatePropType worldStatePropType, bool val)
    {
        _goap.WorldState.Set((int)worldStatePropType, val);
    }

    protected void UpdateOrder()
    {
        if (Agent.PlayerOrder == null)
        {
            return;            
        }

        if (_goap.WorldState.Get((int)WorldStatePropType.REACT_TO_DAMAGE))
        {
            // 如果存在REACT_TO_DAMAGE，则清除所有输入
            _goap.WorldState.Set((int)WorldStatePropType.ORDER_MOVE, false);
            _goap.WorldState.Set((int)WorldStatePropType.ORDER_ATTACK, false);
            _goap.WorldState.Set((int)WorldStatePropType.ORDER_ROLL, false);
            Agent.PlayerOrder.Clear();
            return;
        }
    }

    protected void UpdateGOAP()
    {
        _goap.OnUpdate();
    }

    protected void UpdateWorldState()
    {
        // 更新WorldState里某些实时属性值
        _goap.WorldState.Set((int)WorldStatePropType.ENOUGH_BERSERK, Agent.BlackBoard.Berserk > Agent.BlackBoard.maxBerserk * 0.75f);
        _goap.WorldState.Set((int)WorldStatePropType.IN_WEAPON_RANGE, Agent.BlackBoard.DesiredTargetInWeaponRange);
        _goap.WorldState.Set((int)WorldStatePropType.IN_COMBAT_RANGE, Agent.BlackBoard.DesiredTargetInCombatRange);
        _goap.WorldState.Set((int)WorldStatePropType.LOOKING_AT_TARGET, Agent.BlackBoard.LookAtDesiredTarget);
        _goap.WorldState.Set((int)WorldStatePropType.WEAPON_IN_HAND, Agent.BlackBoard.WeaponInHand);        
        _goap.WorldState.Set((int)WorldStatePropType.IN_IDLE, Agent.BlackBoard.inIdle);
        _goap.WorldState.Set((int)WorldStatePropType.IN_BLOCK, Agent.BlackBoard.IsBlocking);
        _goap.WorldState.Set((int)WorldStatePropType.TARGET_IN_KNOCK_DOWN, Agent.BlackBoard.DesiredTargetIsKnockedDown);
        _goap.WorldState.Set((int)WorldStatePropType.AHEAD_OF_TARGET, Agent.BlackBoard.AheadOfDesiredTarget);
        _goap.WorldState.Set((int)WorldStatePropType.BEHIND_TARGET, Agent.BlackBoard.BehindDesiredTarget);
    }

    public override void OnDead()
    {        
        base.OnDead();
        _goap.WorldState.Set((int)WorldStatePropType.REACT_TO_DAMAGE, true);        
    }

    public override void OnInjury()
    {
        base.OnInjury();
        _goap.WorldState.Set((int)WorldStatePropType.REACT_TO_DAMAGE, true);        
    }

    public override void OnKnockDown()
    {
        base.OnKnockDown();
        _goap.WorldState.Set((int)WorldStatePropType.REACT_TO_DAMAGE, true);
    }
}
