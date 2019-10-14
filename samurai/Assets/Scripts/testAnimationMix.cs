using UnityEngine;
using Phenix.Unity.Anim;
using Phenix.Unity.Utilities;

public class testAnimationMix : MonoBehaviour
{
    Animation _anim;
    AnimationMixMgr _mixMgr;

    [SerializeField]
    Transform _spine;

    [SerializeField]
    Transform _tar;

    bool _firing = false;

    // Use this for initialization
    void Start()
    {
        _anim = GetComponent<Animation>();
        _mixMgr = GetComponent<AnimationMixMgr>();
        Debug.Log(Vector3.Scale(new Vector3(1,0,0), new Vector3(10,100, 1000)));
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyUp(KeyCode.A))
        {
            AnimationTools.PlayAnim(_anim, "combatMoveF", 0.2f);
            _firing = false;
        }
        if (Input.GetKeyUp(KeyCode.Space))
        {
            _mixMgr.PlayAnim(_anim, "mixBowFire", 0.2f);
            _firing = true;
        }
        if (Input.GetKeyUp(KeyCode.B))
        {
            AnimationTools.PlayAnim(_anim, "idle", 2f);
            _firing = false;
        }

        if (_firing && _spine != null)
        {
            _spine.LookAt(_tar);
        }
    }
}
