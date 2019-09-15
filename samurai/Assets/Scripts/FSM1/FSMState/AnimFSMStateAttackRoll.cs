using UnityEngine;
using Phenix.Unity.Utilities;
using Phenix.Unity.AI;

public class AnimFSMStateAttackRoll : AnimFSMState
{
    enum AttackRollState
    {
        PREPARE,
        ROLL,
        STAND_UP,
    }
    
    AnimFSMEventAttackRoll _eventAttackRoll;

    Quaternion _finalRotation;
    Quaternion _startRotation;

    float _curRotationTime;
    float _rotationTime;
    float _endOfStateTime;
    float _hitTimer;

    //CombatEffectsManager.CacheData Effect;

    bool _rotationOk = false;
    AttackRollState _state;


    public AnimFSMStateAttackRoll(Agent1 agent)
        : base((int)AnimFSMStateType.ATTACK_ROLL, agent)
    {

    }

    public override void OnEnter(FSMEvent ev = null)
    {
        base.OnEnter(ev);
        Agent.BlackBoard.invulnerable = true;        
        //Effect = null;
    }

    public override void OnExit()
    {
        Agent.BlackBoard.invulnerable = false;
        Agent.BlackBoard.speed = 0;
        //if (Effect != null)
        //    CombatEffectsManager.Instance.ReturnRolllEffect(Effect);

        //Effect = null;
    }

    protected override void Initialize(FSMEvent ev)
    {
        _eventAttackRoll = ev as AnimFSMEventAttackRoll;

        _state = AttackRollState.PREPARE;
        Tools.PlayAnimation(Agent.AnimEngine, "attackRollStart", 0.4f);
        base.Initialize(_eventAttackRoll);
        Agent.BlackBoard.motionType = MotionType.NONE;
        _endOfStateTime = Agent.AnimEngine["attackRollStart"].length * 0.95f + Time.timeSinceLevelLoad;
        _hitTimer = 0;
        UpdateFinalRotation();
        //Agent.SoundPlay(Agent.RollSounds[0]);
    }

    public override void OnUpdate()
    {
        switch (_state)
        {
            case AttackRollState.PREPARE:
                {
                    UpdateFinalRotation();
                    if (_rotationOk == false)
                    {
                        _curRotationTime += Time.deltaTime;
                        if (_curRotationTime >= _rotationTime)
                        {
                            _curRotationTime = _rotationTime;
                            _rotationOk = true;
                        }

                        float progress = _curRotationTime / _rotationTime;
                        Quaternion q = Quaternion.Lerp(_startRotation, _finalRotation, progress);
                        Agent.Transform.rotation = q;
                    }

                    if (_endOfStateTime < Time.timeSinceLevelLoad)
                        InitializeRoll();
                }
                break;
            case AttackRollState.ROLL: 
                {                    
                    if (TransformTools.MoveOnGround(Agent.Transform, Agent.CharacterController, 
                        Agent.Transform.forward * Agent.BlackBoard.attackRollSpeed * Time.deltaTime, false) == false)
                    {
                        HandleAttackResult.DoRollDamage(Agent, _eventAttackRoll.animAttackData, Agent.BlackBoard.attackRollWeaponRange);                        
                        InitializeStandUp();
                    }
                    else if (_hitTimer < Time.timeSinceLevelLoad)
                    {
                        HandleAttackResult.DoRollDamage(Agent, _eventAttackRoll.animAttackData, Agent.BlackBoard.attackRollWeaponRange);
                        _hitTimer = Time.timeSinceLevelLoad + Agent.BlackBoard.attackRollHitTime;//0.2f;
                    }
                }
                break;
            case AttackRollState.STAND_UP:
                {
                    if (_endOfStateTime < Time.timeSinceLevelLoad)
                    {
                        IsFinished = true;
                        _eventAttackRoll.IsFinished = true;
                    }
                }
                break;
        }
    }

    void InitializeRoll()
    {
        _state = AttackRollState.ROLL;
        Tools.PlayAnimation(Agent.AnimEngine, "attackRollLoop", 0.1f);
        Agent.BlackBoard.motionType = MotionType.ROLL;
        //Effect = CombatEffectsManager.Instance.PlayRollEffect(Transform);
    }

    void InitializeStandUp()
    {
        _state = AttackRollState.STAND_UP;
        Tools.PlayAnimation(Agent.AnimEngine, "attackRollEnd", 0.1f);
        Agent.BlackBoard.motionType = MotionType.ROLL;
        _endOfStateTime = Agent.AnimEngine["attackRollEnd"].length * 0.95f + Time.timeSinceLevelLoad;
        //CombatEffectsManager.Instance.ReturnRolllEffect(Effect);
        //Effect = null;
        //Agent.SoundPlay(Agent.RollSounds[2]);
    }

    void UpdateFinalRotation()
    {
        if (_eventAttackRoll.target == null)
            return;

        Vector3 dir = _eventAttackRoll.target.Position - Agent.Position;
        dir.Normalize();

        _startRotation = Agent.Transform.rotation;
        _finalRotation.SetLookRotation(dir);

        float angle = Vector3.Angle(Agent.Transform.forward, dir);

        if (angle > 0)
        {
            _rotationTime = angle / 100.0f;
            _rotationOk = false;
            _curRotationTime = 0;
        }
        else
        {
            _rotationOk = true;
            _rotationTime = 0;
        }
    }
}