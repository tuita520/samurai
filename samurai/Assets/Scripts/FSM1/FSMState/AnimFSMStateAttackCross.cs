using UnityEngine;
using Phenix.Unity.Utilities;
using Phenix.Unity.AI;

public class AnimFSMStateAttackCross : AnimFSMState
{
    enum AttackStatus
    {
        PREPARING,
        ATTACKING,
        FINISHED,
    }

    AnimFSMEventAttackCross _eventAttackCross;
        
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
    bool _hitTimeStart = false;
    float _attackPhaseTime;

    bool _rotationOk = false;
    bool _positionOK = false;

    bool _isCritical = false;
    bool _knockdown = false;
    bool _backHit = false;

    int _remainAttackCount = 0;

    AttackStatus _attackStatus;

    public AnimFSMStateAttackCross(Agent1 agent)
        : base((int)AnimFSMStateType.ATTACK_CROSS, agent)
    {

    }
    public override void OnExit()
    {
        Agent.BlackBoard.speed = 0;
    }

    protected override void Initialize(FSMEvent ev = null)
    {
        _eventAttackCross = ev as AnimFSMEventAttackCross;

        _attackStatus = AttackStatus.PREPARING;
        Agent.BlackBoard.motionType = MotionType.ATTACK;
        _startRotation = Agent.Transform.rotation;
        _startPosition = Agent.Transform.position;

        if (_eventAttackCross.target != null)
        {
            float angle = 0;
            float distance = 0;
            Vector3 dir = _eventAttackCross.target.Position - Agent.Transform.position;
            distance = dir.magnitude;
            if (distance > 0.1f)
            {
                dir.Normalize();
                angle = Vector3.Angle(Agent.Transform.forward, dir);
                if (angle < 40 && Vector3.Angle(Agent.Forward, _eventAttackCross.target.Forward) < 80)
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
                _finalPosition = _eventAttackCross.target.transform.position - dir * Agent.BlackBoard.weaponRange;

            _moveTime = (_finalPosition - _startPosition).magnitude / 20.0f;
            _rotationTime = angle / 720.0f;
        }
        else
        {
            _finalRotation.SetLookRotation(_eventAttackCross.attackDir);
            _rotationTime = Vector3.Angle(Agent.Transform.forward, _eventAttackCross.attackDir) / 720.0f;
            _moveTime = 0;
        }

        _rotationOk = (_rotationTime == 0);
        _positionOK = (_moveTime == 0);
        _currentRotationTime = 0;
        _currentMoveTime = 0;

        _isCritical = IsCritical();
        _knockdown = IsKnockDown();

        _remainAttackCount = 2;
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

            if (_hitTimeStart == false && _hitTime <= Time.timeSinceLevelLoad)
            {
                _hitTimeStart = true;
                HandleAttackResult.DoMeleeDamage(Agent, _eventAttackCross.target, Agent.BlackBoard.attackerWeapon,
                    _eventAttackCross.animAttackData, _isCritical, _knockdown,
                    _eventAttackCross.animAttackData.isFatal);
            }

            if (_attackPhaseTime < Time.timeSinceLevelLoad)
            {
                if (--_remainAttackCount > 0)
                {
                    // 再次攻击
                    InitializeAttacking(false);
                }
                else
                {
                    _attackStatus = AttackStatus.FINISHED;
                }
            }
        }
        else if (_attackStatus == AttackStatus.FINISHED && _endOfStateTime <= Time.timeSinceLevelLoad)
        {
            //Debug.Log(Time.timeSinceLevelLoad + " attack finished");
            IsFinished = true;
            _eventAttackCross.IsFinished = true;
        }
    }

    bool IsKnockDown()
    {
        return _eventAttackCross.animAttackData.hitAreaKnockdown && Random.Range(0, 100) < 60 * Agent.BlackBoard.criticalChance;
    }

    bool IsCritical()
    {
        if (/*Agent.IsPlayer && */_eventAttackCross.animAttackData.hitCriticalType != CriticalHitType.NONE &&
            _eventAttackCross.target && _eventAttackCross.target.BlackBoard.criticalAllowed && 
            _eventAttackCross.target.BlackBoard.IsBlocking == false &&
            _eventAttackCross.target.BlackBoard.invulnerable == false)
        {
            if (_backHit)
            {
                return true;
            }
            else
            {
                return Random.Range(0, 100) < Agent.BlackBoard.criticalChance *
                    _eventAttackCross.animAttackData.criticalModificator *
                    _eventAttackCross.target.BlackBoard.criticalHitModifier;
            }
        }

        return false;
    }

    void InitializeAttacking(bool playAnim = true)
    {
        if (playAnim)
        {
            AnimationTools.PlayAnim(Agent.AnimEngine, _eventAttackCross.animAttackData.animName, 0.2f);
        }
        
        _hitTime = Time.timeSinceLevelLoad + _eventAttackCross.animAttackData.hitTime;
        _hitTimeStart = false;
        _startPosition = Agent.Transform.position;
        _finalPosition = _startPosition + Agent.Transform.forward * _eventAttackCross.animAttackData.moveDistance;
        _moveTime = _eventAttackCross.animAttackData.attackMoveEndTime - 
            _eventAttackCross.animAttackData.attackMoveStartTime;
        _endOfStateTime = Time.timeSinceLevelLoad + 
            Agent.AnimEngine[_eventAttackCross.animAttackData.animName].length * 0.9f;

        if (_eventAttackCross.animAttackData.lastAttackInCombo)
        {
            _attackPhaseTime = _endOfStateTime;
        }
        else
        {
            _attackPhaseTime = Time.timeSinceLevelLoad + _eventAttackCross.animAttackData.attackEndTime;
        }

        _currentMoveTime = -_eventAttackCross.animAttackData.attackMoveStartTime;

        if (_eventAttackCross.target && _eventAttackCross.target.BlackBoard.IsAlive)
        {
            HandleCamera.SlowMotion(_isCritical, _eventAttackCross.animAttackData.isFatal);
        }
    }

    //void HandleHit()
    //{
    //    AttackMeleeHitData hitData = new AttackMeleeHitData();
    //    hitData.agent = Agent;
    //    hitData.target = _eventAttackCross.target;
    //    hitData.attackData = _eventAttackCross.animAttackData;
    //    hitData.isCritical = _isCritical;
    //    hitData.isKnockDown = _knockdown;
    //    onAttackHit.Invoke(hitData);
    //}

}