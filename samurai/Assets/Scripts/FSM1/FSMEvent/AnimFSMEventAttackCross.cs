using UnityEngine;
using Phenix.Unity.Collection;

public class AnimFSMEventAttackCross : AnimFSMEvent
{
    public static Pool<AnimFSMEventAttackCross> pool = new Pool<AnimFSMEventAttackCross>(10, Reset);

    public Agent1 target;
    public AnimAttackData animAttackData;
    public Vector3 attackDir;
    

    public override void Release()
    {
        pool.Collect(this);        
    }

    public override void Reset()
    {
        base.Reset();
        target = null;        
        animAttackData = null;
        attackDir = Vector3.zero;
    }
    
    public AnimFSMEventAttackCross()
    {
        EventCode = (int)AnimFSMEventCode.ATTACK_CROSS;
        FSMStateTypeToTransfer = (int)AnimFSMStateType.ATTACK_CROSS;
        Reset();
    }    
}