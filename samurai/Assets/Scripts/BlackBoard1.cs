using UnityEngine;

[System.Serializable]
public class BlackBoard1
{
    public Agent1 Agent { get; set; }

    public float GOAPMaxWeightRetreat = 0.7f;
    public float GOAPMaxWeightPress = 0.8f;
    public float GOAPMaxWeightAvoidLeft = 0.85f;
    public float GOAPMaxWeightAvoidRight = 0.85f;
    public float GOAPMaxWeightAttackTarget = 0.9f;        
    public float GOAPMaxWeightReactToDamage = 1f;
    public float GOAPMaxWeightAlert = 0.7f;
    public float GOAPMaxWeightCalm = 0.9f;
    public float GOAPMaxWeightShow = 0.3f;
    public float GOAPMaxWeightOrderAttack = 0.9f;
    public float GOAPMaxWeightOrderDodge = 0.9f;
    public float GOAPMaxWeightOrderMove = 0.9f;
    public float GOAPMaxWeightIdle = 0.1f;
    public float GOAPMaxWeightBlock = 0.8f;

    //public float GOAP_ReactToDamageRelevancy = 1.0f;
    //public float GOAP_INJURY = 0.9f;
    //public float GOAP_KNOCKDOWN = 0.95f;
    //public float GOAP_DEATH = 1f;
        
    float _block = 0;
    public float minBlock = 0;
    public float maxBlock = 100;
    public float blockModificator = 0;
    public float blockAttackModificator = 0;
    public float blockInjuryModificator = 0;
    public float Block
    {
        get
        {
            return _block;
        }

        set
        {
            _block = value;
            if (_block < minBlock)
            {
                _block = minBlock;
            }
            else if (_block > maxBlock)
            {
                _block = maxBlock;
            }
        }
    }
        
    float _berserk = 0;
    public float minBerserk = 0;
    public float maxBerserk = 100;
    public float berserkModificator = 0;
    public float berserkBlockModificator = 0;
    public float berserkInjuryModificator = 0;
    public float berserkAttackModificator = 0;
    public float Berserk
    {
        get
        {
            return _berserk;
        }

        set
        {
            _berserk = value;
            if (_berserk < minBerserk)
            {
                _berserk = minBerserk;
            }
            else if (_berserk > maxBerserk)
            {
                _berserk = maxBerserk;
            }
        }
    }
        
    float _rage = 0;
    public float minRage;
    public float maxRage;
    public float rageModificator = 10;
    public float rageInjuryModificator = 10;
    public float rageBlockModificator = 0;
    public float Rage
    {
        get
        {
            return _rage;
        }

        set
        {
            _rage = value;
            if (_rage < minRage)
            {
                _rage = minRage;
            }
            else if (_rage > maxRage)
            {
                _rage = maxRage;
            }
        }
    }
        
    float _fear = 0;
    public float minFear;
    public float maxFear;
    public float fearModificator = 10;
    public float fearInjuryModificator = 5;
    public float fearBlockModificator = 0;
    public float fearAttackModificator = 20;    
    public float Fear
    {
        get
        {
            return _fear;
        }

        set
        {
            _fear = value;
            if (_fear < minFear)
            {
                _fear = minFear;
            }
            else if (_fear > maxFear)
            {
                _fear = maxFear;
            }
        }
    }


    [System.NonSerialized]
    public MotionType motionType = MotionType.NONE;                       // 动作类型    
    [System.NonSerialized]
    public WeaponState weaponState;                                       // 武器在手状态  
    public WeaponState weaponStateOnAwake = WeaponState.NOT_IN_HANDS;
    public WeaponType weaponSelected = WeaponType.KATANA;                 // 武器类型    
    public float weaponRange = 2;
    public bool allowedFlashToWeaponRange = false;
    public bool showWeaponTrail = false;
    public float SqrWeaponRange { get { return weaponRange * weaponRange; } }
    public float combatRange = 4;
    public float SqrCombatRange { get { return combatRange * combatRange; } }
    public float safeRange = 100;

    public float attackRollWeaponRange = 2f;

    public float attackCD = 2;
    [System.NonSerialized]
    public float nextAttackTimer = 0;

    public float maxSprintSpeed = 8;
    public float maxRunSpeed = 4;
    public float maxWalkSpeed = 1.5f;
    public float maxCombatMoveSpeed = 1;
    public float maxWhirlMoveSpeed = 0;
    //public float maxKnockDownLyingTime = 4;

    public float speedSmooth = 2.0f;
    public float rotationSmooth = 20.0f;
    public float rotationSmoothInMove = 8.0f;
    public float rollDistance = 4.0f;
    public float moveSpeedModifier = 1;
    public float attackRollSpeed = 15;

    [System.NonSerialized]
    public bool inIdle = false;    

    [System.NonSerialized]
    public float speed = 0;
    [System.NonSerialized]
    public Vector3 moveDir;
    [System.NonSerialized]
    public MoveType moveType;

    [System.NonSerialized]
    public float health = 100;
    public float maxHealth = 100;
        
    [System.NonSerialized]
    public Vector3 desiredDirection;
    [System.NonSerialized]
    public Agent1 desiredTarget;

