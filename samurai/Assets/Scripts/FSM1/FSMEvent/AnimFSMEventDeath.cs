using UnityEngine;
using Phenix.Unity.Collection;

public class AnimFSMEventDeath : AnimFSMEvent
{
    public static Pool<AnimFSMEventDeath> pool = new Pool<AnimFSMEventDeath>(10, Reset);

    public Agent1 attacker;
    public WeaponType fromWeapon;
    public DamageType damageType;
    public Vector3 impuls;    

    public AnimFSMEventDeath()
    {
        EventCode = (int)AnimFSMEventCode.DEATH;
        FSMStateTypeToTransfer = (int)AnimFSMStateType.DEATH;
        Reset();
    }

    public override void Reset()
    {
        base.Reset();
        attacker = null;
        fromWeapon = WeaponType.NONE;
        impuls = Vector3.zero;        
    }

    public override void Release()
    {
        pool.Collect(this);
    }
}