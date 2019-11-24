using UnityEngine;
using System.Collections.Generic;
using Phenix.Unity.UI;
using Phenix.Unity.Pattern;
using Phenix.Unity.Message;

[System.Serializable]
public class UIPrefabData
{
    public UIType uiType;
    public GameObject uiRootPrefab;
}

public class UIFacadeComponent : Singleton<UIFacadeComponent>
{
    [SerializeField]
    Canvas _canvas;

    UIFacade _facade;

    [SerializeField]
    List<UIPrefabData> _uiRootPrefabs = new List<UIPrefabData>();

    // combo tip sprite列表
    public List<Sprite> comboTipSprites = new List<Sprite>();
    // full combo tip sprite列表
    public List<Sprite> fullComboTipSprites = new List<Sprite>();    
    // 屏幕溅血 sprite列表
    public List<Sprite> screenBloodSprites = new List<Sprite>();
        
    void Start()
    {
        _facade = new UIFacade(_canvas);

        // 注册UI
        foreach (var item in _uiRootPrefabs)
        {
            switch (item.uiType)
            {                
                case UIType.VIEW_SPLASH:
                    _facade.RegisterViewUI(item.uiRootPrefab, null, new ViewSplash(item.uiType));
                    break;
                case UIType.VIEW_MAIN_MENU:
                    _facade.RegisterViewUI(item.uiRootPrefab, null, new ViewMainMenu(item.uiType));
                    break;
                case UIType.VIEW_SELECT_STAGE:
                    _facade.RegisterViewUI(item.uiRootPrefab, new ModelSelectStage(item.uiType), new ViewSelectStage(item.uiType));
                    break;
                case UIType.VIEW_FACE:
                    _facade.RegisterViewUI(item.uiRootPrefab, new ModelFace(item.uiType), new ViewFace(item.uiType));
                    break;
                case UIType.VIEW_LOADING:
                    _facade.RegisterViewUI(item.uiRootPrefab, null, new ViewLoading(item.uiType));
                    break;
                case UIType.VIEW_ARENA:
                    _facade.RegisterViewUI(item.uiRootPrefab, new ModelArena(item.uiType), new ViewArena(item.uiType));       
                    break;                
                default:
                    break;
            }            
        }

        // 注册消息处理
        _facade.RegisterMessage((int)UIMessageType.TARGET_CHANGED, new MessageHandlerTargetChanged());
        _facade.RegisterMessage((int)UIMessageType.SPLASH_COMPLETED, new MessageHandlerSplashCompleted());
        _facade.RegisterMessage((int)UIMessageType.ENTER_SELECT_STAGE, new MessageHandlerEnterSelectStage());
        _facade.RegisterMessage((int)UIMessageType.ENTER_STAGE, new MessageHandlerEnterStage());
        _facade.RegisterMessage((int)UIMessageType.RETURN_MAIN_MENU, new MessageHandlerReturnMainMenu());
        _facade.RegisterMessage((int)UIMessageType.EXIT_GAME, new MessageHandlerExitGame());

        _facade.StartUp((int)UIType.VIEW_SPLASH);
    }

    // Update is called once per frame
    void Update()
    {
        _facade.OnUpdate();
    }

    public void SendMsg(Message msg)
    {
        _facade.SendMsg(msg);
    }
}
