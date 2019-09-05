using UnityEngine;[System.Serializable]
public class AnimSetEnemyArcher : AnimSet{
    protected AnimAttackData AnimAttacksBow;
	void Awake()	{        AnimAttacksBow = new AnimAttackData("bowFire", null, -1, 2.95f, 3.0f, 10, 20, 1, CriticalHitType.NONE, false);		Animation anims = GetComponent<Animation>();
        anims["idle"].layer = 0;
        anims["idleBow"].layer = 0;        anims["walk"].layer = 0;        anims["combatMoveF"].layer = 0;
        anims["combatMoveB"].layer = 0;
        anims["combatMoveR"].layer = 0;
        anims["combatMoveL"].layer = 0;
        anims["rotateLeft"].layer = 0;
        anims["rotateRight"].layer = 0;
        
        anims["death01"].layer = 2;
        anims["death02"].layer = 2;
        anims["injuryFront"].layer = 1;
        anims["injuryBack"].layer = 1;

        anims["bowFire"].layer = 0;
         //anims["showBow"].layer = 0;        //anims["hideSword"].layer = 1;

        anims["knockdown"].layer = 0;
        anims["knockdownLoop"].layer = 0;
        anims["knockdownUp"].layer = 0;
        anims["knockdownDeath"].layer = 0;
	}	public override string GetIdleAnim(WeaponType weapon, WeaponState weaponState)	{        if (weaponState == WeaponState.NOT_IN_HANDS)            return "idle";        return "idleBow";	}

    public override string GetIdleActionAnim(WeaponType weapon, WeaponState weaponState)
    {
        Debug.LogError("unssupported !!");

        if(weapon == WeaponType.KATANA)
            return "idleTount";

        return "idle";
    }

    public override string GetMoveAnim(MotionType motion, MoveType move, WeaponType weapon, WeaponState weaponState)    {        if (weaponState == WeaponState.NOT_IN_HANDS)        {            return "walk";        }        else         {
            if (move == MoveType.FORWARD)
                return "combatMoveF";
            else if (move == MoveType.BACKWARD)
                return "combatMoveB";
            else if (move == MoveType.RIGHTWARD)
                return "combatMoveR";
            else
                return "combatMoveL";
        }
    }

    public override string GetRotateAnim(MotionType motionType, RotationType rotationType)
    {
        if (rotationType == RotationType.LEFT)
            return "rotateLeft";

        return "rotateRight";
    }
    public override string GetRollAnim(WeaponType weapon, WeaponState weaponState){return null;}    public override string GetBlockAnim(BlockState state, WeaponType weapon)    {
        return "idle";    }    public override string GetShowWeaponAnim(WeaponType weapon)    {        return "idleBow";    }    public override string GetHideWeaponAnim(WeaponType weapon)    {        return "idle";    }


    public override AnimAttackData GetFirstAttackAnim(WeaponType weapom, OrderAttackType attackType)
    {
        return AnimAttacksBow;
    }


    public override string GetInjuryAnim(WeaponType weapon, DamageType type)
    {

        if(type == DamageType.BACK)
            return "injuryBack";

        string[] anims = { "injuryFront",};

        return anims[Random.Range(0, anims.Length)];
    }

    public override string GetDeathAnim(WeaponType weapon, DamageType type)
    {
        string[] anims = { "death01", "death02"};

        return anims[Random.Range(0, 100) % anims.Length];
    }


    public override string GetKnockdowAnim(KnockdownState state, WeaponType weapon)
    {
        switch (state)
        {
            case KnockdownState.Down:
                return "knockdown";
            case KnockdownState.Loop:
                return "knockdownLoop";
            case KnockdownState.Up:
                return "knockdownUp";
            case KnockdownState.Fatality:
                return "knockdownDeath";
            default:
                return "";
        }
    }}