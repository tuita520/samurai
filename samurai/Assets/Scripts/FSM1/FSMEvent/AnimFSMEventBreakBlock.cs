using Phenix.Unity.Collection;

public class AnimFSMEventBreakBlock : AnimFSMEvent
{
    public static Pool<AnimFSMEventBreakBlock> pool = new Pool<AnimFSMEventBreakBlock>(10, Reset);

    public Agent1 attacker;
    public WeaponType attackerWeapon;
    public bool success;

    public AnimFSMEventBreakBlock()
    {
        EventCode = (int)AnimFSMEventCode.BREAK_BLOCK;
        FSMStateTypeToTransfer = (int)AnimFSMStateType.IDLE;
        Reset();
    }

    public override void Reset()
    {
        base.Reset();
        attacker = null;
        attackerWeapon = WeaponType.NONE;
        success = false;
    }

    public override void Release()
    {
        pool.Collect(this);
    }
}