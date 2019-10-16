using UnityEngine;
using Phenix.Unity.Utilities;

public class AttackMeleeHitData
{
    public Agent1 agent;
    public Agent1 target;
    public bool isCritical;
    public bool isKnockDown;
    public AnimAttackData attackData;
}

public class AttackWhirlHitData
{
    public Agent1 agent;
    public Agent1 target;
    public AnimAttackData attackData;
}

public class HandleAttackResult
{
    public static void DoMeleeDamage(Agent1 agent, Agent1 mainTarget, WeaponType weaponType, 
        AnimAttackData animAttackData, bool critical, bool knockdown, bool fatalAttack)
    {
        Vector3 dirToEnemy;        
        Vector3 attackerDir = agent.Forward;

        for (int i = 0; i < Game1.Instance.AgentMgr.agents.Count; i++)
        {
            Agent1 target = Game1.Instance.AgentMgr.agents[i];           
            if (target == null || target == agent || 
                target.BlackBoard.IsAlive == false || target.BlackBoard.IsKnockedDown)
            {
                continue;
            }

            if (fatalAttack)
            {
                // 一招制敌，如玩家的跳杀跌倒的敌人技能
                ReceiveDamage(target, agent, weaponType, target.BlackBoard.maxHealth, animAttackData.hitMomentum);
                continue;
            }

            dirToEnemy = target.Position - agent.Position;
            float dist = dirToEnemy.magnitude;
            dirToEnemy.Normalize();
            
            if (target.BlackBoard.invulnerable || 
                (target.BlackBoard.damageOnlyFromBack && Vector3.Angle(attackerDir, target.Forward) > 80))
            {
                if (dist <= agent.BlackBoard.weaponRange)
                {
                    // 攻击无效
                    ReceiveHitCompletelyBlocked(target, agent);
                }                
                continue;
            }

            if (dist > agent.BlackBoard.weaponRange)
            {
                if (animAttackData.hitAreaKnockdown && knockdown && dist < agent.BlackBoard.weaponRange * 1.2f)
                {
                    // 击倒                    
                    ReceiveKnockDown(target, agent, dirToEnemy * animAttackData.hitMomentum);                    
                }
                else if (animAttackData.useImpuls && dist < agent.BlackBoard.weaponRange * 1.4f)
                {
                    // 击退
                    ReceiveImpuls(target, agent, dirToEnemy * animAttackData.hitMomentum);
                }
                continue;
            }

            if (dist > 0.5f && Vector3.Angle(attackerDir, dirToEnemy) > animAttackData.hitAngle)
            {
                if (animAttackData.useImpuls)
                {
                    // 击退
                    ReceiveImpuls(target, agent, dirToEnemy * animAttackData.hitMomentum);
                }
                continue;
            }

            if (target.BlackBoard.criticalAllowed && 
                animAttackData.hitCriticalType != CriticalHitType.NONE &&
                agent.BlackBoard.FaceToOtherBack(target)) // from behind            
            {
                // 碎尸
                ReceiveCriticalHit(target, agent, animAttackData.hitCriticalType, false);                
            }
            else if (target.BlackBoard.IsBlocking)            
            {
                // 被格挡
                ReceiveBlockedHit(target, agent, weaponType, animAttackData.hitDamage, animAttackData);                
            }
            else if (target.BlackBoard.criticalAllowed && 
                    critical &&
                    (mainTarget == target || 
                    (animAttackData.hitCriticalType == CriticalHitType.HORIZONTAL && Random.Range(0, 100) < 30)))
            {
                // 碎尸
                ReceiveCriticalHit(target, agent, animAttackData.hitCriticalType, false);                
            }
            else if (animAttackData.hitAreaKnockdown && knockdown)            
            {
                // 击倒
                ReceiveKnockDown(target, agent, dirToEnemy * 
                    (1 - (dist / agent.BlackBoard.weaponRange) + animAttackData.hitMomentum));                
            }
            else if (animAttackData.hitAngle == -1/*比如attack whirl*/ || Vector3.Angle(attackerDir, dirToEnemy) < animAttackData.hitAngle)
            {
                // 普通伤害
                ReceiveDamage(target, agent, weaponType, animAttackData.hitDamage, animAttackData.hitMomentum);                
            }
            else
            {                
                // miss                
                agent.SoundComponent.SoundMgr.PlayOneShot((int)SoundType.ATTACK_MISS);
            }
        }        
    }

