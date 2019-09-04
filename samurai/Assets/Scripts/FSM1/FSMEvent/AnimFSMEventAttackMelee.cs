using UnityEngine;
using UnityEngine.Events;
using Phenix.Unity.Collection;

public class AnimFSMEventAttackMelee : AnimFSMEvent
{
    public static Pool<AnimFSMEventAttackMelee> pool = new Pool<AnimFSMEventAttackMelee>(10, Reset);

    public Agent1 target;
    public AnimAttackData animAttackData;
    public AttackType attackType;
    public Vector3 attackDir;
    public bool hitDone;
    public bool attackPhaseDone;

    public override void Release()
    {
        pool.Collect(this);        
    }

    public override void Reset()
    {
        base.Reset();
        target = null;
        hitDone = false;
        attackPhaseDone = false;
        animAttackData = null;
        attackType = AttackType.NONE;        
    }
    
    public AnimFSMEventAttackMelee()
    {
        EventCode = (int)AnimFSMEventCode.ATTACK_MELEE;
        FSMStateTypeToTransfer = (int)AnimFSMStateType.ATTACK_MELEE;
        Reset();
    }    
}