using UnityEngine;
using UnityEngine.UI;
using Phenix.Unity.UI;
using Phenix.Unity.Scene;
using DG.Tweening;

public class ViewLoading : View
{
    Text _percent;            // 进度 %
    Text _tip;                // 技巧提示    
    Text _poetry;             // 题诗

    Slider _slider;

    float _curProgress = 0;
    float _tarProgress = 0;
    float _speed = 0.4f;

    public ViewLoading(UIType viewType)
        : base((int)viewType)
    {

    }

    public override void Init()
    {
        _poetry = UIRoot.transform.Find("Poetry").GetComponent<Text>();
        _tip = UIRoot.transform.Find("Tip").GetComponent<Text>();
        _percent = UIRoot.transform.Find("Percent").GetComponent<Text>();

        _slider = UIRoot.transform.Find("ProgressBackground").GetComponent<Slider>();
    }

    public override void Open()
    {
        base.Open();
        UIRoot.SetActive(true);

        string str = _poetry.text;
        _poetry.text = string.Empty;
        _poetry.DOText(str, 10).SetDelay(0.5f).SetEase(Ease.Linear);
    }

    public override void Close()
    {
        base.Close();
        UIRoot.SetActive(false);
    }

    public void OnLoading(float progress)
    {
        _tarProgress = progress;        
    }

    public void OnReady()
    {
        _tarProgress = 1;
    }

    public override void OnUpdate()
    {
        _curProgress = Mathf.Lerp(_curProgress, _tarProgress, Time.deltaTime * _speed);
        _percent.text = (int)(_curProgress * 100)+ "%";
        _slider.value = _curProgress;
        if (_curProgress > 0.99f)
        {
            _slider.value = 1;
            _percent.text = "100%";
            SceneMgr.Instance.ActivateSceneOnReady();
        }
    }
}
