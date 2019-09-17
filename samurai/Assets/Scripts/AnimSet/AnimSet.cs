using System.Collections.Generic;
using UnityEngine;

public abstract class AnimSet : MonoBehaviour
{
    public class ComboStep
    {
        public OrderAttackType orderAttackType;
        public AnimAttackData data;
    }

    public class Combo
    {
        public ComboType comboType;
        public ComboStep[] comboSteps;
    }
    
    List<OrderAttackType> _comboProgress = new List<OrderAttackType>();    // 连续攻击动作类型(出招表)
    List<Combo> _comboAttacks = new List<Combo>();
    int _curAttackIdx = 0;
    
    protected List<OrderAttackType> ComboProgress { get { return _comboProgress; } }
    protected List<Combo> ComboAttacks { get { return _comboAttacks; } }

    public void ResetComboProgress()
    {
        _comboProgress.Clear();
        _curAttackIdx = 0;
    }

    public int GetComboAttackCount(ComboType comboType)
    {
        for (int i = 0; i < _comboAttacks.Count; ++i)
        {
            Combo combo = _comboAttacks[i];
            if (combo.comboType == comboType)
            {
                return combo.comboSteps.Length;
            }
        }
        return 0;
    }

    public AnimAttackData ProcessSingle(ComboType comboType)
    {
        for (int i = 0; i < _comboAttacks.Count; ++i)
        {
            Combo combo = _comboAttacks[i];
            if (combo.comboType != comboType)
            {
                continue;
            }
            int len = combo.comboSteps.Length;
            if (len == 0)
            {
                return null;
            }
            // 返回随机一记单招
            return combo.comboSteps[Random.Range(0, len)].data;
        }
        return null;
    }

    public AnimAttackData ProcessCombo(ComboType comboType)
    {
        for (int i = 0; i < _comboAttacks.Count; ++i)
        {
            Combo combo = _comboAttacks[i];
            if (combo.comboType != comboType)
            {
                continue;
            }
            int len = combo.comboSteps.Length;
            if (len == 0)
            {
                return null;
            }
            return combo.comboSteps[_curAttackIdx++ % len].data;       
        }
        return null;
    }

    public AnimAttackData ProcessOrderCombo(OrderAttackType attackType)
    {
        if (attackType != OrderAttackType.O && attackType != OrderAttackType.X)
            return null;

        _comboProgress.Add(attackType);

        // 遍历每一种连招
        for (int i = 0; i < _comboAttacks.Count; ++i)
        {
            Combo combo = _comboAttacks[i];
            bool valid = _comboProgress.Count <= combo.comboSteps.Length;

            // 遍历出招记录
            for (int ii = 0; ii < _comboProgress.Count && ii < combo.comboSteps.Length; ++ii)
            {
                if (_comboProgress[ii] != combo.comboSteps[ii].orderAttackType)
                {
                    valid = false;
                    break;
                }
            }

            if (valid)
            {
                combo.comboSteps[_comboProgress.Count - 1].data.lastAttackInCombo = 
                    (NextAttackIsAvailable(OrderAttackType.X) == false && NextAttackIsAvailable(OrderAttackType.O) == false);
                combo.comboSteps[_comboProgress.Count - 1].data.firstAttackInCombo = false;
                combo.comboSteps[_comboProgress.Count - 1].data.comboIndex = i;
                combo.comboSteps[_comboProgress.Count - 1].data.fullCombo = 
                    _comboProgress.Count == combo.comboSteps.Length;
                combo.comboSteps[_comboProgress.Count - 1].data.comboStep = _comboProgress.Count;

                //GuiManager.Instance.ShowComboProgress(ComboProgress);
                return combo.comboSteps[_comboProgress.Count - 1].data;
            }
        }

        _comboProgress.Clear();
        _comboProgress.Add(attackType);

        for (int i = 0; i < _comboAttacks.Count; i++)
        {
            if (_comboAttacks[i].comboSteps[0].orderAttackType == attackType)
            {
                // Debug.Log(Time.timeSinceLevelLoad + " New combo " + i + " step " + ComboProgress.Count);
                _comboAttacks[i].comboSteps[0].data.lastAttackInCombo = false;
                _comboAttacks[i].comboSteps[0].data.firstAttackInCombo = true;
                _comboAttacks[i].comboSteps[0].data.comboIndex = i;
                _comboAttacks[i].comboSteps[0].data.fullCombo = false;
                _comboAttacks[i].comboSteps[0].data.comboStep = 0;

                //GuiManager.Instance.ShowComboProgress(ComboProgress);
                return _comboAttacks[i].comboSteps[0].data;
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

        for (int i = 0; i < _comboAttacks.Count; i++)
        {
            Combo combo = _comboAttacks[i];

            bool valid = true;
            for (int ii = 0; ii < progress.Count; ii++)
            {
                if (progress[ii] != combo.comboSteps[ii].orderAttackType)
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


    public abstract string GetIdleAnim( WeaponType weapon, WeaponState weaponState );
    public abstract string GetIdleActionAnim( WeaponType weapon, WeaponState weaponState );
    public abstract string GetMoveAnim( MotionType motion, MoveType move, WeaponType weapon, WeaponState weaponState );
    public abstract string GetRotateAnim( MotionType motionType, RotationType rotationType );
    public abstract string GetRollAnim( WeaponType weapon, WeaponState weaponState );
    public abstract string GetBlockAnim( BlockState block, WeaponType weapon );
    public abstract string GetKnockdowAnim( KnockdownState block, WeaponType weapon );
    public abstract string GetShowWeaponAnim( WeaponType weapon );
    public abstract string GetHideWeaponAnim( WeaponType weapon );    
    public virtual string GetInjuryPhaseAnim(int phase) { return null; }
    public abstract string GetInjuryAnim( WeaponType weapon, DamageType type );
    public abstract string GetDeathAnim( WeaponType weapon, DamageType type );

    // 以下函数可以删除
    public virtual AnimAttackData GetFirstAttackAnim(WeaponType weapon, OrderAttackType attackType) { return null; }
    public virtual AnimAttackData GetWhirlAttackAnim() { return null; }
    public virtual AnimAttackData GetRollAttackAnim() { return null; }
}
