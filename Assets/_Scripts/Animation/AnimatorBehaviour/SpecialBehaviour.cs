using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace BTA
{

    public class SpecialBehaviour : StateMachineBehaviour
    {
        //Animator m_animator;
        //AnimatorStateInfo m_stateInfo;

        //#region Event

        //public class StateEvent : UnityEvent<bool> { }
        //public class SpecialAttackEvent : UnityEvent<float> { }
        //public SpecialAttackEvent OnSpecialAttackEvent = new SpecialAttackEvent();
        //public class ConnectEvent : UnityEvent<AttackType, AttackData, int> { }

        //public StateEvent OnAttackStateEvent = new StateEvent();
        //public StateEvent OnRecoveryStateEvent = new StateEvent();

        //public ConnectEvent OnConnectEvent = new ConnectEvent();
        //[HideInInspector] public UnityEvent OnRecoveryEvent = new UnityEvent();
        //#endregion

        //[SerializeField] SpecialAttack m_specialAttack;
        //SpecialLevel m_currSpecialLevel;
        //AttackStruct m_currAttackStruct;

        //float m_startUpTimer = 0f;
        //float m_connectTimer = 0f;
        //float m_recoveryTimer = 0F;

        //float m_pursueComboTimer = 0f;

        //int m_currSpecialLevelID = 0;
        //int m_currattackID = 0;
        
        //bool m_hasEnded = false;

        //// OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
        //override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        //{
        //    //Debug.Log("State Enter");
        //    m_animator = animator;
        //    m_stateInfo = stateInfo;

        //    //Init
        //    m_currSpecialLevelID = animator.GetInteger("SpecialLevel");
        //    m_currSpecialLevel = m_specialAttack.SpecialLevels[m_currSpecialLevelID];
        //    m_currAttackStruct = m_currSpecialLevel.atkStruct;
        //    m_hasEnded = false;

        //    float currSpecialSpeed = m_stateInfo.speed / m_currSpecialLevel.animationDuration;
        //    m_animator.speed = currSpecialSpeed;

        //    //Debug.Log("animation speed = " + m_stateInfo.speed + " or speed = " + m_stateInfo.speed * m_stateInfo.speedMultiplier);
        //    m_startUpTimer = m_currAttackStruct.startUpDuration / (m_stateInfo.speed * m_stateInfo.speedMultiplier);
        //    m_connectTimer = 0f;
        //    m_recoveryTimer = 0F; 

        //    //Debug.Log("Attack !");
        //    m_animator.SetBool("Attacking", true);
        //    OnAttackStateEvent.Invoke(true);
        //}

        //// OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
        //override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        //{
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
        //            EndRecovery();
        //    }
        //}

        //// OnStateExit is called when a transition ends and the state machine finishes evaluating this state
        //override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        //{
        //    //Debug.Log("Exit Recovery =>" + m_recoveryHasEnded);
        //    EndRecovery();
        //}

        //private void StartConnect(int id)
        //{
        //    if (id == 0)
        //        OnSpecialAttackEvent.Invoke(m_currSpecialLevel.range);

        //    //Debug.Log("Connect Begin");
        //    OnConnectEvent.Invoke(AttackType.SpecialAttack, m_currAttackStruct.attackDatas[id], m_currSpecialLevelID);
        //    m_connectTimer = m_currAttackStruct.attackDatas[id].connectDuration / (m_stateInfo.speed * m_stateInfo.speedMultiplier);
        //}

        //private void EndConnect()
        //{
        //    //Debug.Log("Connect End");
        //    m_currattackID++;

        //    // if there is more connect to Do 
        //    if (m_currAttackStruct.attackDatas.Length > m_currattackID)
        //        StartConnect(m_currattackID);
        //    else
        //        StartRecovery();
        //}

        //private void StartRecovery()
        //{
        //    //Debug.Log("Recovery Begin");
        //    m_recoveryTimer = m_currAttackStruct.recoverDuration / (m_stateInfo.speed * m_stateInfo.speedMultiplier);
        //    OnRecoveryEvent.Invoke();
        //}

        //private void EndRecovery()
        //{
        //    if (m_hasEnded)
        //        return;

        //    m_hasEnded = true;

        //    m_animator.SetFloat("PursueComboTimer", 0f);
        //    m_animator.SetInteger("ComboLength", 0);

        //    m_animator.speed = 1f;
        //    OnRecoveryStateEvent.Invoke(false);
        //    //Debug.Log("Recovery End");
        //    m_animator.SetBool("Attacking", false);
        //    OnAttackStateEvent.Invoke(false);
        //}
    }
}
