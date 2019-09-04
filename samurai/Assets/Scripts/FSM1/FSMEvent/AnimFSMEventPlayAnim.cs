using Phenix.Unity.Collection;

public class AnimFSMEventPlayAnim : AnimFSMEvent
{
    public static Pool<AnimFSMEventPlayAnim> pool = new Pool<AnimFSMEventPlayAnim>(10, Reset);

    public string animName;
    public bool lookAtTarget;
    public bool invulnerable = false;

    public AnimFSMEventPlayAnim()
    {
        EventCode = (int)AnimFSMEventCode.PLAY_ANIM;
        FSMStateTypeToTransfer = (int)AnimFSMStateType.PLAY_ANIM;
        Reset();
    }

    public override void Reset()
    {
        base.Reset();
        animName = string.Empty;
    }

    public override void Release()
    {
        pool.Collect(this);
    }
}