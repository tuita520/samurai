using UnityEngine;
using Phenix.Unity.Pattern;

public class Game1 : Singleton<Game1>
{
    [SerializeField]
    AgentMgr _agentMgr = new AgentMgr();

    public AgentMgr AgentMgr { get { return _agentMgr; } }
    public Transform world;


    //BGM _bgm;
    GameState _gameState;

	// Use this for initialization
	protected override void Awake ()
    {
        base.Awake();
        //_bgm = GetComponent<BGM>();
        _gameState = GameState.Game;
        world = GameObject.Find("World").transform;
    }

    void Start()
    {
        //_bgm.FadeIn(_bgm.bgmClip);
        SoundCenter.Instance.SoundMgr.PlayLoop((int)SoundType.BGM);
    }

    // Update is called once per frame
    void Update ()
    {
        if (IsPause())
        {
            return;
        }
        //foreach (var agent in agents)
        //{            
        //    if (agent.gameObject.activeSelf)
        //    {
        //        //agent.Loop();
        //    }            
        //}
	}

    public void PauseGame()
    {
        _gameState = GameState.Pause;
    }

    public void ResumeGame()
    {
        _gameState = GameState.Game;
    }

    public bool IsPause()
    {
        return _gameState == GameState.Pause;
    }
}
