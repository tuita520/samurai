using UnityEngine;
public class AnimSetBossOrochi : AnimSet
    protected AnimAttackData AnimAttackX;
    protected AnimAttackData AnimAttackBerserk;
    protected AnimAttackData AnimAttackInjury;
        AnimAttackBerserk = new AnimAttackData("attackO", null, 0, 0.9f, 3.5f, 30, 30, 3.5f, CriticalHitType.NONE, false);
        AnimAttackInjury = new AnimAttackData("attackInjury", null, 0, 0.5f, 1.5f, 30, 180, 4, CriticalHitType.NONE, false);

    public override string GetIdleActionAnim(WeaponType weapon, WeaponState weaponState)
    {
        return "tount";
    }

    public override string GetMoveAnim(MotionType motion, MoveType move, WeaponType weapon, WeaponState weaponState)
        return "walk";

    public override string GetRotateAnim(MotionType motionType, RotationType rotationType)
    {
        if (rotationType == RotationType.LEFT)
            return "rotateL";

        return "rotateR";
    }


    public override AnimAttackData GetFirstAttackAnim(WeaponType weapon, OrderAttackType attackType)
    {
        if (attackType == OrderAttackType.X)
            return AnimAttackX;
        else if (attackType == OrderAttackType.O)
            return AnimAttackInjury;
        //else if (attackType == OrderAttackType.BERSERK)
          //  return AnimAttackBerserk;

        return null;
    }

    public override string GetInjuryPhaseAnim(int phase) {
        string[] s = { "injury1", "injury2", "injury3", "injuryEnd" };

        return s[phase]; 
    }

    public override string GetInjuryAnim(WeaponType weapon, DamageType type) { return "null"; }

    public override string GetDeathAnim(WeaponType weapon, DamageType type)
    {		
        return "death";
    }

    public override string GetKnockdowAnim(KnockdownState state, WeaponType weapon)  { return ""; }