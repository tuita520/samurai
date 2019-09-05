using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ComboMgr : MonoBehaviour
{
    class ComboStep
    {
        public OrderAttackType attackType;     
        public AnimAttackData data;
    }

    class Combo
    {
        public ComboType fullComboType;
        public ComboStep[] comboSteps;
    }

    AnimSetPlayer       _animSetPlayer;
    List<OrderAttackType>    _comboProgress = new List<OrderAttackType>();    // 连续攻击动作类型
    Combo[]             _playerComboAttacks = new Combo[6];         // 6种连招数据

    public void Reset()
    {
        _comboProgress.Clear();
    }

    void Start()
    {
        _animSetPlayer = GetComponent<AnimSetPlayer>();
        _playerComboAttacks[0] = new Combo()
        {
            fullComboType = ComboType.RAISE_WAVE,
            comboSteps = new ComboStep[]
            {
                new ComboStep(){attackType = OrderAttackType.X, data = _animSetPlayer.attackDataList[0]},
                new ComboStep(){attackType = OrderAttackType.X, data = _animSetPlayer.attackDataList[1]},
                new ComboStep(){attackType = OrderAttackType.X, data = _animSetPlayer.attackDataList[2]},                                
                new ComboStep(){attackType = OrderAttackType.X, data = _animSetPlayer.attackDataList[3]},
                //new ComboStep(){attackType = AttackType.None, data = _animSetPlayer.AttackData[3]},
                new ComboStep(){attackType = OrderAttackType.O, data = _animSetPlayer.attackDataList[4]},
            }
        };
        _playerComboAttacks[1] = new Combo()
        {
            fullComboType = ComboType.HALF_MOON,
            comboSteps = new ComboStep[]
            {
                new ComboStep(){attackType = OrderAttackType.O, data = _animSetPlayer.attackDataList[5]},
                new ComboStep(){attackType = OrderAttackType.O, data = _animSetPlayer.attackDataList[6]},
                new ComboStep(){attackType = OrderAttackType.O, data = _animSetPlayer.attackDataList[7]},
                new ComboStep(){attackType = OrderAttackType.X, data = _animSetPlayer.attackDataList[8]},
                new ComboStep(){attackType = OrderAttackType.X, data = _animSetPlayer.attackDataList[9]},
            }
        };
        _playerComboAttacks[2] = new Combo()
        {
            fullComboType = ComboType.CLOUD_CUT,
            comboSteps = new ComboStep[]
            {               
                    new ComboStep(){attackType = OrderAttackType.O, data = _animSetPlayer.attackDataList[5]},
                    new ComboStep(){attackType = OrderAttackType.O, data = _animSetPlayer.attackDataList[6]},
                    new ComboStep(){attackType = OrderAttackType.X, data = _animSetPlayer.attackDataList[17]},
                    new ComboStep(){attackType = OrderAttackType.X, data = _animSetPlayer.attackDataList[18]},
                    new ComboStep(){attackType = OrderAttackType.X, data = _animSetPlayer.attackDataList[19]},
            }
        };

        _playerComboAttacks[3] = new Combo()
        {
            fullComboType = ComboType.FLYING_DRAGON,
            comboSteps = new ComboStep[]
            {
                new ComboStep(){attackType = OrderAttackType.X, data = _animSetPlayer.attackDataList[0]},
                new ComboStep(){attackType = OrderAttackType.O, data = _animSetPlayer.attackDataList[10]},
                new ComboStep(){attackType = OrderAttackType.O, data = _animSetPlayer.attackDataList[11]},
                new ComboStep(){attackType = OrderAttackType.X, data = _animSetPlayer.attackDataList[12]},
                new ComboStep(){attackType = OrderAttackType.X, data = _animSetPlayer.attackDataList[13]},
            }
        };
        _playerComboAttacks[4] = new Combo()
        {
            fullComboType = ComboType.WALKING_DEATH,
            comboSteps = new ComboStep[]
            {
                new ComboStep(){attackType = OrderAttackType.X, data = _animSetPlayer.attackDataList[0]},
                new ComboStep(){attackType = OrderAttackType.X, data = _animSetPlayer.attackDataList[1]},
                new ComboStep(){attackType = OrderAttackType.O, data = _animSetPlayer.attackDataList[14]},
                new ComboStep(){attackType = OrderAttackType.X, data = _animSetPlayer.attackDataList[15]},
                new ComboStep(){attackType = OrderAttackType.X, data = _animSetPlayer.attackDataList[16]},
            }
        };

        _playerComboAttacks[5] = new Combo()
        {
            fullComboType = ComboType.CRASH_GENERAL,
            comboSteps = new ComboStep[]
            {
                new ComboStep(){attackType = OrderAttackType.O, data = _animSetPlayer.attackDataList[5]},
                new ComboStep(){attackType = OrderAttackType.X, data = _animSetPlayer.attackDataList[20]},
                new ComboStep(){attackType = OrderAttackType.O, data = _animSetPlayer.attackDataList[21]},
                new ComboStep(){attackType = OrderAttackType.O, data = _animSetPlayer.attackDataList[22]},
                new ComboStep(){attackType = OrderAttackType.O, data = _animSetPlayer.attackDataList[23]},
            }
        };
    }    

    public AnimAttackData ProcessCombo(OrderAttackType attackType)
    {
        if (attackType != OrderAttackType.O && attackType != OrderAttackType.X)
            return null;

        _comboProgress.Add(attackType);

        // 遍历每一种连招
        for (int i = 0; i < _playerComboAttacks.Length; ++i)
        {
            Combo combo = _playerComboAttacks[i];
            bool valid = _comboProgress.Count <= combo.comboSteps.Length;

            // 遍历出招记录
            for (int ii = 0; ii < _comboProgress.Count && ii < combo.comboSteps.Length; ++ii)
            {
                if (_comboProgress[ii] != combo.comboSteps[ii].attackType)
                {
                    valid = false;
                    break;
                }
            }

            if (valid)
            {
                combo.comboSteps[_comboProgress.Count-1].data.lastAttackInCombo = (NextAttackIsAvailable(OrderAttackType.X) == false && NextAttackIsAvailable(OrderAttackType.O) == false);
                combo.comboSteps[_comboProgress.Count-1].data.firstAttackInCombo = false;
                combo.comboSteps[_comboProgress.Count-1].data.comboIndex = i;
                combo.comboSteps[_comboProgress.Count-1].data.fullCombo = _comboProgress.Count == combo.comboSteps.Length;
                combo.comboSteps[_comboProgress.Count-1].data.comboStep = _comboProgress.Count;
                
                //GuiManager.Instance.ShowComboProgress(ComboProgress);
                return combo.comboSteps[_comboProgress.Count-1].data;
            }
        }

        _comboProgress.Clear();
        _comboProgress.Add(attackType);

        for (int i = 0; i < _playerComboAttacks.Length; i++)
        {
            if (_playerComboAttacks[i].comboSteps[0].attackType == attackType)
            {
                // Debug.Log(Time.timeSinceLevelLoad + " New combo " + i + " step " + ComboProgress.Count);
                _playerComboAttacks[i].comboSteps[0].data.lastAttackInCombo = false;
                _playerComboAttacks[i].comboSteps[0].data.firstAttackInCombo = true;                
                _playerComboAttacks[i].comboSteps[0].data.comboIndex = i;
                _playerComboAttacks[i].comboSteps[0].data.fullCombo = false;
                _playerComboAttacks[i].comboSteps[0].data.comboStep = 0;

                //GuiManager.Instance.ShowComboProgress(ComboProgress);
                return _playerComboAttacks[i].comboSteps[0].data;
            }
        }

        Debug.LogError("Could not find any combo attack !!! Some shit happens");

        return null;
    }

    bool NextAttackIsAvailable(OrderAttackType attackType)
    {
        if (attackType != OrderAttackType.O && attackType != OrderAttackType.X)
            return false;

        if (_comboProgress.Count == 5)
            return false;

        List<OrderAttackType> progress = new List<OrderAttackType>(_comboProgress);
        progress.Add(attackType);

        for (int i = 0; i < _playerComboAttacks.Length; i++)
        {
            Combo combo = _playerComboAttacks[i];           

            bool valid = true;
            for (int ii = 0; ii < progress.Count; ii++)
            {
                if (progress[ii] != combo.comboSteps[ii].attackType)
                {
                    valid = false;
                    break;
                }
            }

            if (valid)
                return true;
        }
        return false;
    }
}
