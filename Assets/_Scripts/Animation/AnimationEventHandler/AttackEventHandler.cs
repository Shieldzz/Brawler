using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace BTA
{

    public class AttackEventHandler : MonoBehaviour
    {
        protected Animator m_animator;

        protected AttackType m_attackType;
        protected AttackStruct m_currAttackStruct;

        #region Event

        public class StateEvent : UnityEvent<bool> { }
        public class AttackEvent : UnityEvent<AttackType> { }
        public class ConnectEvent : UnityEvent<AttackType, AttackData, int> { }

        public StateEvent OnAttackStateEvent = new StateEvent();
        public AttackEvent OnAttackBeginEvent = new AttackEvent();
        public AttackEvent OnAttackEndEvent = new AttackEvent();

        public ConnectEvent OnConnectEvent = new ConnectEvent();
        [HideInInspector] public UnityEvent OnRecoveryEvent = new UnityEvent();

        #endregion

        protected virtual void Start()
        {
            m_animator = GetComponent<Animator>();
        }

        protected virtual void OnAttackStart(AttackStruct newStruct)
        {
            //Debug.Log("Start");
            m_attackType = (AttackType)m_animator.GetInteger("AttackType");
            //Debug.Log("Attack type = " + m_attackType);
            m_currAttackStruct = newStruct;

            m_animator.SetBool("Attacking", true);
            OnAttackStateEvent.Invoke(true);
            OnAttackBeginEvent.Invoke(m_attackType);
        }

        protected virtual void OnConnect(int attackID)
        {
            //Debug.Log("Connect");
            AttackData currData = m_currAttackStruct.attackDatas[attackID];


            //currData.ID = Random.Range(-100000000, 100000000);
            //Debug.Log("ID = " + currData.ID);

            OnConnectEvent.Invoke(m_attackType, currData, 0);
        }

        protected virtual void OnRecovery()
        {
             //Debug.Log("Recovery");

            OnRecoveryEvent.Invoke();
        }

        //need to proc when attack is cut too :x 
        protected virtual void OnAttackEnd()
        {
            //Debug.Log("End");

            m_animator.SetBool("Attacking", false);
            OnAttackStateEvent.Invoke(false);
            OnAttackEndEvent.Invoke(m_attackType);
        }
    }
}
