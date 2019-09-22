using UnityEngine;
using Phenix.Unity.Collection;

public class AnimFSMEventAttackBow : AnimFSMEvent
{
    public static Pool<AnimFSMEventAttackBow> pool = new Pool<AnimFSMEventAttackBow>(10, Reset);

    public Agent1 target;
    public AnimAttackData animAttackData;    
    public Vector3 attackDir;   
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
        attackPhaseDone = false;
        animAttackData = null;
    }
    
    public AnimFSMEventAttackBow()
    {
        EventCode = (int)AnimFSMEventCode.ATTACK_BOW;
        FSMStateTypeToTransfer = (int)AnimFSMStateType.ATTACK_BOW;
        Reset();
    }    
}