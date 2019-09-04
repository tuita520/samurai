using Phenix.Unity.Collection;

public class AnimFSMEventIdle : AnimFSMEvent
{
    public static Pool<AnimFSMEventIdle> pool = new Pool<AnimFSMEventIdle>(10, Reset);

    public AnimFSMEventIdle()
    {
        EventCode = (int)AnimFSMEventCode.IDLE;
        FSMStateTypeToTransfer = (int)AnimFSMStateType.IDLE;
        Reset();
    }

    public override void Release()
    {
        pool.Collect(this);
    }
}