// 视图类型
public enum UIType
{
    NONE = 0,
    SPLASH,             // 闪屏
    MAIN_MENU,          // 主菜单
    SELECT_STAGE,       // 选择关卡
    INFORMATION,        // 角色简介
    LOADING,            // 载入
    ARENA,              // 比武擂台
    PAUSE,              // 暂停 
}

// 消息类型
public enum UIMessageType
{
    NONE = 0,
    TARGET_CHANGED,     // 目标改变
}
