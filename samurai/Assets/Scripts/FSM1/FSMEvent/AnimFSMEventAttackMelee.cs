using UnityEngine;
using Phenix.Unity.Collection;

public class AnimFSMEventAttackMelee : AnimFSMEvent
{
    public static Pool<AnimFSMEventAttackMelee> pool = new Pool<AnimFSMEventAttackMelee>(10, Reset);

    public Agent1 target;
    public AnimAttackData animAttackData;    
    public Vector3 attackDir;    
    public bool attackPhaseStart;
    public bool attackPhaseDone;

    public override void Release()
    {
        pool.Collect(this);        
    }

    public override void Reset()
    {
        base.Reset();
        target = null;
        attackDir = Vector3.zero;        
        attackPhaseStart = false;
        attackPhaseDone = false;
        animAttackData = null;
    }
    
    public AnimFSMEventAttackMelee()
    {
        EventCode = (int)AnimFSMEventCode.ATTACK_MELEE;
        FSMStateTypeToTransfer = (int)AnimFSMStateType.ATTACK_MELEE;
        Reset();
    }    
}