    Agent1 _attacker = null;        
    public Agent1 Attacker
    {
        get { return _attacker; }
        set
        {
            if (_attacker != value)
            {                
                _attackerRepeatCount = 0;
            }
            else
            {
                ++_attackerRepeatCount;
            }
            _attacker = value;
        }
    }

    int _attackerRepeatCount = 0;
    public int AttackerRepeatCount
    {
        get { return _attackerRepeatCount; }
    }

    [System.NonSerialized]
    public WeaponType attackerWeapon;
    [System.NonSerialized]
    public DamageType damageType;

    [System.NonSerialized]
    public DamageResultType damageResultType;
    [System.NonSerialized]
    public Vector3 impuls;

    // Damage settings        
    [System.NonSerialized]
    public bool invulnerable = false;
    public bool criticalAllowed = true;    
    public float criticalHitModifier = 1;
    public float criticalChance = 18;
    public bool damageOnlyFromBack = false;
    public bool knockDownAllowed = false;
    public bool knockDownDamageDeadly = false;
    //[System.NonSerialized]
    //public BlockResult blockResult = BlockResult.NONE;
    public float breakBlockChance = 20;

    public float knockDownLyingTime = 3;
    public float blockHoldTime = 5;

    public float attackRollHitTime = 0.2f;
    public float attackWhirlHitTime = 0.5f;

    public bool IsBlocking { get { return motionType == MotionType.BLOCK/* || motionType == MotionType.BLOCKING_ATTACK*/; } }
    public bool IsAlive { get { return health > 0 && Agent.gameObject.activeSelf; } }
    public bool IsKnockedDown { get { return motionType == MotionType.KNOCK_DOWN && knockDownDamageDeadly; } }
    public bool DesiredTargetIsKnockedDown
    {
        get
        {
            return HasAttackTarget && desiredTarget.BlackBoard.IsKnockedDown;
        }
    }

    public bool InAttackCD { get { return Time.timeSinceLevelLoad < nextAttackTimer; } }

    // 目标与我距离
    public float DistanceToDesiredTarget
    {
        get
        {
            if (desiredTarget != null)
            {
                Vector3 vec = desiredTarget.Position - Agent.Transform.position;
                return vec.magnitude;
            }
            else
            {
                return Mathf.Infinity;
            }
        }
    }

    public float ForwardAngleToDesiredTarget
    {
        get
        {
            return Phenix.Unity.Utilities.TransformTools.ForwardAngleToTarget(Agent.Transform, desiredTarget.Transform);            
        }
    }

    public bool HasAttackTarget
    {
        get { return desiredTarget != null && desiredTarget.BlackBoard.IsAlive; }
    }

    public bool WeaponInHand
    {
        get { return weaponState == WeaponState.IN_HAND; }
    }

    public bool DesiredTargetInRange(float range)
    {
        if (desiredTarget != null)
        {
            return DistanceToDesiredTarget < range;
        }
        else
        {
            return false;
        }
    }

    // 目标是否在武器范围内
    public bool DesiredTargetInWeaponRange
    {
        get
        {            
            return DesiredTargetInRange(weaponRange);
        }
    }

    // 目标是否在战斗范围内
    public bool DesiredTargetInCombatRange
    {
        get
        {
            return DesiredTargetInRange(combatRange);
        }
    }

    public bool LookAtDesiredTarget
    {        
        get
        {
            if (desiredTarget == null)
            {
                return false;
            }
            return Phenix.Unity.Utilities.TransformTools.IsLookingAtTarget(Agent.Transform, desiredTarget.Transform);
        }
    }

    // target朝向相对self的角度
    float AngleToDesiredTargetForward
    {        
        get
        {
            return Phenix.Unity.Utilities.TransformTools.AngleToTargetForward(Agent.Transform, desiredTarget.Transform);
        }
    }

    // 目标是否面向自己（和自己的朝向无关）
    public bool AheadOfDesiredTarget
    {
        get
        {
            return HasAttackTarget &&
                Phenix.Unity.Utilities.TransformTools.IsAheadOfTarget(Agent.Transform, desiredTarget.Transform);            
        }
    }

    // 目标是否背对自己（和自己的朝向无关）
    public bool BehindDesiredTarget
    {
        get
        {
            return HasAttackTarget && 
                Phenix.Unity.Utilities.TransformTools.IsBehindTarget(Agent.Transform, desiredTarget.Transform);
        }
    }

    public bool InAttackMotion
    {
        get
        {
            return motionType == MotionType.ATTACK;
        }
    }

    public bool InRollMotion
    {
        get
        {
            return motionType == MotionType.ROLL;
        }
    }

    /// <summary>
    /// 是否A脸朝着B背
    /// </summary>
    public bool FaceToOtherBack(Agent1 other)
    {
        return Vector3.Angle(Agent.Forward, other.Forward) <= 45;
    }
        

    public void Reset()
    {
        motionType = MotionType.NONE;
        weaponState = weaponStateOnAwake;
        
        speed = 0;
        moveDir = Vector3.zero;

        health = maxHealth;

        _rage = minRage;
        _block = minBlock;
        _fear = minFear;

        desiredDirection = Vector3.zero;
        desiredTarget = null;
        _attacker = null;
        _attackerRepeatCount = 0;

        nextAttackTimer = 0;
    }
}
