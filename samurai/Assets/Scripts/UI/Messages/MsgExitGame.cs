using Phenix.Unity.Message;
using Phenix.Unity.Scene;
using Phenix.Unity.Collection;
using Phenix.Unity.UI;
using UnityEngine;

public class MsgExitGame : Message
{
    public static Pool<MsgExitGame> pool = new Pool<MsgExitGame>();

    public MsgExitGame()
    {
        msgID = (int)UIMessageType.EXIT_GAME;
    }

    public override void Release()
    {
        pool.Collect(this);
    }
}

public class MessageHandlerExitGame : MessageHandler
{
    public override void OnMessage(Message msg)
    {
        Application.Quit();
    }    
}