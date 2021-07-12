using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


namespace BTA
{
    public enum CharacterState
    {
        Idle = 0,
        Walk,
        Attack,
        Dash,
        Jump,
        Fall,
        Stagger,
        Death,
        Init // = to spawn state ??? useful or not ? 
    }

    public abstract class ACharacter : Entity
    {
        protected bool paused;

        protected CharacterState m_currState = CharacterState.Idle;

        protected Animator m_animator;
        public Animator Animator { get { return m_animator; } }

        //#region Events

        //public class StateEvent : UnityEvent<bool> { }

        //public StateEvent OnInitState = new StateEvent();
        //public StateEvent OnWalkState = new StateEvent();
        //public StateEvent OnState = new StateEvent();
        //public StateEvent OnInitState = new StateEvent();
        //public StateEvent OnInitState = new StateEvent();
        //public StateEvent OnInitState = new StateEvent();
        //public StateEvent OnInitState = new StateEvent();
        //public StateEvent OnInitState = new StateEvent();
        //public StateEvent OnInitState = new StateEvent();
        //public StateEvent OnInitState = new StateEvent();

        //#endregion

        // Use this for initialization
        void Awake()
        {
            //m_animator.GetComponentInChildren<Animator>();

        }

        // Update is called once per frame
        void Update()
        {
        }

        void SwitchStateTo(CharacterState nextState)
        {
            if (m_currState == nextState)
                return;

            EnableState(m_currState, false);
            EnableState(nextState, true);

            m_currState = nextState;
        }

        void EnableState(CharacterState state, bool enable)
        {
            switch (state)
            {
                case CharacterState.Idle:
                    OnIdleState(enable);
                    break;
                case CharacterState.Walk:
                    OnWalkState(enable);
                    break;
                case CharacterState.Attack:
                    OnAttackState(enable);
                    break;
                case CharacterState.Dash:
                    OnDashState(enable);
                    break;
                case CharacterState.Jump:
                    OnJumpState(enable);
                    break;
                case CharacterState.Fall:
                    OnFallState(enable);
                    break;
                case CharacterState.Stagger:
                    OnStaggerState(enable);
                    break;
                case CharacterState.Death:
                    OnDeathState(enable);
                    break;
                case CharacterState.Init:
                    OnInitState(enable);
                    break;
                default:
                    break;
            }
        }

        virtual protected void OnIdleState(bool enable) { }
        virtual protected void OnWalkState(bool enable) { }
        virtual protected void OnAttackState(bool enable) { }
        virtual protected void OnFallState(bool enable) { }
        virtual protected void OnStaggerState(bool enable) { }
        virtual protected void OnDeathState(bool enable) { }
        virtual protected void OnInitState(bool enable) { }

        virtual protected void OnDashState(bool enable) { }
        virtual protected void OnJumpState(bool enable) { }
    }
}