    static void ReceiveDamage(Agent1 agent, Agent1 attacker, WeaponType byWeapon, float damage, float hitMomentum)
    {
        if (agent.BlackBoard.IsAlive == false)
            return;

        if (attacker.IsPlayer)
        {
            //Game.Instance.Hits += 1; 连击次数            
        }

        agent.BlackBoard.Attacker = attacker;
        agent.BlackBoard.attackerWeapon = byWeapon;
        agent.BlackBoard.impuls = attacker.Forward * hitMomentum;

        if (agent.BlackBoard.IsKnockedDown)
        {
            agent.BlackBoard.health = 0;
            agent.BlackBoard.damageType = DamageType.IN_KNOCK_DOWN;            
            agent.Decision.OnDead();
                        
            attacker.SoundComponent.SoundMgr.PlayOneShot((int)SoundType.ATTACK_FATAL);

            if (attacker.IsPlayer)
            {
                //Game.Instance.Score += Experience;
                //Player.Instance.AddExperience(Experience, 1.5f + Game.Instance.Hits * 0.1f);
            }
        }
        else
        {
            agent.BlackBoard.health = Mathf.Max(0, agent.BlackBoard.health - damage);
            agent.BlackBoard.damageType = attacker.BlackBoard.FaceToOtherBack(agent) ? DamageType.BACK : DamageType.FRONT;

            if (agent.BlackBoard.IsAlive)
            {
                agent.Decision.OnInjury();
                
                SpriteComponent.Instance.CreateSprite(SpriteType.BLOOD,
                    new Vector3(agent.transform.localPosition.x, agent.transform.localPosition.y + 0.5f,
                    agent.transform.localPosition.z), new Vector3(90, Random.Range(0, 180), 0));
            }
            else
            {
                agent.Decision.OnDead();
                
                if (attacker.IsPlayer)
                {
                    //Game.Instance.Score += Experience;
                    //Player.Instance.AddExperience(Experience, 1 + Game.Instance.Hits * 0.1f);
                }
            }

            if (damage >= 15)
            {                
                ParticleComponent.Instance.Play(ParticleType.BIG_BLOOD_AND_HIT_BLINK, 
                    agent.Transform.position, -attacker.Forward); ;
            }
            else
            {                
                ParticleComponent.Instance.Play(ParticleType.BLOOD_AND_HIT_BLINK, 
                    agent.Transform.position, -attacker.Forward); ;
            }

            attacker.SoundComponent.SoundMgr.PlayOneShot((int)SoundType.ATTACK_HIT);
        }
    }

    static void ReceiveHitCompletelyBlocked(Agent1 agent, Agent1 attacker)
    {
        //CombatEffectMgr.Instance.PlayBlockHitEffect(agent.ChestPosition, -attacker.Forward);
        ParticleComponent.Instance.Play(ParticleType.BLOCK_SUCCESS, agent.ChestPosition, -attacker.Forward);
        /*BlackBoard.Berserk += BlackBoard.BerserkBlockModificator;
        BlackBoard.Rage += BlackBoard.RageBlockModificator;
        if (attacker.IsPlayer)
            Game.Instance.NumberOfBlockedHits++;*/
        attacker.SoundComponent.SoundMgr.PlayOneShot((int)SoundType.ATTACK_BLOCK);
    }

