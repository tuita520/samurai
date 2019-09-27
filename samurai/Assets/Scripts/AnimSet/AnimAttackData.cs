using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimAttackData
{
    public string animName;
    public float moveDistance;// best attack distance

    //timers
    public float attackMoveStartTime;
    public float attackMoveEndTime;
    public float attackEndTime;    

    // hit parameters
    public float hitTime;
    public float hitDamage;
    public float hitAngle;
    public float hitMomentum;
    public CriticalHitType hitCriticalType;
    public bool hitAreaKnockdown; // 有击倒效果
    //public bool breakBlock; // 有破防效果
    public bool useImpuls;
    public float criticalModificator = 1;
    //public bool sloMotion;

    // trail
    public GameObject trail;
    public Transform trailParenTrans;
    public GameObject dust;
    public Animation animationDust;
    public Animation anim;
    public Renderer renderer;
    public Material material;    
    public Material materialDust;

    // combo
    public bool firstAttackInCombo = true;
    public bool lastAttackInCombo = false;
    public int comboIndex;
    public bool fullCombo;
    public int comboStep;

    public bool isFatal = false; // 是否一击必杀
    public bool isCounter = false; // 是否属于反击技能

    public AnimAttackData(string animName, GameObject trail, float moveDistance, float hitTime, float attackEndTime, float hitDamage, 
        float hitAngle, float hitMomentum, CriticalHitType criticalType, bool areaKnockDown, bool isFatal = false, bool isCounter = false)
    {
        this.animName = animName;
        this.trail = trail;

        if (this.trail)
        {
            trailParenTrans = this.trail.transform.parent;

            if (this.trail.transform.Find("dust"))
            {
                dust = this.trail.transform.Find("dust").gameObject;
                animationDust = dust.GetComponent<Animation>();
                materialDust = dust.GetComponent<Renderer>().material;
            }

            anim = this.trail.transform.parent.GetComponent<Animation>();

            renderer = this.trail.GetComponentInChildren(typeof(Renderer)) as Renderer;
            if (renderer == null)
            {                
                renderer = this.trail.GetComponentInChildren(typeof(SkinnedMeshRenderer)) as Renderer;                
            }

            if (renderer == null)
            {
                material = null;
                Debug.LogError("Trail - no Material");
            }
            else
            {
                material = renderer.material;
            }
        }
        else
        {
            anim = null;
            renderer = null;
            material = null;
        }

        this.moveDistance = moveDistance;

        this.attackEndTime = attackEndTime;
        attackMoveStartTime = 0;
        attackMoveEndTime = this.attackEndTime * 0.7f;

        this.hitTime = hitTime;
        this.hitDamage = hitDamage;
        this.hitAngle = hitAngle;
        this.hitMomentum = hitMomentum;
        hitCriticalType = criticalType;
        hitAreaKnockdown = areaKnockDown;        
        useImpuls = false;
        criticalModificator = 1;
        this.isFatal = isFatal;
        this.isCounter = isCounter;
    }

    public AnimAttackData(string animName, GameObject trail, float moveDistance, float hitTime, float moveStartTime, float moveEndTime, 
        float attackEndTime, float hitDamage, float hitAngle, float hitMomentum, CriticalHitType criticalType, float criticalMod, 
        bool areaKnockDown, bool breakBlock, bool useImpuls, bool sloMotion, bool isFatal = false, bool isCounter = false)
    {
        this.animName = animName;
        this.trail = trail;

        if (this.trail)
        {
            trailParenTrans = this.trail.transform.parent;

            if (this.trail.transform.Find("dust"))
            {
                dust = this.trail.transform.Find("dust").gameObject;
                animationDust = dust.GetComponent<Animation>();
                materialDust = dust.GetComponent<Renderer>().material;
            }

            anim = this.trail.transform.parent.GetComponent<Animation>();

            renderer = this.trail.GetComponentInChildren(typeof(Renderer)) as Renderer;
            if (renderer == null)
            {
                renderer = this.trail.GetComponentInChildren(typeof(SkinnedMeshRenderer)) as Renderer;
            }

            if (renderer == null)
            {
                material = null;
                Debug.LogError("Trail - no Material");
            }
            else
            {
                material = renderer.material;
            }
        }
        else
        {
            anim = null;
            renderer = null;
            material = null;
        }

        this.moveDistance = moveDistance;

        attackMoveStartTime = moveStartTime;
        attackMoveEndTime = moveEndTime;
        this.attackEndTime = attackEndTime;

        this.hitTime = hitTime;
        this.hitDamage = hitDamage;
        this.hitAngle = hitAngle;
        this.hitMomentum = hitMomentum;
        hitCriticalType = criticalType;
        hitAreaKnockdown = areaKnockDown;        
        this.useImpuls = useImpuls;
        criticalModificator = criticalMod;
        //this.sloMotion = sloMotion;
        this.isFatal = isFatal;
        this.isCounter = isCounter;
    }


    override public string ToString() { return base.ToString() + ": " + animName; }
}
