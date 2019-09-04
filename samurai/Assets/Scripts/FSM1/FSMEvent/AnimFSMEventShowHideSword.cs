using UnityEngine;
using Phenix.Unity.Collection;

public class AnimFSMEventShowHideSword : AnimFSMEvent
{
    public static Pool<AnimFSMEventShowHideSword> pool = new Pool<AnimFSMEventShowHideSword>(10, Reset);

    public bool show = false;

    public AnimFSMEventShowHideSword()
    {
        EventCode = (int)AnimFSMEventCode.SHOW_HIDE_SWORD;
        FSMStateTypeToTransfer = (int)AnimFSMStateType.IDLE;
        Reset();
    }
    
    public override void Release()
    {
        pool.Collect(this);
    }
}