    static void ReceiveBlockedHit(Agent1 agent, Agent1 attacker, WeaponType byWeapon, 
        float damage, AnimAttackData data)
    {
        agent.BlackBoard.Attacker = attacker;
        agent.BlackBoard.attackerWeapon = byWeapon;
        //_goapMgr.WorldState.SetWSProperty(WorldStatePropKey.EVENT, EventTypes.BLOCK_BROKEN);

        bool fromBehind = Vector3.Dot(attacker.Forward, agent.Forward) > -0.1f;
        if (fromBehind)
        {
            //agent.BlackBoard.blockResult = BlockResult.FAIL;
            ReceiveDamage(agent, attacker, byWeapon, damage, data.hitMomentum);// block失败（扣血）
            //agent.BlackBoard.health = Mathf.Max(1, agent.BlackBoard.health - damage);
            //agent.BlackBoard.damageType = DamageType.BACK;
            //CombatEffectMgr.Instance.PlayBloodEffect(agent.Transform.position, -attacker.Forward);
            ParticleComponent.Instance.Play(ParticleType.BLOOD_AND_HIT_BLINK, agent.Transform.position, -attacker.Forward); ;
            //SpriteEffectsManager.Instance.CreateBlood(Transform);
            SpriteComponent.Instance.CreateSprite(SpriteType.BLOOD, 
                new Vector3(agent.transform.localPosition.x, agent.transform.localPosition.y + 0.5f,
                    agent.transform.localPosition.z), 
                new Vector3(90, Random.Range(0, 180), 0));

            attacker.SoundComponent.SoundMgr.PlayOneShot((int)SoundType.ATTACK_HIT);
        }
        else
        {
            // 如果满足破防几率
            if (Random.Range(0, 100) < agent.BlackBoard.breakBlockChance) 
            {
                // block失败（但不扣血）                
                //agent.BlackBoard.blockResult = BlockResult.FAIL;
                AnimFSMEventBreakBlock eventBreakBlock = AnimFSMEventBreakBlock.pool.Get();
                eventBreakBlock.attacker = attacker;
                eventBreakBlock.attackerWeapon = byWeapon;
                eventBreakBlock.success = false;
                agent.FSMComponent.SendEvent(eventBreakBlock);
                //if (attacker.isPlayer)
                //  Game.Instance.NumberOfBreakBlocks++;
                //CombatEffectsManager.Instance.PlayBlockBreakEffect(Transform.position, -attacker.Forward);

                attacker.SoundComponent.SoundMgr.PlayOneShot((int)SoundType.ATTACK_BLOCK);
            }
            else
            {
                // block成功
                //agent.BlackBoard.blockResult = BlockResult.SUCCESS;                
                AnimFSMEventBreakBlock eventBreakBlock = AnimFSMEventBreakBlock.pool.Get();
                eventBreakBlock.attacker = attacker;
                eventBreakBlock.attackerWeapon = byWeapon;
                eventBreakBlock.success = true;
                agent.FSMComponent.SendEvent(eventBreakBlock);
                //if (attacker.isPlayer)
                //  Game.Instance.NumberOfBlockedHits++;
                //CombatEffectMgr.Instance.PlayBlockHitEffect(agent.ChestPosition, -attacker.Forward);
                ParticleComponent.Instance.Play(ParticleType.BLOCK_SUCCESS, agent.ChestPosition, -attacker.Forward);
                attacker.SoundComponent.SoundMgr.PlayOneShot((int)SoundType.ATTACK_BLOCK);
            }
        }
    }

    static void ReceiveImpuls(Agent1 agent, Agent1 attacker, Vector3 impuls)
    {
        agent.BlackBoard.Attacker = attacker;
        agent.BlackBoard.attackerWeapon = WeaponType.NONE;
        agent.BlackBoard.impuls = impuls;
        agent.BlackBoard.damageType = attacker.BlackBoard.FaceToOtherBack(agent) ? DamageType.BACK : DamageType.FRONT;
        agent.Decision.OnInjury();
    }

    static void ReceiveKnockDown(Agent1 agent, Agent1 attacker, Vector3 impuls)
    {
        if (agent.BlackBoard.IsAlive == false || agent.BlackBoard.knockDownAllowed == false)
            return;

        if (attacker.IsPlayer)
        {
            //Game.Instance.Hits += 1;
            //Game.Instance.NumberOfKnockdowns++;
        }

        agent.BlackBoard.Attacker = attacker;
        agent.BlackBoard.impuls = impuls;

        agent.Decision.OnKnockDown();
        //CombatEffectsManager.Instance.PlayKnockdownEffect(Transform.position, -attacker.Forward);
        attacker.SoundComponent.SoundMgr.PlayOneShot((int)SoundType.ATTACK_KNOCK_DOWN);
    }

