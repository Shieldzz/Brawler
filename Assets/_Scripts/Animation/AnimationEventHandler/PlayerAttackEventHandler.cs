using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace BTA
{

    public class PlayerAttackEventHandler : AttackEventHandler
    {

        PlayerFighting m_fighting;

        int m_currComboLength = 0;

        SpecialLevel m_currSpecialLevel;

        #region Event

        public class SpecialAttackEvent : UnityEvent<float> { }
        public SpecialAttackEvent OnSpecialAttackEvent = new SpecialAttackEvent();

        public StateEvent OnRecoveryStateEvent = new StateEvent();

        #endregion

        protected override void Start()
        {
            base.Start();
            m_fighting = GetComponentInParent<PlayerFighting>();
            m_currComboLength = m_animator.GetInteger("ComboLength");
        }

        protected override void OnAttackStart(AttackStruct newStruct)
        {
            //Debug.Log("Start");

            if (m_animator.GetBool("Recovering"))
                OnAttackEnd();

            base.OnAttackStart(newStruct);
        }

        protected override void OnConnect(int attackID)
        {
            //Debug.Log("Connect");
            base.OnConnect(attackID);
        }

        protected override void OnRecovery()
        {
           // Debug.Log("Recovery");
            ComboAttackStruct currComboAttack = (ComboAttackStruct)m_currAttackStruct;
            m_animator.SetFloat("PursueComboTimer", currComboAttack.pursueComboTimer);
            m_animator.SetTrigger("Comboing");
            IncrementIntParameter("ComboLength");

            m_animator.SetBool("Recovering", true);
            OnRecoveryStateEvent.Invoke(true);
            base.OnRecovery();
        }

        //need to proc when attack is cut too :x 
        protected override void OnAttackEnd()
        {
            //Debug.Log("End");
            m_animator.SetBool("Recovering", false);
            OnRecoveryStateEvent.Invoke(false);
            base.OnAttackEnd();
        }

        private void OnSpecialStart(SpecialAttack specialStruct)
        {
            int currSpecialLevel = m_fighting.GetChainGaugeLevel() - 1; // cause of level 0

            m_currSpecialLevel = specialStruct.SpecialLevels[currSpecialLevel];
            m_currAttackStruct = m_currSpecialLevel.atkStruct;

            OnSpecialAttackEvent.Invoke(m_currSpecialLevel.range);
            m_animator.speed = 1f / m_currSpecialLevel.animationDuration;

            OnAttackStart(m_currSpecialLevel.atkStruct);

            StartCoroutine(ManageSpecial());
        }

        private void OnSpecialRecovery()
        {
            //Debug.Log("Spe Recovery");

            m_animator.ResetTrigger("Comboing");
            m_animator.SetFloat("PursueComboTimer", 0f);
            m_animator.SetInteger("ComboLength", 0);
            m_animator.speed = 1f;

            OnRecoveryEvent.Invoke();
        }

        IEnumerator ManageSpecial()
        {
            yield return new WaitForSeconds(m_currSpecialLevel.startUpDuration);

            for (int id = 0; id < m_currAttackStruct.attackDatas.Length; id++)
            {
               Debug.Log("Special Connect");
                OnConnect(id);
                yield return new WaitForSeconds(m_currSpecialLevel.connectDuration);
            }

            //Debug.Log("Special Recovery");
            OnSpecialRecovery();

            yield return new WaitForEndOfFrame();
        }

        private void IncrementIntParameter(string paramName)
        {
            int hashKey = Animator.StringToHash(paramName);
            m_animator.SetInteger(hashKey, m_animator.GetInteger(hashKey) + 1);
        }
    }
}
