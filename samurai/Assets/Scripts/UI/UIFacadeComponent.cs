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
        
    protected override void Awake()
    {
        base.Awake();

        _facade = new UIFacade(_canvas);

        // 注册UI
        foreach (var item in _uiRootPrefabs)
        {
            switch (item.uiType)
            {                
                case UIType.SPLASH:
                    break;
                case UIType.MAIN_MENU:
                    break;
                case UIType.SELECT_STAGE:
                    break;
                case UIType.INFORMATION:
                    break;
                case UIType.LOADING:
                    break;
                case UIType.ARENA:
                    _facade.RegisterUI(item.uiRootPrefab, new ModelArena(item.uiType), new ViewArena(item.uiType));                    
                    break;
                case UIType.PAUSE:
                    break;
                default:
                    break;
            }            
        }

        // 注册消息处理
        _facade.RegisterMessage((int)UIMessageType.TARGET_CHANGED, new MessageHandlerTargetChanged());

        _facade.StartUp((int)UIType.ARENA);
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
