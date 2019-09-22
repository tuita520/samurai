using UnityEngine.Events;
using Phenix.Unity.AI;

public enum AnimFSMEventCode
{
    NONE = -1,
    IDLE,
    GOTO,
    MOVE,
    ROLL,
    ATTACK_MELEE,
    ATTACK_WHIRL,
    COMBAT_MOVE,
    ROTATE,
    SHOW_HIDE_SWORD,
    INJURY,
    KNOCK_DOWN,
    DEATH,
    PLAY_ANIM,
    BLOCK,
    BREAK_BLOCK,
    ATTACK_ROLL,
    ATTACK_CROSS,
    ATTACK_BOW,
    FLASH,
    INJURY_BOSS,
    MOVE_ROTATE,
    COUNT,
}

public class AnimFSMEvent : FSMEvent
{
    public AnimFSMEvent() { }
    
    public virtual void Reset()
    {
        IsFinished = false;
    }

    public static void Reset(AnimFSMEvent inst) { inst.Reset(); }
    public override void Release() { }    
}