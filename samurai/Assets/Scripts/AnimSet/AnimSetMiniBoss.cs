using UnityEngine;[System.Serializable]
public class AnimSetMiniBoss : AnimSet{
    protected AnimAttackData AnimAttacksSwordL;
    protected AnimAttackData AnimAttacksSwordS;
    protected AnimAttackData AnimRollAttack;	void Awake()	{        AnimAttacksSwordL = new AnimAttackData("attackA", null, 0.7f, 0.6f, 0.4f, 0.85f, 0.9f, 10, 20, 2.5f, CriticalHitType.NONE, 0, false, false, false, false);
        AnimAttacksSwordS = new AnimAttackData("attackB", null, 0.7f, 0.6f, 0.4f, 0.85f, 0.9f, 10, 20, 2.5f, CriticalHitType.NONE, 0, false, false, false, false);
        AnimRollAttack = new AnimAttackData("", null, 0, 0.5f, 1.5f, 30, 20, 1.5f, CriticalHitType.NONE, false);		Animation anims = GetComponent<Animation>();
        anims["idleSword"].layer = 0;        anims["walkSword"].layer = 0;        
        /*anims["death01"].layer = 2;
        anims["death02"].layer = 2;
        anims["injuryFront01"].layer = 1;
        anims["injuryFront02"].layer = 1;
        anims["injuryFront03"].layer = 1;   
        anims["injuryFront04"].layer = 1;        anims["injuryBack"].layer = 1;*/

        anims["attackRollLoop"].speed = 0.7f;
        anims["attackA"].layer = 0;
  //      anims["attackRoll"].layer = 0;
        anims["idleTount"].speed = 1.5f;

        /*  anims["blockStart"].layer = 0;
         anims["blockLoop"].layer = 0;
         anims["blockEnd"].layer = 0;
         anims["blockFailed"].layer = 0;
         anims["blockHit"].layer = 0;*/
        //  anims["blockStepLeft"].layer = 0;
        //        anims["blockStepRight"].layer = 0;

        /*ComboAttacks.Add(new Combo()
        {
            comboType = ComboType.SINGLE_SWORD,
            comboSteps = new ComboStep[]
            {
                new ComboStep(){orderAttackType = OrderAttackType.NONE, data = new AnimAttackData
                    ("attackA", null, 0.7f, 0.6f, 0.4f, 0.85f, 0.9f, 10, 20, 2.5f, CriticalHitType.NONE,
                    0, false, false, false, false)},                
            }
        });*/

        ComboAttacks.Add(new Combo()
        {
            comboType = ComboType.CROSS,
            comboSteps = new ComboStep[]
            {                
                new ComboStep(){orderAttackType = OrderAttackType.NONE, data = new AnimAttackData
                    ("attackB", null, 1f, 0.25f, 0.1f, 0.45f, 0.45f, 10, 20, 1f, CriticalHitType.NONE,
                    0, false, false, false, false)},
            }
        });

        ComboAttacks.Add(new Combo()
        {
            comboType = ComboType.ATTACK_ROLL,
            comboSteps = new ComboStep[]
            {
                new ComboStep(){orderAttackType = OrderAttackType.NONE, data = new AnimAttackData
                    ("", null, 0, 0.5f, 1.5f, 30, 20, 1.5f, CriticalHitType.NONE, false)}
            }
        });
    }	public override string GetIdleAnim(WeaponType weapon, WeaponState weaponState)	{        return "idleSword";	}

    public override string GetIdleActionAnim(WeaponType weapon, WeaponState weaponState)
    {
        return "idleTount";
    }

    public override string GetMoveAnim(MotionType motion, MoveType move, WeaponType weapon, WeaponState weaponState)    {
        return "walkSword";    }

    public override string GetRotateAnim(MotionType motionType, RotationType rotationType)
    {
        if (motionType == MotionType.BLOCK)
        {
            if (rotationType == RotationType.LEFT)
                return "rotateBlockLeft";

            return "rotateBlockRight";
        }

        if (rotationType == RotationType.LEFT)
            return "rotateLeft";

        return "rotateLeft";//"rotateRight资源动作很奇怪";
    }
    public override string GetRollAnim(WeaponType weapon, WeaponState weaponState){return null;}    public override string GetBlockAnim(BlockState state, WeaponType weapon)    {
        if (state == BlockState.START)
            return "blockStart";
        else if (state == BlockState.HOLD)
            return "blockLoop";
        else if (state == BlockState.BLOCK_FAIL)
            return "blockFailed";
        else if (state == BlockState.BLOCK_SUCCESS)
            return "blockHit";
        else
            return "blockEnd";    }    public override string GetShowWeaponAnim(WeaponType weapon)    {        return "";    }    public override string GetHideWeaponAnim(WeaponType weapon)    {        return "";    }


    public override AnimAttackData GetFirstAttackAnim(WeaponType weapom, OrderAttackType attackType)
    {
        return AnimAttacksSwordL;
    }

    public override AnimAttackData GetRollAttackAnim()
    {
        return AnimRollAttack;
    }

    public override string GetInjuryAnim(WeaponType weapon, DamageType type)
    {
        return "injury";

/*        if(type == E_DamageType.Back)
            return "injuryBack";

        string[] anims = { "injuryFront01", "injuryFront02", "injuryFront03", "injuryFront04" };

        return anims[Random.Range(0, anims.Length)];*/
    }

    public override string GetDeathAnim(WeaponType weapon, DamageType type)
    {
        string[] anims = { "death", "death02"};

        return anims[Random.Range(0, 100) % anims.Length];
    }

    public override string GetKnockdowAnim(KnockdownState state, WeaponType weapon)
    {
        return "";
    }}