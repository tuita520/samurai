using Phenix.Unity.UI;

public class ModelArena : Model
{
    float _hpPlayer = 0;
    float _hpTarget = 0;    

    UnityEngine.Sprite _headPlayer = null;
    UnityEngine.Sprite _headTarget = null;       

    public Agent1 Player { get; set; }
    public Agent1 Target { get; set; }

    public ModelArena(UIType uiType)
        : base((int)uiType)
    {

    }

    public override void OnUpdate()
    {
        UpdateHP();
        UpdateHead();
    }

    void UpdateHP()
    {
        if (Player)
        {
            float hp = Player.BlackBoard.health / Player.BlackBoard.maxHealth;
            if (hp != _hpPlayer)
            {
                _hpPlayer = hp;
                (View.Get(UIID) as ViewArena).UpdateHPPlayer(hp);
            }
        }

        if (Target)
        {
            float hp = Target.BlackBoard.health / Target.BlackBoard.maxHealth;
            if (hp != _hpTarget)
            {
                _hpTarget = hp;
                (View.Get(UIID) as ViewArena).UpdateHPTarget(hp);
            }
        }        
    }

    void UpdateHead()
    {
        if (Player)
        {
            UnityEngine.Sprite head = Player.BlackBoard.head;
            if (head != _headPlayer)
            {
                _headPlayer = head;
                (View.Get(UIID) as ViewArena).UpdateHeadPlayer(head);
            }
        }

        if (Target)
        {
            UnityEngine.Sprite head = Target.BlackBoard.head;
            if (head != _headTarget)
            {
                _headTarget = head;
                (View.Get(UIID) as ViewArena).UpdateHeadTarget(head);
            }
        }
    }
}
