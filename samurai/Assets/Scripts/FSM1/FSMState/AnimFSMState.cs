using UnityEngine.Events;
using Phenix.Unity.AI;

public enum AnimFSMStateType
{
    NONE = 0,
    IDLE,
    GOTO,
    COMBAT_MOVE,
    ATTACK_MELEE,
    ATTACK_WHIRL,
    INJURY,
    DEATH,
    KNOCK_DOWN,
    ROTATE,    
    PLAY_ANIM,
    MOVE,
    ROLL,
    BLOCK,
    ATTACK_ROLL,
    ATTACK_CROSS,
    GOTO_TARGET,    // 行进到武器范围之内
}

public abstract class AnimFSMState : FSMState
{
    protected Agent1 Agent { get; private set; }

    protected UnityAction<AttackMeleeHitData> onAttackHit = new UnityAction<AttackMeleeHitData>(HandleAttackResult.AttackMeleeHitHandler);
    protected UnityAction<AttackWhirlHitData> onAttackWhirlHit = new UnityAction<AttackWhirlHitData>(HandleAttackResult.AttackWhirlHitHandler);
    protected UnityAction<MontageShotData> onMontageShot = new UnityAction<MontageShotData>(HandleMontageShot.MontageShotHandler);
    protected UnityAction<SoundData> onSound = new UnityAction<SoundData>(HandleSound.SoundHandler);
    protected UnityAction<EffectData> onEffect = new UnityAction<EffectData>(HandleEffect.EffectHandler);
    
    public AnimFSMState(int stateType, Agent1 agent) : base(stateType)
    {
        Agent = agent;
    }

    /*protected void CrossFade(string anim, float fadeInTime)
    {
        if (Agent.AnimEngine.IsPlaying(anim))
        {
            // 如果自身正在被播放
            Agent.AnimEngine.CrossFadeQueued(anim, fadeInTime, QueueMode.PlayNow);
        }
        else
        {
            Agent.AnimEngine.CrossFade(anim, fadeInTime);
        }
    }*/

    // 当侧面发生碰撞（可选）或离地时返回false
    //protected bool MoveOnGround(Vector3 velocity, bool allowSideCollision /*= true*/ )
    /*{
        Vector3 old = Agent.Transform.position;

        // -----为了保证始终和地面接触begin------
        Agent.Transform.position += Vector3.up * Time.deltaTime;
        velocity.y -= 9 * Time.deltaTime;
        // -----为了保证始终和地面接触end------

        CollisionFlags flags = Agent.CharacterController.Move(velocity); // CharacterController.Move无视重力

        if (allowSideCollision == false && (flags & CollisionFlags.Sides) != 0) // （flags & CollisionFlags.Sides) != 0 侧面碰撞
        {
            Agent.Transform.position = old;
            return false;
        }

        if ((flags & CollisionFlags.Below) == 0) // (flags & CollisionFlags.Below) == 0 没有和地面碰撞
        {
            Agent.Transform.position = old;
            return false;
        }

        return true;
    }*/

    /*protected bool MoveEx(Vector3 velocity)
    {
        Vector3 old = Agent.Transform.position;
        Agent.Transform.position += Vector3.up * Time.deltaTime;
        velocity.y -= 9 * Time.deltaTime;
        CollisionFlags flags = Agent.CharacterController.Move(velocity);
        if (flags == CollisionFlags.None)
        {
            RaycastHit hit;
            if (Physics.Raycast(Agent.Transform.position, -Vector3.up, out hit, 3) == false)
            {
                Agent.Transform.position = old;
                return false;
            }
        }

        return true;
    }*/
}