    static void ReceiveCriticalHit(Agent1 agent, Agent1 attacker, CriticalHitType type, bool effectOnly/* = false*/)
    {
        if (attacker.IsPlayer)
        {
            //Game.Instance.Hits += 1;
            //Game.Instance.Score += Experience;
            //Player.Instance.AddExperience(Experience, 1.5f + Game.Instance.Hits * 0.1f);
            //Game.Instance.NumberOfCriticals++;
        }

        //BlackBoard.stop = true;
        agent.BlackBoard.health = 0;

        if (type == CriticalHitType.HORIZONTAL)
        {
            int r = Random.Range(0, 100);
            if (r < 33)
                ChoppedBodyMgr1.Instance.ShowChoppedBody(agent.agentType, agent.Transform, ChoppedBodyType1.LEGS);
            else if (r < 66)
                ChoppedBodyMgr1.Instance.ShowChoppedBody(agent.agentType, agent.Transform, ChoppedBodyType1.BEHEADED);
            else
                ChoppedBodyMgr1.Instance.ShowChoppedBody(agent.agentType, agent.Transform, ChoppedBodyType1.HALF_BODY);
        }
        else
        {
            float dot = Vector3.Dot(agent.Forward, attacker.Forward);

            if (dot < 0.5 && dot > -0.5f)
                ChoppedBodyMgr1.Instance.ShowChoppedBody(agent.agentType, agent.Transform, ChoppedBodyType1.SLICE_LEFT_RIGHT);
            else
                ChoppedBodyMgr1.Instance.ShowChoppedBody(agent.agentType, agent.Transform, ChoppedBodyType1.SLICE_FRONT_BACK);
        }

        attacker.SoundComponent.SoundMgr.PlayOneShot((int)SoundType.ATTACK_HIT);

        //CombatEffectsManager.Instance.PlayCriticalEffect(Transform.position, -attacker.Forward);
        //Mission.Instance.ReturnHuman(hitAgent.gameObject);
        agent.gameObject.SetActive(false); // 临时这么写着
    }

    public static void DoRollDamage(Agent1 agent, AnimAttackData data, float range)
    {
        if (agent == null || data == null)
        {
            return;
        }
        if (agent.BlackBoard.motionType != MotionType.ROLL)
        {
            return;
        }        
                
        Vector3 attackerDir = agent.Forward;
        Vector3 dirToTarget = Vector3.zero;

        for (int i = 0; i < Game1.Instance.AgentMgr.agents.Count; i++)
        {
            Agent1 target = Game1.Instance.AgentMgr.agents[i];
            if (target == null || target == agent ||
                target.BlackBoard.IsAlive == false || target.BlackBoard.IsKnockedDown)
            {
                continue;
            }

            if (target.BlackBoard.invulnerable ||
                (target.BlackBoard.damageOnlyFromBack && TransformTools.Instance.IsBehindTarget(agent.Transform, target.Transform) == false))
            {
                // 攻击无效
                ReceiveHitCompletelyBlocked(target, agent);                
                continue;
            }

            dirToTarget = target.Position - agent.Position;

            if (dirToTarget.sqrMagnitude < range * range)
            {
                //if (data.useImpuls)
                //{
                //    ReceiveImpuls(target, agent, dirToTarget.normalized * data.hitMomentum);
                //}                
                ReceiveDamage(target, agent, WeaponType.BODY, data.hitDamage, data.hitMomentum);                
            }
        }   
    }

    public static bool DoRangeDamage(Agent1 agent, Transform arrowTransform, float damage, float hitMomentum)
    {
        foreach (var target in Game1.Instance.AgentMgr.agents)
        {
            if (agent == target)
            {
                continue;
            }

            if (target.BlackBoard.invulnerable)
            {
                continue;
            }

            Vector3 pos1 = arrowTransform.position;
            pos1.y = 0;
            Vector3 pos2 = target.Transform.position;
            pos2.y = 0;

            if ((pos1 - pos2).sqrMagnitude < 0.4 * 0.4f)
            {
                ReceiveDamage(target, agent, WeaponType.BOW, damage, hitMomentum);
                return true;
            }
        }

        return false;
    }

