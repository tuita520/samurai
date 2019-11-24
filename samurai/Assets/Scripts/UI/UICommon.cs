// 视图类型
public enum UIType
{
    NONE = 0,
    VIEW_SPLASH,             // 闪屏
    VIEW_MAIN_MENU,          // 主菜单
    VIEW_SELECT_STAGE,       // 选择关卡    
    VIEW_FACE,               // 人物志
    VIEW_LOADING,            // 载入
    VIEW_ARENA,              // 沙场     

    FACE_HEAD,               // 人物志中头像UI的prefab   
}

// 消息类型
public enum UIMessageType
{
    NONE = 0,
    TARGET_CHANGED,     // 目标改变
    SPLASH_COMPLETED,   // SPLASH完成
    ENTER_SELECT_STAGE, // 进入选关界面
    ENTER_STAGE,        // 进入关卡
    RETURN_MAIN_MENU,   // 返回主界面
    EXIT_GAME,          // 离开游戏    
}
