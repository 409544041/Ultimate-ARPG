﻿using GWLPXL.ARPGCore.Attributes.com;
using GWLPXL.ARPGCore.Combat.com;
using GWLPXL.ARPGCore.GameEvents.com;
using GWLPXL.ARPGCore.Items.com;
using GWLPXL.ARPGCore.Leveling.com;
using GWLPXL.ARPGCore.Statics.com;
using GWLPXL.ARPGCore.Types.com;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace GWLPXL.ARPGCore.com
{


    public class PlayerHealth : MonoBehaviour, IReceiveDamage
    {
        public Action<DamageResults> OnTakeDamage;
        [SerializeField]
        protected PlayerDefault combatHandler = null;
        [SerializeField]
        protected PlayerHealthEvents healthEvents = new PlayerHealthEvents();
        [SerializeField]
        protected ResourceType healthResource = ResourceType.Health;
        [SerializeField]
        protected float iFrameTime = .25f;
        protected bool isDead = false;
        protected bool canBeAttacked = false;
        protected CombatGroupType[] combatGroups = new CombatGroupType[1] { CombatGroupType.Friendly };
        protected IActorHub owner = null;
        protected IActorHub lastCharacterToHitMe;
        protected bool immortal = false;

        #region unity calls
        protected virtual void Awake()
        {
            Setup();
        }
        #endregion

        #region public interfaces
        public bool IsHurt()
        {
            return !canBeAttacked;
        }

        public ResourceType GetHealthResource()
        {
            return healthResource;
        }


        public CombatGroupType[] GetMyCombatGroup()
        {
            return combatGroups;
        }

        public void SetCharacterThatHitMe(IActorHub user)
        {
            lastCharacterToHitMe = user;
        }

        public void SetUser(IActorHub forUser)
        {
            owner = forUser;
        }

        public void SetInvincible(bool isImmoratal) => immortal = isImmoratal;

        public Transform GetInstance()
        {
            return transform;
        }

        public bool IsDead()
        {
            return isDead;
        }

        public void Die()
        {
            DefaultDie();

        }


        /// <summary>
        /// not effected by iframes, only takes into account ele type damage and resist
        /// </summary>
        /// <param name="damageAmount"></param>
        /// <param name="type"></param>
        public void TakeDamage(int damageAmount, ElementType type)
        {
           // DefaultTakeDamage(damageAmount, type);

        }
        /// <summary>
        /// effected by iframes, checks physical and ele damage
        /// </summary>
        /// <param name="damageAmount"></param>
        /// <param name="damageDealer"></param>
        public void TakeDamage(int damageAmount, IActorHub damageDealer)
        {
          //  DefaultTakeActorDamage(damageAmount, damageDealer);

        }
        public void CheckDeath()
        {
            DefautlCheckDeath();
        }
        #endregion

        #region protected virtual
        protected virtual void Setup()
        {
            canBeAttacked = true;
            if (combatHandler == null) combatHandler = ScriptableObject.CreateInstance<PlayerDefault>();
        }

       

        protected virtual void DefaultDie()
        {
            if (isDead) return;

            isDead = true;
            NotifyCustomDeathEvent();
        }

        protected virtual void NotifyCustomDeathEvent()
        {
            healthEvents.SceneEvents.OnDie.Invoke();
            if (healthEvents.GameEvents.DeathEvent == null) return;

            healthEvents.GameEvents.DeathEvent.DiedObj = this.gameObject;
            GameEventHandler.RaiseDeathEvent(healthEvents.GameEvents.DeathEvent);

        }

        protected virtual void NotifyCustomDamageEvent(ElementType type, int eleDmgTaken)
        {
            healthEvents.SceneEvents.OnDamageTaken.Invoke(eleDmgTaken);
            if (healthEvents.GameEvents.TookDamageEvent == null) return;

            healthEvents.GameEvents.TookDamageEvent.EventVars = new DamageEvent(this, eleDmgTaken, type, eleDmgTaken.ToString(), transform.position + Vector3.up * 2f);
            GameEventHandler.RaisePlayerDamageEvent(healthEvents.GameEvents.TookDamageEvent);

        }


       

        protected virtual void DefautlCheckDeath()
        {
            if (owner.MyStats.GetRuntimeAttributes().GetResourceNowValue(ResourceType.Health) <= 0)
            {
                Die();
            }
        }

        protected virtual IEnumerator CanBeAttackedCooldown(float duration)
        {
            canBeAttacked = false;
            yield return new WaitForSeconds(duration);
            canBeAttacked = true;
        }

        public void TakeDamage(AttackValues values)
        {
            if (canBeAttacked == false)
            {
                return;
            }

            values = combatHandler.TakeDamageFormula(values, owner);


            if (immortal == false)
            {
                for (int i = 0; i < values.ElementAttacks.Count; i++)
                {
                    if (values.ElementAttacks[i].Reduced > 0)//prevent dmg if immortal, but show everything else
                    {
                        owner.MyStats.GetRuntimeAttributes().ModifyNowResource(healthResource, -values.ElementAttacks[i].Reduced);
                    }

                }

                for (int i = 0; i < values.PhysicalAttack.Count; i++)
                {
                    if (values.PhysicalAttack[i].PhysicalReduced > 0)
                    {
                        owner.MyStats.GetRuntimeAttributes().ModifyNowResource(healthResource, -values.PhysicalAttack[i].PhysicalReduced);
                    }
                }

            }

            DamageResults d = new DamageResults(values.ElementAttacks, values.PhysicalAttack, this);
            OnTakeDamage?.Invoke(d);
            CombatLogger.AddResult(d);
            NotifyUI(d);
            CheckDeath();
            StartCoroutine(CanBeAttackedCooldown(iFrameTime));//we are invulnerable for a short time
        }
        protected virtual void NotifyUI(DamageResults results)
        {

          //  if (can == null) return;
          //  dungeoncanvas.DamageResults(results);

        }
        public IActorHub GetUser()
        {
            return owner;
        }


        #endregion

    }



}