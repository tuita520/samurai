using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Phenix.Unity.UI;

public class ViewArena : View
{
    Button _btnPause;           // 暂停按钮

    Image _headPlayer;          // 玩家头像
    Slider _HPPlayer;           // 玩家血条

    Image _headTarget;           // 当前敌人头像
    Slider _HPTarget;            // 当前敌人血条

    NumFont _beatTip;           // 连击数

    Image[] _comboTips;         // combo技能提示
    Image _fullComboTip;        // full combo提示

    Image[] _screenBloods;      // 屏幕溅血

    public ViewArena(UIType viewType)
        : base((int)viewType)
    {

    }

    public override void Init()
    {
        // 用根节点的transform来查找，可以找到隐藏子节点。用GameObject.Find则不行
        _btnPause = UIRoot.transform.Find("BtnPause").GetComponent<Button>();
        _btnPause.onClick.AddListener(OnClickPause);

        _headPlayer = UIRoot.transform.Find("PanelPlayer/Head").GetComponent<Image>();
        _HPPlayer = UIRoot.transform.Find("PanelPlayer/HP").GetComponent<Slider>();

        _headTarget = UIRoot.transform.Find("PanelTarget/Head").GetComponent<Image>();
        _HPTarget = UIRoot.transform.Find("PanelTarget/HP").GetComponent<Slider>();

        _beatTip = UIRoot.transform.Find("BeatTip/Tip").GetComponent<NumFont>();
        _comboTips = UIRoot.transform.Find("ComboTips/Tips").GetComponentsInChildren<Image>();
        _fullComboTip = UIRoot.transform.Find("ComboTips/FullComboTip").GetComponent<Image>();

        _screenBloods = UIRoot.transform.Find("ScreenBloods").GetComponentsInChildren<Image>();
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

    void OnClickPause()
    {
        Debug.Log("PAUSE");
    }

    public void ShowComboTips(List<OrderAttackType> attackTypeList)
    {
        if (attackTypeList.Count == 0)
        {
            HideComboTips();
            return;
        }
        if (attackTypeList.Count + 2 > _comboTips.Length)
        {
            return;
        }
        _comboTips[0].gameObject.SetActive(true);
        _comboTips[_comboTips.Length - 1].gameObject.SetActive(true);
        for (int i = 0; i < attackTypeList.Count; ++i)
        {
            if (attackTypeList[i] == OrderAttackType.X)
            {
                _comboTips[i + 1].sprite = UIFacadeComponent.Instance.comboTipSprites[0];
                _comboTips[i + 1].gameObject.SetActive(true);
            }
            else if (attackTypeList[i] == OrderAttackType.O)
            {
                _comboTips[i + 1].sprite = UIFacadeComponent.Instance.comboTipSprites[1];
                _comboTips[i + 1].gameObject.SetActive(true);
            }
        }
    }

    void HideComboTips()
    {
        foreach (var img in _comboTips)
        {
            img.gameObject.SetActive(false);
        }
    }

    public void ShowFullComboTip(ComboType fullComboType)
    {
        switch (fullComboType)
        {
            case ComboType.RAISE_WAVE:
                _fullComboTip.sprite = UIFacadeComponent.Instance.fullComboTipSprites[0];
                break;
            case ComboType.HALF_MOON:
                _fullComboTip.sprite = UIFacadeComponent.Instance.fullComboTipSprites[1];
                break;
            case ComboType.CLOUD_CUT:
                _fullComboTip.sprite = UIFacadeComponent.Instance.fullComboTipSprites[2];
                break;
            case ComboType.WALKING_DEATH:
                _fullComboTip.sprite = UIFacadeComponent.Instance.fullComboTipSprites[3];
                break;
            case ComboType.CRASH_GENERAL:
                _fullComboTip.sprite = UIFacadeComponent.Instance.fullComboTipSprites[4];
                break;
            case ComboType.FLYING_DRAGON:
                _fullComboTip.sprite = UIFacadeComponent.Instance.fullComboTipSprites[5];
                break;
            default:
                return;
        }
        _fullComboTip.gameObject.SetActive(true);
        UITween.Instance.Bounce(_fullComboTip.gameObject, 2, 0.25f, 2, 0);
    }

    public void ShowScreenBloods()
    {
        /*if (_screenBloods.activeSelf == false)
        {
            bloods.SetActive(true);
        }*/
        
        foreach (var img in _screenBloods)
        {
            img.sprite = UIFacadeComponent.Instance.screenBloodSprites[Random.Range(0, UIFacadeComponent.Instance.screenBloodSprites.Count)];
            img.rectTransform.localPosition = new Vector3(Random.Range(-60, 60), Random.Range(-60, 60), 0);
            img.rectTransform.localEulerAngles = new Vector3(0, 0, Random.Range(0, 180));
            UITween.Instance.FadeOut(img.gameObject, 1.5f, 5);
        }
    }

    public void ShowTargetPanel()
    {
        _headTarget.gameObject.transform.parent.gameObject.SetActive(true);
    }

    public void HideTargetPanel()
    {
        _headTarget.gameObject.transform.parent.gameObject.SetActive(false);
    }

    public void UpdateHPPlayer(float hp)
    {
        _HPPlayer.value = hp;
    }

    public void UpdateHPTarget(float hp)
    {
        _HPTarget.value = hp;
    }

    public void UpdateHeadPlayer(Sprite head)
    {
        _headPlayer.sprite = head;
    }

    public void UpdateHeadTarget(Sprite head)
    {
        _headTarget.sprite = head;
    }
}
