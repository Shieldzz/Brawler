using System;
using UnityEngine;
using UnityEngine.Events;

namespace BTA
{

    public class Damageable : MonoBehaviour
    {
        [SerializeField] Animator m_animator;

        [Serializable]
        public class DamageEvent : UnityEvent<AttackTrigger, Damageable>
        { }

        public DamageEvent OnTakeDamage;
        public DamageEvent OnDieEvent;

        [Serializable]
        public class HealthEvent : UnityEvent<Damageable>
        { }

        public HealthEvent OnHeal;

        private bool m_isInvincible = false;
        private float m_invicibilityTimer;
        private AttackTrigger m_lastDamager;

        [SerializeField] private float m_maxLife = 100f;
        private float m_currentLife = 0f;
        [SerializeField] private float m_invincibilityDuration = 0.5f;

        public float MaxLife { get { return m_maxLife; } set { m_maxLife = value; } }
        public float Life { get { return m_currentLife; } set { m_currentLife = value; } }


        private float m_timerLastID = 0.5f;
        private float m_currentTimerLastID = 0f;
        private int m_lastID = 0;

        void Awake()
        {
            m_currentLife = m_maxLife;
        }

        void Update()
        {
            if (m_currentTimerLastID > 0)
            {
                m_currentTimerLastID -= Time.deltaTime;
                if (m_currentTimerLastID <= 0f)
                    m_lastID = -1;
            }

            //TODO: change invicibility to coroutine
            if (m_isInvincible)
            {
                m_invicibilityTimer -= Time.deltaTime;
                if (m_invicibilityTimer <= 0f)
                    DisableInvincibility();
            }
        }

        /**
         *  parameter {Damager} damager : damager class that inflict damage
         *  parameter {int} amout : amout of damage
         *  return {bool}
         *  Inflict damage to the owner class.
         */
        public bool TakeDamage(AttackTrigger trigger, float amout, int id)
        {
            if (!IsAlive() || m_isInvincible || id == m_lastID)
                return false;

            m_currentLife -= amout;
            m_lastDamager = trigger;
            OnTakeDamage.Invoke(trigger, this);

            id = m_lastID;
            m_currentTimerLastID = m_timerLastID;

            if (!IsAlive())
            {
                Die();
                return true;
            }
            else if (m_animator)
                m_animator.SetTrigger("Hit");

            EnableInvincibility();

            return true;
        }

        public void GainHealth(float amout)
        {
            m_currentLife += amout;
            OnHeal.Invoke(this);
        }

        public bool IsAlive()
        {
            return m_currentLife > 0f;
        }

        private void Die()
        {
            Debug.Log("Die");
            OnDieEvent.Invoke(m_lastDamager, this);
            //TODO: Raise event, or something else
        }

        private void EnableInvincibility()
        {
            m_invicibilityTimer = m_invincibilityDuration;
            m_isInvincible = true;
        }

        private void DisableInvincibility()
        {
            m_isInvincible = false;
        }
    }
}
