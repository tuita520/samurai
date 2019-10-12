using UnityEngine;
using Phenix.Unity.Utilities;
using Phenix.Unity.AI;
using Phenix.Unity.Extend;

public class AnimFSMStateAttackWhirl : AnimFSMState
{
    AnimFSMEventAttackWhirl _eventWhirl;
    float _maxSpeed;
    float _timeToEndState;    
    
    float _timeToStartEffect;
    float _timeToEndEffect;

    Quaternion _finalRotation = new Quaternion();
    Quaternion _startRotation = new Quaternion();

    float _hitTimer;
    float _rotationProgress;    

    public AnimFSMStateAttackWhirl(Agent1 agent)
        : base((int)AnimFSMStateType.ATTACK_WHIRL, agent)
    {

    }

    public override void OnEnter(FSMEvent ev = null)
    {
        base.OnEnter(ev);
        Agent.BlackBoard.invulnerable = true;
        if (Agent.particleSystemWhirlWind != null && Agent.BlackBoard.showMotionEffect)
        {
            ParticleTools.Instance.Play(Agent.particleSystemWhirlWind, 0, 1f);
        }
    }

    public override void OnExit()
    {
        Agent.BlackBoard.invulnerable = false;
        Agent.BlackBoard.speed = 0;
        ParticleTools.Instance.Stop(Agent.particleSystemWhirlWind);
    }

    protected override void Initialize(FSMEvent ev)
    {
        _eventWhirl = ev as AnimFSMEventAttackWhirl;

        AnimationTools.PlayAnim(Agent.AnimEngine, _eventWhirl.data.animName, 0.2f);
        //UpdateFinalRotation();
        Agent.BlackBoard.motionType = MotionType.ATTACK;// MotionType.WALK;
        _rotationProgress = 0;
        _timeToEndState = Agent.AnimEngine[_eventWhirl.data.animName].length * 0.9f + Time.timeSinceLevelLoad;
        _hitTimer = Time.timeSinceLevelLoad + 0.75f;

        // Owner.PlayLoopSound(Owner.BerserkSound, 1, AnimEngine[Action.Data.AnimName].length - 1, 0.5f, 0.9f);

        _timeToStartEffect = Time.timeSinceLevelLoad + 1;
        _timeToEndEffect = Time.timeSinceLevelLoad + Agent.AnimEngine[_eventWhirl.data.animName].length - 1;        
    }

    public override void OnUpdate()
    {
        UpdateFinalRotation();

        _rotationProgress += Time.deltaTime * Agent.BlackBoard.rotationSmoothInMove;
        _rotationProgress = Mathf.Min(_rotationProgress, 1);
        Agent.Transform.rotation = Quaternion.Slerp(_startRotation, _finalRotation, _rotationProgress);        
        if (Agent.AnimEngine[_eventWhirl.data.animName].time > 
                Agent.AnimEngine[_eventWhirl.data.animName].length * 0.1f /*为了防止动画开始时候的滑步*/)
        {
            float preSpeed = Agent.BlackBoard.speed;
            float curSmooth = Agent.BlackBoard.speedSmooth * Time.deltaTime;
            Agent.BlackBoard.speed = Mathfx.Hermite(Agent.BlackBoard.speed, 
                Agent.BlackBoard.maxWhirlMoveSpeed, curSmooth);
            Agent.BlackBoard.moveDir = Agent.Forward;

            float dist = Agent.BlackBoard.speed * Time.deltaTime;
            bool _moveOk = TransformTools.MoveOnGround(Agent.transform, Agent.CharacterController, 
                Agent.BlackBoard.moveDir * dist, true);
            if (_moveOk == false)
            {
                Agent.BlackBoard.speed = preSpeed;
            }
        }     

        if (_hitTimer < Time.timeSinceLevelLoad) // 伤害结算计时
        {
            HandleAttackResult.DoMeleeDamage(Agent, _eventWhirl.target/*Agent.BlackBoard.desiredTarget*/, Agent.BlackBoard.weaponSelected,
                _eventWhirl.data, false, false, false);
            _hitTimer = Time.timeSinceLevelLoad + Agent.BlackBoard.attackWhirlHitTime;//0.75f;
        }

        if (_timeToEndState < Time.timeSinceLevelLoad)
        {
            IsFinished = true;
            _eventWhirl.IsFinished = true;
        }
    }

    void UpdateFinalRotation()
    {
        Vector3 dir;
        if (_eventWhirl.target/*Agent.BlackBoard.desiredTarget*/ != null)
        {
            dir = _eventWhirl.target/*Agent.BlackBoard.desiredTarget*/.Position - Agent.Position;
        }
        else
        {
            dir = Agent.Forward;
        }
        dir.Normalize();

        _finalRotation.SetLookRotation(dir);
        _startRotation = Agent.Transform.rotation;

        if (_startRotation != _finalRotation)
            _rotationProgress = 0;
    }
}