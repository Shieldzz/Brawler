using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace BTA
{
    public class EnemiesTpEventHandler : MonoBehaviour
    {
        protected Animator m_animator;

        #region Event

        public class StateEvent : UnityEvent<bool> { }
        public StateEvent OnTpStateEvent = new StateEvent();

        #endregion

        protected void Start()
        {
            m_animator = GetComponent<Animator>();
        }

        protected virtual void OnTpStart()
        {
            m_animator.SetBool("Teleporting", true);
            OnTpStateEvent.Invoke(true);
        }

        protected virtual void OnTpEnd()
        {
            m_animator.SetBool("Teleporting", false);
            OnTpStateEvent.Invoke(false);
        }
    }
}