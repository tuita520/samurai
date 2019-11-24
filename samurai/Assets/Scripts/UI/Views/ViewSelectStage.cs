using UnityEngine;
using UnityEngine.UI;
using Phenix.Unity.UI;
using Phenix.Unity.Scene;
using DG.Tweening;

public class ViewSelectStage : View
{
    Button _btnReturn;
    Button _btnEnterStage;
    Button _btnFace;

    public ViewSelectStage(UIType viewType)
        : base((int)viewType)
    {

    }

    public override void Init()
    {
        _btnReturn = UIRoot.transform.Find("Stages/Buttons/BtnReturn").GetComponent<Button>();
        _btnEnterStage = UIRoot.transform.Find("Stages/Buttons/BtnEnterStage").GetComponent<Button>();
        _btnFace = UIRoot.transform.Find("Stages/Buttons/BtnFace").GetComponent<Button>();

        _btnReturn.onClick.AddListener(() => { Controllers.SendMsg(MsgReturnMainMenu.pool.Get()); });
        _btnEnterStage.onClick.AddListener(() => {
            MsgEnterStage msg = MsgEnterStage.pool.Get();
            msg.leaveView = this;
            msg.enterView = View.Get((int)UIType.VIEW_ARENA);
            msg.stageName = "Arena";
            Controllers.SendMsg(msg);
        });
        _btnFace.onClick.AddListener(() => {
            View.Get((int)UIType.VIEW_FACE).Open();
        });
    }

    public override void Open()
    {
        base.Open();
        UIRoot.SetActive(true);

    }

    public override void Close()
    {
        base.Close();
        UIRoot.SetActive(false);
    }  

    public override void OnUpdate()
    {
     
    }
}
