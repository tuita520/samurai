using UnityEngine;
using UnityEngine.UI;
using Phenix.Unity.UI;
using DG.Tweening;
using UnityEngine.SceneManagement;

public class ViewSplash : View
{
    Image _stamp;          // 印章

    public ViewSplash(UIType viewType)
        : base((int)viewType)
    {

    }

    public override void Init()
    {        
        _stamp = UIRoot.transform.Find("Stamp").GetComponent<Image>();        
    }

    public override void Open()
    {
        base.Open();
        UIRoot.SetActive(true);
        Sequence sequence = DOTween.Sequence();
        sequence.Append(_stamp.transform.DOScale(new Vector3(3, 3, 1), 3).From());
        sequence.Join(_stamp.material.DOFade(0, 3).From());
        sequence.AppendInterval(2).OnComplete(() => { SplashCompleted(); });        
    }

    public override void Close()
    {
        base.Close();
        UIRoot.SetActive(false);
    }

    void SplashCompleted()
    {
        Controllers.SendMsg(MsgSplashCompleted.pool.Get());
    }
}
