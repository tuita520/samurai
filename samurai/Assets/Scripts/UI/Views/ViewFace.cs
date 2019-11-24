using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Phenix.Unity.UI;
using DG.Tweening;
using UnityEngine.EventSystems;

public class ViewFace : View
{
    ModelFace _model;

    SimpleScrollView _heads;            // 头像滑动列表
    RadarChart _radarChart;             // 属性雷达图

    LongPressable _btnTurnLeft;         // TV对象旋转钮
    LongPressable _btnTurnRight;        // TV对象旋转钮
    Button _btnClose;

    Transform _target;                  // TV对象父节点
    float _rotateSpeed = 1;             // TV对象旋转速度
    float _width = 0;

    public ViewFace(UIType viewType)
        : base((int)viewType)
    {

    }

    public void SetSpeed(float val) { _radarChart.SetPropValue(0, val / 100); }
    public void SetStamina(float val) { _radarChart.SetPropValue(1, val / 100); }
    public void SetSword(float val) { _radarChart.SetPropValue(2, val / 100); }
    public void SetFoxy(float val) { _radarChart.SetPropValue(3, val / 100); }
    public void SetDefend(float val) { _radarChart.SetPropValue(4, val / 100); }

    public override void Init()
    {
        _model = Model.Get((int)UIType.VIEW_FACE) as ModelFace;

        _width = (UIRoot.transform as RectTransform).rect.width;

        _heads = UIRoot.transform.Find("Faces/Panel/Body/Heads").GetComponent<SimpleScrollView>();
        _radarChart = UIRoot.transform.Find("Faces/Panel/Body/Chart/RadarPanel/RadarChart").GetComponent<RadarChart>();

        _target = UIRoot.transform.Find("Faces/Panel/Body/Chart/TVShow/TVCamera/Target");
        _btnTurnLeft = UIRoot.transform.Find("Faces/Panel/Body/Chart/TVShow/BtnTurnLeft").GetComponent<LongPressable>();
        _btnTurnRight = UIRoot.transform.Find("Faces/Panel/Body/Chart/TVShow/BtnTurnRight").GetComponent<LongPressable>();

        _btnClose = UIRoot.transform.Find("Faces/BtnClose").GetComponent<Button>();        

        _btnTurnLeft.onLongPressing.AddListener((x) => { _target.Rotate(Vector3.up, -_rotateSpeed); });
        _btnTurnRight.onLongPressing.AddListener((x) => { _target.transform.Rotate(Vector3.up, _rotateSpeed); });
        _btnClose.onClick.AddListener(() => { Close(); });

        _heads.onCellSelected.AddListener(OnHeadSelectedChange);        
    }

    void OnHeadSelectedChange(GameObject preSelected, GameObject curSelected)
    {
        if (preSelected)
        {
            // 取消选中
            preSelected.transform.Find("Selected").gameObject.SetActive(false);
        }
        if (curSelected)
        {
            // 设置选中
            curSelected.transform.Find("Selected").gameObject.SetActive(true);

            // 刷新数据
            SetDefend(_model.GetDefend(curSelected.name));
            SetSpeed(_model.GetSpeed(curSelected.name));
            SetSword(_model.GetSword(curSelected.name));
            SetStamina(_model.GetStamina(curSelected.name));
            SetFoxy(_model.GetFoxy(curSelected.name));

            // 更新TVShow对象
            if (_target.childCount > 0)
            {
                // 保证只可能有一个对象
                GameObject.DestroyImmediate(_target.GetChild(0).gameObject);
            }            
            GameObject go = _model.GetTarget(curSelected.name);
            if (go)
            {
                go.transform.SetParent(_target);
                go.transform.localPosition = Vector3.zero;
                go.transform.localScale = Vector3.one;
                go.transform.forward = new Vector3(go.transform.forward.x, go.transform.forward.y, -go.transform.forward.z);                
            }
        }
    }

    public override void Open()
    {        
        FillHeads(_model.GetHeads());

        base.Open();
        UIRoot.SetActive(true);
        (UIRoot.transform as RectTransform).DOMoveX(_width, 1).From(true);
    }

    void FillHeads(List<GameObject> heads)
    {
        bool selected = false;
        foreach (var head in heads)
        {            
            if (head == null)
            {
                continue;
            }
            
            head.GetComponent<Clickable>().onClick.AddListener(() => { _heads.CellSelected = head; });
            _heads.Add(head, false);
            if (selected == false)
            {
                _heads.CellSelected = head;
                selected = true;
            }
        }
    }

    public override void Close()
    {
        (UIRoot.transform as RectTransform).DOMoveX(_width, 1).SetRelative(true).OnComplete(() => { base.Close(); UIRoot.SetActive(false); });
    }

    public override void OnUpdate()
    {

    }
}
