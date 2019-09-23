using UnityEngine.Events;
using Phenix.Unity.AI;

public enum AnimFSMStateType
{
    NONE = 0,
    IDLE            = 1,     // 待机
    GOTO            = 2,     // 朝目的地跑动
    COMBAT_MOVE     = 3,     // 战斗中行走
    ATTACK_MELEE    = 4,
    ATTACK_WHIRL    = 5,
    INJURY          = 6,
    DEATH           = 7,
    KNOCK_DOWN      = 8,     // 被击倒
    ROTATE          = 9,     // 站定旋转
    PLAY_ANIM       = 10,    // 播放动画（挑衅、暴怒等）
    MOVE            = 11,    // 朝指定方向持续移动
    ROLL            = 12,
    BLOCK           = 13,    // 格挡
    ATTACK_ROLL     = 14,
    ATTACK_CROSS    = 15,
    ATTACK_BOW      = 16,    
    FLASH           = 17,    // 瞬移到指定位置    
    INJURY_BOSS     = 18,
    MOVE_ROTATE     = 19,    // 边移动边旋转    
}

public abstract class AnimFSMState : FSMState
{
    protected Agent1 Agent { get; private set; }

    //protected UnityAction<AttackMeleeHitData> onAttackHit = new UnityAction<AttackMeleeHitData>(HandleAttackResult.AttackMeleeHitHandler);
    //protected UnityAction<AttackWhirlHitData> onAttackWhirlHit = new UnityAction<AttackWhirlHitData>(HandleAttackResult.AttackWhirlHitHandler);
    //protected UnityAction<MontageShotData> onMontageShot = new UnityAction<MontageShotData>(HandleCamera.SlowMotion);
    //protected UnityAction<SoundData> onSound = new UnityAction<SoundData>(HandleSound.SoundHandler);
    //protected UnityAction<EffectData> onEffect = new UnityAction<EffectData>(HandleEffect.EffectHandler);
    
    public AnimFSMState(int stateType, Agent1 agent) : base(stateType)
    {
        Agent = agent;
    }    
}