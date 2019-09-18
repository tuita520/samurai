using UnityEngine;
using System.Collections;


public enum HardwareType
{
    iPhone3G = 0,
    iPhone4G = 1,
    iPad = 2,
    Max = 3,
}

public enum WeaponType 
{
	NONE = -1,
	KATANA = 0,
	BODY,
    BOW,
	MAX,
}

public enum BlockState
{
    NONE = -1,
    START = 0,          // 起式
    HOLD,               // 蓄势
    END,                // 收式
    BLOCK_SUCCESS,      // 格挡成功
    BLOCK_FAIL,         // 格挡失败
}

public enum KnockdownState
{
    None = -1,
    Down = 0,
    Loop,
    Up,
    Fatality,
}

public enum WeaponState
{
    NOT_IN_HANDS,
	IN_HAND,
	//Attacking,
	//Reloading,
	//Empty,
}

public enum OrderAttackType
{
	NONE = -1,
	X = 0,
	O = 1,
    //BOSS_BASH = 2,  // 暂时无用
    //FATALITY = 3,   // 一击必杀（如针对倒地敌人的跳杀）
    //COUNTER = 4,    // 反击
    //BERSERK = 5,    // 暴击
	//MAX = 6,
}
    
public enum AgentType
{
	NONE = -1,
    SWORD_MAN = 0,
    SWORD_MAN_LOW,
    PEASANT,
    PEASANT_LOW,
    DOUBLE_SWORDS_MAN,
    BOW_MAN,    
    MINI_BOSS01,    
    BOSS_OROCHI,
	NPC_MAX,
    PLAYER,
    SAMURAI,
}

public enum GameState
{
	MainMenu,
	IngameMenu,
	Game,
	SaleScreen,
    Tutorial,
    Shop,
    Pause,
}

public enum GameType
{
	SinglePlayer,
    ChapterOnly,
	Survival,
    FirstTimeTutorial,
    Tutorial,
    SaleScreen,
}

public enum GameDifficulty
{
    Easy,
	Normal,
	Hard,
}

public enum DamageResultType
{
    NONE,
    INJURY,
    KNOCK_DOWN,
    DEATH,
}

public enum DamageType
{
    NONE,
    FRONT,
    BACK,
    //BREAK_BLOCK, // 破防
    IN_KNOCK_DOWN,    
}

public enum CriticalHitType
{
    NONE,
    VERTICAL,
	HORIZONTAL,
}

public enum ComboLevel
{
	One = 1,
	Two = 2,
    Three = 3,
    Max = 3
}

public enum ComboLevelPrice
{
    One = 0,
    Two = 1000,
    Three = 2000
}

public enum SwordLevel
{
    One = 1,
    Two = 2,
    Three = 3,
    Four = 4,
    Five = 5,
    Max = 5
}

public enum SwordLevelPrice
{
    One = 0,
    Two = 1000,
    Three = 1500,
    Four = 2000,
    Five = 3000,
}

public enum HealthLevel
{
    One = 1,
    Two = 2,
    Three = 3,
    Max = 3
}

public enum HealtLevelPrice
{
    One = 0,
    Two = 1500,
    Three = 3000,
}

public enum RotationType
{
    LEFT,
    RIGHT
}

public enum MotionType
{
	NONE,
	WALK,
	RUN,
    SPRINT,
    ROLL,
    ATTACK,
    BLOCK,
    //BLOCKING_ATTACK,
    INJURY,    
    DEATH,
    KNOCK_DOWN,
    ANIMATION_DRIVE
}

public enum MoveType
{
    NONE,
    FORWARD,
    BACKWARD,
    LEFTWARD,
    RIGHTWARD,
}

public enum LookType
{
    None,
    TrackTarget,
}


public enum EventTypes
{
    NONE,
    //ENEMYSTEP,
    //ENEMYSEE,
    //ENEMYLOST,
    HIT,
    DEAD,
    //IMINPAIN,
    BLOCK_BROKEN,    // 被人破防
    KNOCKDOWN,
    //FriendInjured,
}


public enum Direction
{
    Forward,
    Backward,
    Left,
    Right,
    Up,
    Down
}

public enum BlockResult
{
    NONE = 0,
    SUCCESS,
    FAIL,
}

public enum ComboType
{
    NONE = 0,
    /*---------------主角招式-------------------*/
    RAISE_WAVE,         // XXXXO 浪翻（快速）
    HALF_MOON,          // OOOXX 半月（破防）
    CLOUD_CUT,          // OOXXX 云切（致命）
    WALKING_DEATH,      // XXOXX 踏死（击倒）
    CRASH_GENERAL,      // OXOOO 破将（重击、群伤）
    FLYING_DRAGON,      // XOOXX 飞龙

    /*---------------普通招式-------------------*/
    SINGLE_SWORD,       // 一刀流（单击）
    MULTI_SWORDS,       // N刀流（N连击）
    JUMP_KILL,          // 跳杀（主角、samurai）
    CROSS,              // 左右交叉（miniboss）
    REVENGE,            // 复仇（boss）

    /*---------------暴击招式-------------------*/
    WHIRL,              // 旋风斩（double sword man）
    COUNTER,            // 反击（swordman）
    ATTACK_ROLL,        // 冲滚（miniboss）
    ATTACK_BERSERK,     // 暴击（boss）
}