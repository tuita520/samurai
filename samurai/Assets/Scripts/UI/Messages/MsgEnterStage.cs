using Phenix.Unity.Message;
using Phenix.Unity.Scene;
using Phenix.Unity.Collection;
using Phenix.Unity.UI;


public class MsgEnterStage : Message
{
    public static Pool<MsgEnterStage> pool = new Pool<MsgEnterStage>();

    public string stageName = string.Empty; // 将要进入的关卡scene名字
    public View leaveView;      // 将要离开的view
    public View enterView;      // 将要进入的view

    public MsgEnterStage()
    {
        msgID = (int)UIMessageType.ENTER_STAGE;
    }

    public override void Release()
    {
        pool.Collect(this);
    }
}

public class MessageHandlerEnterStage : MessageHandler
{
    public override void OnMessage(Message msg)
    {
        MsgEnterStage msgEnterStage = (MsgEnterStage)msg;
        
        msgEnterStage.leaveView.Close();
        ViewLoading viewLoading = View.Get((int)(UIType.VIEW_LOADING)) as ViewLoading;
        viewLoading.Open();
        SceneMgr.Instance.LoadScene(msgEnterStage.stageName, false, viewLoading.OnLoading, viewLoading.OnReady, 
            (() => { viewLoading.Close(); msgEnterStage.enterView.Open(); }));        
    }
}