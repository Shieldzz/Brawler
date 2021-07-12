using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace BTA
{
    //[Serializable]
    public class PlayerCacFighting : PlayerFighting
    {
        private CacWeaponTrigger m_attackTrigger = null;

        // Use this for initialization
        protected override void Awake()
        {
            base.Awake();
            m_attackTrigger = GetComponentInChildren<CacWeaponTrigger>();
            m_attackTrigger.OnHitEvent.AddListener(OnSuccessfulHit);
        }

        protected override void AttackConnect(AttackType type, AttackData data)
        {
            switch (type)
            {
                // Basic Attack
                case AttackType.LightAttack:
                    {
                        m_attackTrigger.Enable(data);
                        //m_sounds.LaunchLightAttackSound();
                        break;
                    }
                case AttackType.HeavyAttack:
                    {
                        m_attackTrigger.Enable(data);
                        //m_sounds.LaunchHeavyAttackSound();
                        break;
                    }
                //Special Attack
                case AttackType.SpecialAttack:
                    {
                        m_specialAttackTrigger.Enable(data);
                        m_sounds.LaunchSpecialAttackSound();
                        break;
                    }
                default:
                    break;
            }
        }

        protected override void AttackRecovery(AttackType type)
        {
            switch (type)
            {
                // Basic Attack
                case AttackType.LightAttack:
                case AttackType.HeavyAttack:
                    m_attackTrigger.Disable();
                    break;

                // Special Attack
                case AttackType.SpecialAttack:
                    m_specialAttackTrigger.Disable();
                    break;
                default:
                    break;
            }
        }
    }
}