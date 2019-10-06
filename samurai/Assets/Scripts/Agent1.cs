using System.Collections;
using UnityEngine;
using Phenix.Unity.Effect;

public class Agent1 : MonoBehaviour
{
    public AgentType agentType;
    public int group = 0; // 同组为友，异组为敌
    Vector3 _collisionCenter;

    [SerializeField]
    BlackBoard1 _blackBoard;

    //public AnimDataSet animDataSet;

    //Fsm         _animFsm;
    //PlayerInput _input;
    //GOAPManager _goapMgr;
    AudioSource _audioEffect;       // 音效

    public Decision Decision { get; private set; }
    public PlayerOrder PlayerOrder { get; private set; }
    public FSMComponent FSMComponent { get; private set; }
    public BlackBoard1 BlackBoard { get { return _blackBoard; } }
    public Animation AnimEngine { get; private set; }
    public Transform Transform { get; private set; }
    public CharacterController CharacterController { get; private set; }
    public AnimSet AnimSet { get; private set; }
    public SoundMgr SoundMgr { get; private set; }
    public MotionTrace MotionTrace { get; private set; }    

    public Vector3 Position { get { return transform.position; } }
    public Vector3 Forward { get { return transform.forward; } }
    public Vector3 Right { get { return transform.right; } }
    public Vector3 ChestPosition { get { return Transform.position + transform.up * 1.5f; } }    

    public bool IsPlayer { get { return agentType == AgentType.PLAYER; } }

    [HideInInspector]
    public ParticleSystem particleSystemRollTust;
    [HideInInspector]
    public ParticleSystem particleSystemFlashTust;
    [HideInInspector]
    public ParticleSystem particleSystemWhirlWind;

    public TrailGenerator[] trailGeneratorList;

    void Awake()
    {
        _blackBoard.Agent = this;
        _blackBoard.Reset();

        Decision = GetComponent<GOAPDecision>();
        PlayerOrder = GetComponent<PlayerOrder>();
        FSMComponent = GetComponent<FSMComponent>();        
        AnimEngine = GetComponent<Animation>();
        Transform = GetComponent<Transform>();
        CharacterController = GetComponent<CharacterController>();
        AnimSet = GetComponent<AnimSet>();
        SoundMgr = GetComponent<SoundMgr>();
        MotionTrace = GetComponent<MotionTrace>();
        
        _audioEffect = GetComponent<AudioSource>();
        _collisionCenter = CharacterController.center;

        Transform tust = transform.Find("rollTust");
        if (tust)
        {
            particleSystemRollTust = tust.GetComponent<ParticleSystem>();
        }
        tust = transform.Find("flashTust");
        if (tust)
        {
            particleSystemFlashTust = tust.GetComponent<ParticleSystem>();
        }
        Transform whirlWind = transform.Find("whirlWind");
        if (whirlWind)
        {
            particleSystemWhirlWind = whirlWind.GetComponent<ParticleSystem>();
        }
    }

    private void Update()
    {
        UpdateCombatProps();
    }

    void UpdateCombatProps()
    {
        BlackBoard.Rage = BlackBoard.Rage + BlackBoard.rageModificator * 1.2f * Time.deltaTime;
        BlackBoard.Berserk = BlackBoard.Berserk + BlackBoard.berserkModificator * 1.2f * Time.deltaTime;

        if (BlackBoard.desiredTarget != null && BlackBoard.AheadOfDesiredTarget)
            BlackBoard.Fear = BlackBoard.Fear + BlackBoard.fearModificator * Time.deltaTime;
        else
            BlackBoard.Fear = BlackBoard.Fear - BlackBoard.fearModificator * Time.deltaTime;

        if (BlackBoard.IsBlocking)
            BlackBoard.Block = BlackBoard.Block + BlackBoard.blockModificator * Time.deltaTime;
    }

    public void UpdateCombatProps(Phenix.Unity.AI.FSMEvent ev)
    {
        switch (ev.EventCode)
        {
            case (int)AnimFSMEventCode.BREAK_BLOCK:
                if ((ev as AnimFSMEventBreakBlock).success)
                {
                    BlackBoard.Fear = BlackBoard.Fear + BlackBoard.fearInjuryModificator * 0.5f;
                    BlackBoard.Rage = BlackBoard.Rage + BlackBoard.rageInjuryModificator * 0.5f;                    
                }
                else
                {
                    BlackBoard.Fear = BlackBoard.Fear + BlackBoard.fearBlockModificator;
                    BlackBoard.Rage = BlackBoard.Rage + BlackBoard.rageBlockModificator;
                    BlackBoard.Berserk = BlackBoard.Berserk + BlackBoard.berserkBlockModificator;
                }
                break;
            case (int)AnimFSMEventCode.INJURY:
                BlackBoard.Fear = BlackBoard.Fear + BlackBoard.fearInjuryModificator;
                BlackBoard.Rage = BlackBoard.Rage + BlackBoard.rageInjuryModificator;
                BlackBoard.Block = BlackBoard.Block + BlackBoard.blockInjuryModificator;
                BlackBoard.Berserk = BlackBoard.Berserk + BlackBoard.berserkInjuryModificator;
                break;
            case (int)AnimFSMEventCode.ATTACK_WHIRL:
            case (int)AnimFSMEventCode.ATTACK_ROLL:            
                BlackBoard.Berserk = BlackBoard.minBerserk;
                break;
            case (int)AnimFSMEventCode.ATTACK_MELEE:
                BlackBoard.Rage = BlackBoard.minRage;
                BlackBoard.Block = BlackBoard.Block + BlackBoard.blockAttackModificator;
                BlackBoard.Fear = BlackBoard.Fear + BlackBoard.fearAttackModificator;
                if ((ev as AnimFSMEventAttackMelee).animAttackData.isCounter)
                {
                    BlackBoard.Berserk = BlackBoard.minBerserk;
                }
                else
                {
                    BlackBoard.Berserk = BlackBoard.Berserk + BlackBoard.berserkAttackModificator;
                }
                break;
            case (int)AnimFSMEventCode.BLOCK:
                BlackBoard.Block = BlackBoard.minBlock;
                break;
            case (int)AnimFSMEventCode.COMBAT_MOVE:
                if ((ev as AnimFSMEventCombatMove).moveType == MoveType.BACKWARD)
                    BlackBoard.Fear = BlackBoard.minFear;
                break;
            default:
                break;
        }        
    }
    
   

    

    

