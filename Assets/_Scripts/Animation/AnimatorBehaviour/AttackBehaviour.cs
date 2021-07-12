using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace BTA
{
    public class AttackBehaviour : StateMachineBehaviour
    {
        //Animator m_animator;
        //AnimatorStateInfo m_stateInfo;
        //bool m_recoveryHasEnded = false;
        //bool m_recoveryPassed = false;

        //#region Event

        //public class StateEvent : UnityEvent<bool> { }
        //public class ConnectEvent : UnityEvent<AttackType, AttackData, int> { }

        //public StateEvent OnAttackStateEvent = new StateEvent();
        //public StateEvent OnRecoveryStateEvent = new StateEvent();

        //public ConnectEvent OnConnectEvent = new ConnectEvent();
        //[HideInInspector] public UnityEvent OnRecoveryEvent = new UnityEvent();
        //#endregion

        //[SerializeField] ComboAttackStruct m_comboAttackStruct;

        //float m_startUpTimer = 0f;
        //float m_connectTimer = 0f;
        //float m_recoveryTimer = 0F;

        //float m_pursueComboTimer = 0f;

        //int m_currattackID = 0;
        //int m_currComboLength = 0;

        //// OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
        //override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        //{
        //    //Debug.Log("State Enter");
        //    m_animator = animator;
        //    m_stateInfo = stateInfo;
        //    m_recoveryHasEnded = false; 
        //    m_recoveryPassed = false;
        //    m_currattackID = 0;

        //    m_currComboLength = animator.GetInteger("ComboLength");

        //    //Init
        //    m_startUpTimer = m_comboAttackStruct.startUpDuration / m_stateInfo.speed;
        //    m_connectTimer = 0f;
        //    m_recoveryTimer = 0F;

        //    m_animator.SetBool("Attacking", true);
        //    //Debug.Log("Attack !");
        //    OnAttackStateEvent.Invoke(true);
        //}

        //// OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
        //override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        //{
        //    if (m_pursueComboTimer >= 0f && m_recoveryPassed)
        //    {
        //        m_pursueComboTimer -= Time.deltaTime;
        //        if (m_pursueComboTimer <= 0f)
        //            m_animator.SetFloat("PursueComboTimer", m_pursueComboTimer);
        //    }

        //    if (m_startUpTimer >= 0f)
        //    {
        //        m_startUpTimer -= Time.deltaTime;
        //        if (m_startUpTimer <= 0f)
        //            StartConnect(0);
        //    }

        //    else if (m_connectTimer >= 0f)
        //    {
        //        m_connectTimer -= Time.deltaTime;
        //        if (m_connectTimer <= 0f)
        //            EndConnect();
        //    }

        //    // What the point :x 
        //    else if (m_recoveryTimer >= 0f)
        //    {
        //        m_recoveryTimer -= Time.deltaTime;
        //        if (m_recoveryTimer <= 0f)
        //        {
        //            //Debug.Log("Time out Recovery =>" + m_recoveryHasEnded);
        //            EndRecovery();
        //        }
        //    }
        //}

        //// OnStateExit is called when a transition ends and the state machine finishes evaluating this state
        //override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        //{
        //    //Debug.Log("Exit Recovery =>" + m_recoveryHasEnded);
        //    EndRecovery();
        //    //OnAttackStateEvent.Invoke(false);
        //}


        //private void StartConnect(int id)
        //{
        //    //Determine an ID -> * 10 ensure that multihit doesn't break 
        //    //Create ID to avoid multiple hit on a same attack
        //    m_comboAttackStruct.attackDatas[id].ID = (GetInstanceID() * 10) - id;

        //    OnConnectEvent.Invoke(m_comboAttackStruct.type, m_comboAttackStruct.attackDatas[id], m_currComboLength);
        //    m_connectTimer = m_comboAttackStruct.attackDatas[id].connectDuration / m_stateInfo.speed;
        //}

        //private void EndConnect()
        //{
        //    //Debug.Log("Connect End");
        //    m_currattackID++;

        //    // if there is more connect to Do 
        //    if (m_comboAttackStruct.attackDatas.Length > m_currattackID)
        //        StartConnect(m_currattackID);
        //    else
        //        StartRecovery();
        //}

        //private void StartRecovery()
        //{
        //    m_recoveryPassed = true;
        //   // Debug.Log("Recovery Begin");
        //    m_recoveryTimer = m_comboAttackStruct.recoverDuration / m_stateInfo.speed;
        //    m_animator.SetBool("Recovering", true);

        //    OnRecoveryStateEvent.Invoke(true);
        //    OnRecoveryEvent.Invoke();



        //    //Combo Length & Multiplier
        //    IncrementIntParameter("ComboLength");
        //    m_animator.SetFloat("PursueComboTimer", m_comboAttackStruct.pursueComboTimer);
        //    m_animator.SetTrigger("Comboing");
        //    m_pursueComboTimer = m_comboAttackStruct.pursueComboTimer;
        //}

        //private void EndRecovery()
        //{
        //    if (m_recoveryHasEnded)
        //        return;

        //    m_recoveryHasEnded = true;

        //    OnRecoveryStateEvent.Invoke(false);
        //   // Debug.Log("Recovery End");
        //    m_animator.SetBool("Recovering", false);
        //    m_animator.SetBool("Attacking", false);
        //    OnAttackStateEvent.Invoke(false);
        //    //m_animator.SetFloat("PursueComboTimer", m_pursueComboTimer);
        //}

        //private void IncrementIntParameter(string paramName)
        //{
        //    int hashKey = Animator.StringToHash(paramName);
        //    m_animator.SetInteger(hashKey, m_animator.GetInteger(hashKey) + 1);
        //}
    }
}

