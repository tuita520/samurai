using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ViewCtrlMain : ViewCtrlBase
{
    public UISet uiSet;
    public Slider hpSlider;
    public Image fullCombo;
    public GameObject bloods;
    public Button pauseBtn;
    
    public List<Image> comboList = new List<Image>();
    public List<Image> hitCount = new List<Image>();
    public Image hitWords;

    // Use this for initialization
    void Start ()
    {
        HideCombo();
        fullCombo.gameObject.SetActive(false);
        bloods.SetActive(false);
        foreach (var img in hitCount)
        {
            img.gameObject.SetActive(false);
        }
        hitWords.gameObject.SetActive(false);
        pauseBtn.onClick.AddListener(OnPauseBtnClick);
    }
	
    public void SetHP(float hpRate)
    {
        hpSlider.value = hpRate;
    }

    public void ShowCombo(List<OrderAttackType> attackTypeList)
    {
        if (attackTypeList.Count == 0)
        {
            HideCombo();
            return;
        }
        if (attackTypeList.Count + 2 > comboList.Count)
        {
            return;
        }
        comboList[0].gameObject.SetActive(true);
        comboList[comboList.Count-1].gameObject.SetActive(true);
        for (int i = 0; i < attackTypeList.Count; ++i)
        {
            if (attackTypeList[i] == OrderAttackType.X)
            {                
                comboList[i + 1].sprite = uiSet.comboSprites[0];
                comboList[i + 1].gameObject.SetActive(true);
            }
            else if(attackTypeList[i] == OrderAttackType.O)
            {
                comboList[i + 1].sprite = uiSet.comboSprites[1];
                comboList[i + 1].gameObject.SetActive(true);
            }
        }        
    }

    void HideCombo()
    {
        foreach (var img in comboList)
        {
            img.gameObject.SetActive(false);
        }
    }

    public void ShowFullCombo(ComboType fullComboType)
    {
        switch (fullComboType)
        {            
            case ComboType.RAISE_WAVE:
                fullCombo.sprite = uiSet.fullComboSprites[0];
                break;
            case ComboType.HALF_MOON:
                fullCombo.sprite = uiSet.fullComboSprites[1];
                break;
            case ComboType.CLOUD_CUT:
                fullCombo.sprite = uiSet.fullComboSprites[2];
                break;
            case ComboType.WALKING_DEATH:
                fullCombo.sprite = uiSet.fullComboSprites[3];
                break;
            case ComboType.CRASH_GENERAL:
                fullCombo.sprite = uiSet.fullComboSprites[4];
                break;
            case ComboType.FLYING_DRAGON:
                fullCombo.sprite = uiSet.fullComboSprites[5];
                break;
            default:
                return;
        }
        fullCombo.gameObject.SetActive(true);
        UITween.Instance.Bounce(fullCombo.gameObject, 2, 0.25f, 2, 0);
    }

    public void ShowBlood()
    {
        if (bloods.activeSelf == false)
        {
            bloods.SetActive(true);
        }
        Image[] images = bloods.GetComponentsInChildren<Image>();
        foreach (var img in images)
        {
            img.sprite = uiSet.bloodSprites[Random.Range(0, uiSet.bloodSprites.Count)];
            img.rectTransform.localPosition = new Vector3(Random.Range(-60, 60), Random.Range(-60, 60), 0);
            img.rectTransform.localEulerAngles = new Vector3(0, 0, Random.Range(0, 180));
            UITween.Instance.FadeOut(img.gameObject, 1.5f, 5);
        }         
    }

    public void ShowHit(uint hitCountVal)
    {
        List<uint> numArr = new List<uint>();
        Mathfx.GetNumArr(hitCountVal, numArr);
        if (numArr.Count > hitCount.Count)
        {
            Debug.Log("not enough hit num image, expected " + numArr.Count + " but " + hitCount.Count);
            return;
        }
        for (int i = 0; i < numArr.Count; ++i)
        {
            int j = hitCount.Count - 1 - i;
            if (hitCount[j].gameObject.activeSelf == false)
            {
                hitCount[j].gameObject.SetActive(true);                
            }
            hitCount[j].sprite = uiSet.hitNumSprites[(int)numArr[i]];
            UITween.Instance.Bounce(hitCount[j].gameObject, 1.5f, 0.25f, 0.5f, 1);
        }
        if (hitWords.gameObject.activeSelf == false)
        {
            hitWords.gameObject.SetActive(true);
        }        
        UITween.Instance.Bounce(hitWords.gameObject, 1.5f, 0.25f, 0.5f, 1);
    }

    void OnPauseBtnClick()
    {
        Game.Instance.PauseGame();
        Time.timeScale = 0;
        Hide();
        UIMgr.Instance.views[ViewType.OPT].Show();
    }
}