    public void PlaySound(AudioClip clip)
    {
        if (_audioEffect && clip)
            _audioEffect.PlayOneShot(clip);
    }

    public void PlaySoundLoop(AudioClip clip, float delay, float time, float fadeInTime, float fadeOutTime)
    {
        StartCoroutine(PlaySoundLoopImp(clip, delay, time, fadeInTime, fadeOutTime));
    }

    IEnumerator PlaySoundLoopImp(AudioClip clip, float delay, float time/*播放时长*/, float fadeInTime, float fadeOutTime)
    {
        if (_audioEffect == null || clip == null)
        {
            yield break;
        }

        _audioEffect.volume = 0;
        _audioEffect.loop = true;
        _audioEffect.clip = clip;

        yield return new WaitForEndOfFrame();
        yield return new WaitForSeconds(delay);

        _audioEffect.Play();

        float step = 1 / fadeInTime;
        while (_audioEffect.volume < 1)
        {
            _audioEffect.volume = Mathf.Min(1.0f, _audioEffect.volume + step * Time.deltaTime);
            yield return new WaitForEndOfFrame();
        }

        yield return new WaitForSeconds(time - fadeInTime - fadeOutTime);

        step = 1 / fadeInTime;
        while (_audioEffect.volume > 0)
        {
            _audioEffect.volume = Mathf.Max(0.0f, _audioEffect.volume - step * Time.deltaTime);
            yield return new WaitForEndOfFrame();
        }

        _audioEffect.Stop();

        yield return new WaitForEndOfFrame();

        _audioEffect.volume = 1;
    }

    public void DisableCollisions()
    {
        CharacterController.detectCollisions = false;
        CharacterController.center = Vector3.up * -20;
    }

    public void EnableCollisions()
    {
        CharacterController.detectCollisions = true;
        CharacterController.center = _collisionCenter;
    }

    public void PlayWeaponTrail(bool isStab)
    {
        foreach (var trailGenerator in trailGeneratorList)
        {
            trailGenerator.Play(isStab);
        }
    }

    public void StopWeaponTrail()
    {
        foreach (var trailGenerator in trailGeneratorList)
        {
            trailGenerator.Stop();
        }
    }

    /*
    public void ShowTrail(AnimAttackData data, float speed, float delay, bool critical, float dustDelay)
    {
        StartCoroutine(ShowTrailImpl(data, speed, delay, critical, dustDelay));
    }

    protected IEnumerator ShowTrailImpl(AnimAttackData data, float speed, float delay, bool critical, float dustDelay)
    {
        if (data.trail == null)
            yield break;

        if (dustDelay < 0)
            dustDelay = 0;
        
        data.trailParenTrans.position = Transform.position + Vector3.up * 0.15f;
        data.trailParenTrans.rotation = Quaternion.AngleAxis(Transform.rotation.eulerAngles.y, Vector3.up);

        data.trail.SetActive(true);

        if (data.dust)
            data.dust.SetActive(false);

        Color color = Color.white;

        data.material.SetColor("_TintColor", color);

        if (data.dust)
        {
            yield return new WaitForSeconds(dustDelay);
            StartCoroutine(ShowTrailDust(data));
        }

        yield return new WaitForSeconds(delay - dustDelay);

        while (color.a > 0)
        {
            color.a -= Time.deltaTime * speed;
            if (color.a < 0)
                color.a = 0;

            data.material.SetColor("_TintColor", color);
            yield return new WaitForEndOfFrame();
        }

        data.trail.SetActive(false);
    }

    public IEnumerator ShowTrailDust(AnimAttackData data)
    {
        Color colorDust = new Color(1, 1, 1, 1);
        data.dust.SetActive(true);

        data.materialDust.SetColor("_TintColor", colorDust);

        data.animationDust["Anim_Dust"].speed = 2.0f;
        data.animationDust.Play();

        while (colorDust.a > 0)
        {
            colorDust.a -= Time.deltaTime * 3;
            if (colorDust.a < 0)
                colorDust.a = 0;

            data.materialDust.SetColor("_TintColor", colorDust);
            yield return new WaitForEndOfFrame();
        }

        data.dust.SetActive(false);
    }*/
}
