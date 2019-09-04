using UnityEngine;
using Phenix.Unity.Collection;

public class AnimFSMEventBlock : AnimFSMEvent
{
    public static Pool<AnimFSMEventBlock> pool = new Pool<AnimFSMEventBlock>(10, Reset);

    public Agent1 attacker;
    public WeaponType fromWeapon;
    public float holdTime;

    public AnimFSMEventBlock()
    {
        EventCode = (int)AnimFSMEventCode.BLOCK;
        FSMStateTypeToTransfer = (int)AnimFSMStateType.BLOCK;
        Reset();
    }

    public override void Reset()
    {
        base.Reset();
        attacker = null;
        fromWeapon = WeaponType.NONE;
        holdTime = 0;
}

    public override void Release()
    {
        pool.Collect(this);
    }
}