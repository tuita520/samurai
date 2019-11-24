using Phenix.Unity.Message;
using Phenix.Unity.Scene;
using Phenix.Unity.Collection;
using Phenix.Unity.UI;


public class MsgReturnMainMenu : Message
{
    public static Pool<MsgReturnMainMenu> pool = new Pool<MsgReturnMainMenu>();

    public MsgReturnMainMenu()
    {
        msgID = (int)UIMessageType.RETURN_MAIN_MENU;
    }

    public override void Release()
    {
        pool.Collect(this);
    }
}

public class MessageHandlerReturnMainMenu : MessageHandler
{
    public override void OnMessage(Message msg)
    {
        ViewSelectStage viewSelectStage = View.Get((int)(UIType.VIEW_SELECT_STAGE)) as ViewSelectStage;
        viewSelectStage.Close();
        ViewMainMenu viewMainMenu = View.Get((int)(UIType.VIEW_MAIN_MENU)) as ViewMainMenu;
        SceneMgr.Instance.LoadScene("MainMenu", true, null, null, (() => { viewMainMenu.Open(); }));
    }
}