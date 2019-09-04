using UnityEngine;
using Phenix.Unity.Collection;

public class AnimFSMEventKnockDown : AnimFSMEvent
{
    public Agent1 attacker;
    public WeaponType fromWeapon;
    public Vector3 impuls;
    public float lyingTime; // 倒在地上的时间

    public static Pool<AnimFSMEventKnockDown> pool = new Pool<AnimFSMEventKnockDown>(10, Reset);
           
    public AnimFSMEventKnockDown()
    {
        EventCode = (int)AnimFSMEventCode.KNOCK_DOWN;
        FSMStateTypeToTransfer = (int)AnimFSMStateType.KNOCK_DOWN;
        Reset();
    }

    public override void Reset()
    {
        base.Reset();
        attacker = null;
        fromWeapon = WeaponType.NONE;        
        impuls = Vector3.zero;
        lyingTime = 0;
    }

    public override void Release()
    {
        pool.Collect(this);
    }
}