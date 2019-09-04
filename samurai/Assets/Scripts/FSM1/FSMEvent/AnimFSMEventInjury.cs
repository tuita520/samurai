using UnityEngine;
using Phenix.Unity.Collection;

public class AnimFSMEventInjury : AnimFSMEvent
{
    public Agent1 attacker;
    public WeaponType fromWeapon;
    public DamageType damageType;
    public Vector3 impuls;

    public static Pool<AnimFSMEventInjury> pool = new Pool<AnimFSMEventInjury>(10, Reset);

    
    
    public AnimFSMEventInjury()
    {
        EventCode = (int)AnimFSMEventCode.INJURY;
        FSMStateTypeToTransfer = (int)AnimFSMStateType.INJURY;
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