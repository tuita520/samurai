using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    public Vector3 Position { get { return transform.position; } }
    public Vector3 Forward { get { return transform.forward; } }
    public Vector3 Right { get { return transform.right; } }
    public Vector3 ChestPosition { get { return Transform.position + transform.up * 1.5f; } }

    public bool IsPlayer { get { return agentType == AgentType.PLAYER; } }
    
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
        
        _audioEffect = GetComponent<AudioSource>();
        _collisionCenter = CharacterController.center;
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
}
