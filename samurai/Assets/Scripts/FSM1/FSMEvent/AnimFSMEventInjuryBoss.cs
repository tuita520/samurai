using UnityEngine;
using Phenix.Unity.Collection;

public class AnimFSMEventInjuryBoss : AnimFSMEvent
{
    public Agent1 attacker;
    public WeaponType fromWeapon;
    public DamageType damageType;
    public Vector3 impuls;

    public static Pool<AnimFSMEventInjuryBoss> pool = new Pool<AnimFSMEventInjuryBoss>(10, Reset);

    
    
    public AnimFSMEventInjuryBoss()
    {
        EventCode = (int)AnimFSMEventCode.INJURY_BOSS;
        FSMStateTypeToTransfer = (int)AnimFSMStateType.INJURY_BOSS;
        Reset();
    }

    public override void Reset()
    {
        base.Reset();
        attacker = null;
        fromWeapon = WeaponType.NONE;
        damageType = DamageType.NONE;
        impuls = Vector3.zero;        
    }

    public override void Release()
    {
        pool.Collect(this);
    }
}