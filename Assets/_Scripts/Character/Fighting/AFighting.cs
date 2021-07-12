using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BTA
{

    public abstract class AFighting : MonoBehaviour
    {
        protected BasicMovement m_movement;
        protected PositionDetector m_positionDetector;
        protected Animator m_animator;

        protected AttackType m_currUsedAttack = AttackType.None;

        // Use this for initialization
        protected virtual void Awake()
        {
            m_movement = GetComponent<BasicMovement>();
            m_positionDetector = GetComponent<PositionDetector>();
            m_animator = GetComponentInChildren<Animator>();

            m_movement.OnAttackInterrupt.AddListener(OnAttackInterrupt);
        }

        #region Attack

        protected abstract void AssignAttackEvent();

        protected virtual void OnAttackConnect(AttackType type, AttackData data, int comboLength) // maybe add force and duration ? & recup ComboLength here !
        {
            m_currUsedAttack = type;

            //Abstract
            AttackConnect(type, data);
        }

        protected virtual void OnAttackRecovery()
        {
            //Abstract
            AttackRecovery(m_currUsedAttack);

            m_currUsedAttack = AttackType.None;
        }

        protected virtual void OnAttackInterrupt()
        {
            OnAttackRecovery();
            m_animator.SetBool("Attacking", false);
        }

        public virtual void AttackInput(AttackType type)
        {
            m_animator.SetInteger("AttackType", (int)type);
            m_animator.SetTrigger("InputAttack");
        }

        protected abstract void AttackConnect(AttackType type, AttackData data);
        protected abstract void AttackRecovery(AttackType type);
        #endregion
    }
}