    /*
     public void ReceiveRangeDamage(Agent attacker, float damage, Vector3 impuls)
    {
        BlackBoard.DamageType = E_DamageType.Front;
        BlackBoard.Attacker = attacker;
        BlackBoard.AttackerWeapon = E_WeaponType.Bow;
        BlackBoard.Impuls = impuls;

        if (Game.Instance.GameDifficulty == E_GameDifficulty.Easy)
            damage *= 0.8f;

        if (Game.Instance.DisabledState > 0)
            damage *= 5;

        BlackBoard.Health = Mathf.Max(0, BlackBoard.Health - damage);

        Fact f = FactsFactory.Create(Fact.E_FactType.E_EVENT);
        f.Belief = 1;
        f.EventType = E_EventTypes.Hit;
        Memory.AddFact(f);

        if (IsAlive)
        {
            WorldState.SetWSProperty(E_PropKey.E_EVENT, E_EventTypes.Hit);
            SpriteEffectsManager.Instance.CreateBlood(Transform);
        }
        else
        {
            WorldState.SetWSProperty(E_PropKey.E_EVENT, E_EventTypes.Died);
            StartCoroutine(Fadeout(15));
        }

        CombatEffectsManager.Instance.PlayBloodEffect(Transform.position, impuls);
    }

    public void ReceiveEnviromentDamage(float damage, Vector3 impuls)
    {
        if (Game.Instance.GameDifficulty == E_GameDifficulty.Easy)
            damage *= 0.5f;

        BlackBoard.DamageType = E_DamageType.Enviroment;
        BlackBoard.Attacker = null;
        BlackBoard.AttackerWeapon =  E_WeaponType.None;
        BlackBoard.Impuls = impuls;

        BlackBoard.Health = Mathf.Max(0, BlackBoard.Health - damage);

        Fact f = FactsFactory.Create(Fact.E_FactType.E_EVENT);
        f.Belief = 1;
        f.EventType = E_EventTypes.Hit;
        Memory.AddFact(f);

        if (IsPlayer)
            CameraBehaviour.Instance.PlayCameraAnim("shakeCombo3", false, false);

        if (IsAlive)
        {
            WorldState.SetWSProperty(E_PropKey.E_EVENT, E_EventTypes.Hit);
            SpriteEffectsManager.Instance.CreateBlood(Transform);
        }
        else
        {
            WorldState.SetWSProperty(E_PropKey.E_EVENT, E_EventTypes.Died);
            StartCoroutine(Fadeout(15));
        }

        CombatEffectsManager.Instance.PlayBloodEffect(Transform.position, impuls);

    }
         */

    //public static void AttackMeleeHitHandler(AttackMeleeHitData hitData)
    //{// 考虑群伤EnemiesRecvDamage
    //if (BlackBoard.isPlayer && AnimAttackData.FullCombo)
    //  GuiManager.Instance.ShowComboMessage(AnimAttackData.ComboIndex);              

    /*      if (_eventAttackMelee.attackType == AttackType.Fatality)
              Agent.DoDamageFatality(_eventAttackMelee.target, Agent.BlackBoard.weaponSelected,
                  _eventAttackMelee.data);
          else
              Agent.DoMeleeDamage(_eventAttackMelee.target, Agent.BlackBoard.weaponSelected,
                  _eventAttackMelee.data, _isCritical, _knockdown);   */

    //if (_attackAction.data.lastAttackInCombo || _attackAction.data.comboStep == 3)
    //  CameraBehaviour.Instance.ComboShake(_attackAction.data.comboStep - 3);

    /*if (AnimAttackData.LastAttackInCombo)
        Owner.StartCoroutine(ShowTrail(AnimAttackData, 1, 0.3f, Critical, MoveTime - Time.timeSinceLevelLoad));
    else
        Owner.StartCoroutine(ShowTrail(AnimAttackData, 2, 0.1f, Critical, MoveTime - Time.timeSinceLevelLoad));*/
    //}

    //public static void AttackWhirlHitHandler(AttackWhirlHitData hitData)
    //{
    // 考虑群伤EnemiesRecvDamage
    /*
    if (hitData.target != null && hitData.target.BlackBoard.IsAlive && 
        hitData.target.BlackBoard.motionType != MotionType.ROLL &&
           hitData.target.BlackBoard.invulnerable == false)
    {
        if ((hitData.target.Position - hitData.agent.Position).sqrMagnitude <
            hitData.agent.BlackBoard.weaponRange * hitData.agent.BlackBoard.weaponRange)
        {
            hitData.target.ReceiveDamage(hitData.agent, WeaponType.Body, hitData.attackData.hitDamage, hitData.attackData);
            //Owner.SoundPlayHit();
        }
    }*/
    //}
}