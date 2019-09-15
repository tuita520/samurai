using UnityEngine;
using Phenix.Unity.Utilities;
using Phenix.Unity.AI;

public class AnimFSMStateAttackMelee : AnimFSMState
{
    enum AttackStatus
    {
        PREPARING,
        ATTACKING,
        FINISHED,
    }

    AnimFSMEventAttackMelee _eventAttackMelee;

    Quaternion _finalRotation;
    Quaternion _startRotation;
    Vector3 _startPosition;
    Vector3 _finalPosition;
    float _currentRotationTime;
    float _rotationTime;
    float _moveTime;
    float _currentMoveTime;
    float _endOfStateTime;
    float _hitTime;
    float _attackPhaseTime;

    bool _rotationOk = false;
    bool _positionOK = false;

    bool _isCritical = false;
    bool _knockdown = false;
    bool _backHit = false;
    AttackStatus _attackStatus;
    bool _hitTimeStart;

    public AnimFSMStateAttackMelee(Agent1 agent)
        : base((int)AnimFSMStateType.ATTACK_MELEE, agent)
    {

    }

    public override void OnExit()
    {
        Agent.BlackBoard.speed = 0;        
    }

    public override bool OnEvent(FSMEvent ev)
    {
        if (ev is AnimFSMEventAttackMelee)
        {
            //if (_eventAttackMelee != null)
            //{
            //    _eventAttackMelee.IsFinished = true;
            //}
            Initialize(ev);
            return true;
        }      

        return false;
    }

    protected override void Initialize(FSMEvent ev = null)
    {
        _eventAttackMelee = ev as AnimFSMEventAttackMelee;        

        _attackStatus = AttackStatus.PREPARING;
        Agent.BlackBoard.motionType = MotionType.ATTACK;        
        _startRotation = Agent.Transform.rotation;
        _startPosition = Agent.Transform.position;
                
        if (_eventAttackMelee.target != null)
        {
            float angle = 0;
            float distance = 0;
            Vector3 dir = _eventAttackMelee.target.Position - Agent.Transform.position;
            distance = dir.magnitude;
            if (distance > 0.1f)
            {
                dir.Normalize();
                angle = Vector3.Angle(Agent.Transform.forward, dir);
                if (angle < 40 && Vector3.Angle(Agent.Forward, _eventAttackMelee.target.Forward) < 80)
                    _backHit = true;
            }
            else
            {
                dir = Agent.Transform.forward;
            }

            _finalRotation.SetLookRotation(dir);

            if (distance < Agent.BlackBoard.weaponRange)
                _finalPosition = _startPosition;
            else
                _finalPosition = _eventAttackMelee.target.transform.position - dir * Agent.BlackBoard.weaponRange;

            _moveTime = (_finalPosition - _startPosition).magnitude / 20.0f;
            _rotationTime = angle / 720.0f;
        }
        else
        {
            _finalRotation.SetLookRotation(_eventAttackMelee.attackDir);
            _rotationTime = Vector3.Angle(Agent.Transform.forward, _eventAttackMelee.attackDir) / 720.0f;
            _moveTime = 0;
        }

        _rotationOk = (_rotationTime == 0);
        _positionOK = (_moveTime == 0);
        _currentRotationTime = 0;
        _currentMoveTime = 0;

        _isCritical = IsCritical();
        _knockdown = IsKnockDown();

        _hitTimeStart = false;
    }

