using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BTA
{
    public class EnemyCacFighting : EnemyFighting
    {
        private CacWeaponTrigger m_attackTrigger = null;

        // Use this for initialization
        protected override void Awake()
        {
            base.Awake();
            m_attackTrigger = GetComponentInChildren<CacWeaponTrigger>();
        }

        protected override void AttackConnect(AttackType type, AttackData data)
        {
            switch (type)
            {
                // Basic Attack
                case AttackType.LightAttack:
                    {
                        m_attackTrigger.Enable(data);
                        m_sounds.LaunchAttackSound();
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
                    m_attackTrigger.Disable();
                    break;
                default:
                    break;
            }
        }
    }
}

