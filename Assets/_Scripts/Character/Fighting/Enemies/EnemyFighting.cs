using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BTA
{

    public abstract class EnemyFighting : AFighting
    {

        protected Enemy m_enemy;
        protected EnemySounds m_sounds;

        protected override void Awake()
        {
            m_enemy = GetComponent<Enemy>();
            m_sounds = GetComponentInChildren<EnemySounds>();
            base.Awake();
            AssignAttackEvent();
        }

        protected override void AssignAttackEvent()
        {
            AttackEventHandler handler = GetComponentInChildren<AttackEventHandler>();

            handler.OnAttackStateEvent.AddListener(m_movement.Freeze);
            handler.OnConnectEvent.AddListener(OnAttackConnect);
            handler.OnRecoveryEvent.AddListener(OnAttackRecovery);
        }
    }
}
