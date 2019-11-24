using UnityEngine;
using UnityEngine.UI;
using Phenix.Unity.UI;
using DG.Tweening;

public class ViewMainMenu : View
{
    Image _title;               // 游戏名称
    Image _stamp;               // 小印章

    RectTransform _btnEnter;    // 进入游戏按钮
    RectTransform _btnExit;     // 离开游戏按钮
    
    public ViewMainMenu(UIType viewType)
        : base((int)viewType)
    {

    }

    public override void Init()
    {
        _title = UIRoot.transform.Find("Background/Title").GetComponent<Image>();
        _stamp = UIRoot.transform.Find("Background/Stamp").GetComponent<Image>();
        _btnEnter = UIRoot.transform.Find("BtnEnter").GetComponent<RectTransform>();
        _btnExit = UIRoot.transform.Find("BtnExit").GetComponent<RectTransform>();

        _btnEnter.GetComponent<Button>().onClick.AddListener(OnClickEnter);
        _btnExit.GetComponent<Button>().onClick.AddListener(OnClickExit);
    }

    void OnClickEnter()
    {
        MsgEnterSelectStage msg = MsgEnterSelectStage.pool.Get();
        msg.leaveView = this;
        Controllers.SendMsg(msg);
    }

    void OnClickExit()
    {
        Controllers.SendMsg(MsgExitGame.pool.Get());
    }

    public override void Open()
    {
        base.Open();
        UIRoot.SetActive(true);

        Sequence sequence = DOTween.Sequence();        
        sequence.Join(_btnEnter.DOLocalMoveX(300, 1).SetEase(Ease.Flash).From(true));
        sequence.Join(_btnExit.DOLocalMoveX(300, 1).SetEase(Ease.Flash).From(true));

        _title.color = new Color(_title.color.r, _title.color.g, _title.color.b, 0);
        DOTween.ToAlpha(() => _title.color, x => _title.color = x, 1, 8);
        _stamp.color = new Color(_stamp.color.r, _stamp.color.g, _stamp.color.b, 0);
        DOTween.ToAlpha(() => _stamp.color, x => _stamp.color = x, 1, 8);
    }

    public override void Close()
    {
        base.Close();
        UIRoot.SetActive(false);
    }
}
