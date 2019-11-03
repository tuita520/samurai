using Phenix.Unity.Message;
using Phenix.Unity.UI;
using Phenix.Unity.Collection;

public class MsgTargetChanged : Message
{
    public static Pool<MsgTargetChanged> pool = new Pool<MsgTargetChanged>();

    public Agent1 newAgent;

    public MsgTargetChanged()
    {
        msgID = (int)UIMessageType.TARGET_CHANGED;
    }

    public override void Release()
    {
        pool.Collect(this);
    }
}

public class MessageHandlerTargetChanged : MessageHandler
{
    public override void OnMessage(Message msg)
    {
        MsgTargetChanged msgTargetChanged = msg as MsgTargetChanged;
        (Model.Get((int)UIType.ARENA) as ModelArena).Target = msgTargetChanged.newAgent;
        if (msgTargetChanged.newAgent)
        {
            (View.Get((int)UIType.ARENA) as ViewArena).ShowTargetPanel();
        }
        else
        {
            (View.Get((int)UIType.ARENA) as ViewArena).HideTargetPanel();
        }
    }
}