    public override void OnUpdate()
    {
        if (_attackStatus == AttackStatus.PREPARING)
        {
            bool dontMove = false;
            if (_rotationOk == false)
            {                
                _currentRotationTime += Time.deltaTime;
                if (_currentRotationTime >= _rotationTime)
                {
                    _currentRotationTime = _rotationTime;
                    _rotationOk = true;
                }
                float progress = _currentRotationTime / _rotationTime;
                Quaternion rotation = Quaternion.Lerp(_startRotation, _finalRotation, progress);
                Agent.Transform.rotation = rotation;                
            }

            if (_positionOK == false)
            {
                _currentMoveTime += Time.deltaTime;
                if (_currentMoveTime >= _moveTime)
                {
                    _currentMoveTime = _moveTime;
                    _positionOK = true;
                }

                if (_currentMoveTime > 0)
                {
                    float progress = _currentMoveTime / _moveTime;
                    Vector3 finalPos = Mathfx.Hermite(_startPosition, _finalPosition, progress);
                    //if (MoveToCollideWithEnemy(finalPos, Transform.forward) == false)
                    if (TransformTools.MoveOnGround(Agent.transform, Agent.CharacterController, 
                        finalPos - Agent.Transform.position, true) == false)
                    {
                        _positionOK = true;
                    }
                }
            }

            if (_rotationOk && _positionOK)
            {
                _attackStatus = AttackStatus.ATTACKING;
                InitializeAttacking();
            }
        }
        else if (_attackStatus == AttackStatus.ATTACKING)
        {
            _currentMoveTime += Time.deltaTime;
            if (_attackPhaseTime < Time.timeSinceLevelLoad)
            {
                //Debug.Log(Time.timeSinceLevelLoad + " attack phase done");
                _eventAttackMelee.attackPhaseDone = true;
                _attackStatus = AttackStatus.FINISHED;
            }

            if (_currentMoveTime >= _moveTime)
                _currentMoveTime = _moveTime;

            if (_currentMoveTime > 0 && _currentMoveTime <= _moveTime)
            {
                float progress = Mathf.Min(1.0f, _currentMoveTime / _moveTime);
                Vector3 finalPos = Mathfx.Hermite(_startPosition, _finalPosition, progress);
                //if (MoveToCollideWithEnemy(finalPos, Transform.forward) == false)
                if (TransformTools.MoveOnGround(Agent.transform, Agent.CharacterController, 
                    finalPos - Agent.Transform.position, false) == false)
                {
                    _currentMoveTime = _moveTime;
                }                
            }

            if (/*_eventAttackMelee.hitTimeStart*/_hitTimeStart == false && _hitTime <= Time.timeSinceLevelLoad)
            {
                //_eventAttackMelee.hitTimeStart = true;
                _hitTimeStart = true;
                HandleAttackResult.DoMeleeDamage(Agent, _eventAttackMelee.target, Agent.BlackBoard.attackerWeapon,
                    _eventAttackMelee.animAttackData, _isCritical, _knockdown, 
                    _eventAttackMelee.animAttackData.isFatal);
            }
        }
        else if (_attackStatus == AttackStatus.FINISHED && _endOfStateTime <= Time.timeSinceLevelLoad)
        {            
            //Debug.Log(Time.timeSinceLevelLoad + " attack finished");
            IsFinished = true;
            _eventAttackMelee.IsFinished = true;
        }
    }    

    bool IsKnockDown()
    {
        return _eventAttackMelee.animAttackData.hitAreaKnockdown && Random.Range(0, 100) < 60 * Agent.BlackBoard.criticalChance;
    }

    bool IsCritical()
    {
        if (/*Agent.IsPlayer && */_eventAttackMelee.animAttackData.hitCriticalType != CriticalHitType.NONE && _eventAttackMelee.target &&
            _eventAttackMelee.target.BlackBoard.criticalAllowed && _eventAttackMelee.target.BlackBoard.IsBlocking == false &&
            _eventAttackMelee.target.BlackBoard.invulnerable == false)
        {
            if (_backHit)
            {
                return true;
            }
            else
            {
                return Random.Range(0, 100) < Agent.BlackBoard.criticalChance * 
                    _eventAttackMelee.animAttackData.criticalModificator * 
                    _eventAttackMelee.target.BlackBoard.criticalHitModifier;
            }
        }
        
        return false;
    }

    void InitializeAttacking()
    {
        Tools.PlayAnimation(Agent.AnimEngine, _eventAttackMelee.animAttackData.animName, 0.2f);
        _hitTime = Time.timeSinceLevelLoad + _eventAttackMelee.animAttackData.hitTime;
        _startPosition = Agent.Transform.position;
        _finalPosition = _startPosition + Agent.Transform.forward * _eventAttackMelee.animAttackData.moveDistance;
        _moveTime = _eventAttackMelee.animAttackData.attackMoveEndTime - _eventAttackMelee.animAttackData.attackMoveStartTime;
        _endOfStateTime = Time.timeSinceLevelLoad + Agent.AnimEngine[_eventAttackMelee.animAttackData.animName].length * 0.9f;

        if (_eventAttackMelee.animAttackData.lastAttackInCombo)
        {
            _attackPhaseTime = _endOfStateTime;
        }
        else
        {
            _attackPhaseTime = Time.timeSinceLevelLoad + _eventAttackMelee.animAttackData.attackEndTime;
        }

        _currentMoveTime = -_eventAttackMelee.animAttackData.attackMoveStartTime;

        if (_eventAttackMelee.target && _eventAttackMelee.target.BlackBoard.IsAlive)
        {
            HandleMontageShot();            
        }
    }

    //void HandleHit()
    //{
    //    AttackMeleeHitData hitData = new AttackMeleeHitData();
    //    hitData.agent = Agent;
    //    hitData.target = _eventAttackMelee.target;
    //    hitData.attackData = _eventAttackMelee.animAttackData;
    //    hitData.isCritical = _isCritical;
    //    hitData.isKnockDown = _knockdown;
    //    onAttackHit.Invoke(hitData);
    //}

    void HandleMontageShot()
    {
        MontageShotData montageData = new MontageShotData();
        montageData.isCritical = _isCritical;
        montageData.isFatalAttack = (_eventAttackMelee.animAttackData.isFatal);
        onMontageShot.Invoke(montageData);
    }
}