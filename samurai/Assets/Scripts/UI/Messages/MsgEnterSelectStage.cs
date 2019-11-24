using Phenix.Unity.Message;
using Phenix.Unity.Scene;
using Phenix.Unity.Collection;
using Phenix.Unity.UI;


public class MsgEnterSelectStage : Message
{
    public static Pool<MsgEnterSelectStage> pool = new Pool<MsgEnterSelectStage>();

    public View leaveView;     // 将离开的view

    public MsgEnterSelectStage()
    {
        msgID = (int)UIMessageType.ENTER_SELECT_STAGE;
    }

    public override void Release()
    {
        pool.Collect(this);
    }
}

public class MessageHandlerEnterSelectStage : MessageHandler
{
    public override void OnMessage(Message msg)
    {
        MsgEnterSelectStage msgEnterSelectStage = (MsgEnterSelectStage)msg;        
        msgEnterSelectStage.leaveView.Close();
        ViewSelectStage viewSelectStage = View.Get((int)(UIType.VIEW_SELECT_STAGE)) as ViewSelectStage;
        SceneMgr.Instance.LoadScene("SelectStage", true, null, null, (() => { viewSelectStage.Open(); }));
    }
}