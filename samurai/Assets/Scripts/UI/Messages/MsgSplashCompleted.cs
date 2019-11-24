using Phenix.Unity.Message;
using Phenix.Unity.Scene;
using Phenix.Unity.Collection;
using Phenix.Unity.UI;


public class MsgSplashCompleted : Message
{
    public static Pool<MsgSplashCompleted> pool = new Pool<MsgSplashCompleted>();
       
    public MsgSplashCompleted()
    {
        msgID = (int)UIMessageType.SPLASH_COMPLETED;
    }

    public override void Release()
    {
        pool.Collect(this);
    }
}

public class MessageHandlerSplashCompleted : MessageHandler
{
    public override void OnMessage(Message msg)
    {
        MsgSplashCompleted msgSplashCompleted = msg as MsgSplashCompleted;

        SceneMgr.Instance.LoadScene("MainMenu", true, null, null, OnCompleted);
    }

    void OnCompleted()
    {
        View.Get((int)(UIType.VIEW_SPLASH)).Close();
        View.Get((int)(UIType.VIEW_MAIN_MENU)).Open();
        UnityEngine.Debug.Log(UnityEngine.SceneManagement.SceneManager.sceneCount);
    }
}