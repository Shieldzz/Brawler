using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


namespace BTA
{

    public class NavMeshMovement : BasicMovement
    {
        protected NavMeshAgent m_navMeshAgent;
        public NavMeshAgent GetNavMeshAgent { get { return m_navMeshAgent; } }
        private PositionDetector m_positionDetector;
        private bool m_isKnock = false;

        // Use this for initialization
        protected override void Awake()
        {
            base.Awake();

            m_positionDetector = GetComponent<PositionDetector>();
            m_positionDetector.OnGroundEvent.AddListener(OnGroundDetection);
            m_positionDetector.enabled = false;
            m_navMeshAgent = GetComponent<NavMeshAgent>();
            m_navMeshAgent.speed = Speed;
            m_navMeshAgent.updateRotation = false;
            m_navMeshAgent.enabled = false;
        }

        protected override void OnDisable()
        {
            base.OnDisable();
        }

        private void OnGroundDetection()
        {
            if (m_navMeshAgent.enabled == false)
            {
                m_positionDetector.enabled = false;
                m_navMeshAgent.enabled = true;
                Freeze(false);
            }
        }

        override protected void FixedUpdate()
        {
            base.FixedUpdate();
        }

        public void UpdateMovement(Vector3 position)
        {
            if (m_isFrozen)
            {
                m_animator.SetFloat("InputX", 0f);
                m_animator.SetFloat("InputY", 0f);
                return;
            }

            //if Freeze Don't update movement or Rotation

            //            XFlip(xAxis);

            //Managing X and Y 
            if (m_navMeshAgent.isActiveAndEnabled)
            {
                m_animator.SetFloat("InputX", m_navMeshAgent.velocity.x);
                m_animator.SetFloat("InputY", m_navMeshAgent.velocity.z);
                m_navMeshAgent.SetDestination(new Vector3(position.x, 0, position.z));
            }
        }

        override public void Freeze(bool dofreeze)
        {
            if (dofreeze == m_isFrozen)
                return;
            base.Freeze(dofreeze);

            if (!m_positionDetector.isActiveAndEnabled)
                m_navMeshAgent.enabled = !dofreeze;
        }

      /*  override public void Stagger()
        {
            Debug.Log("Stagger ! : " + gameObject.name);
            Freeze(true);
            if (m_animator)
                m_animator.SetBool("isStagger", true);

        }
        */
        override public void Knock(float force, Vector3 Direction)
        {
            if (Direction == Vector3.down && m_positionDetector.CheckGround())
            {
                Stagger();
                return;
            }

            if (m_isFrozen)
                Freeze(false);

            //if (Direction == Vector3.up)
            //{
            //    Debug.Log("Plpay KnockUp !");
            //    m_animator.Play("KnockUp", 0);
            //}

            //m_positionDetector.enabled = true;
            m_navMeshAgent.velocity = Vector3.zero;
            m_navMeshAgent.enabled = false;
            m_rigidbody.velocity = Vector3.zero;
            m_rigidbody.AddForce(Direction * force); // / m_rigidBody.mass;

            StartCoroutine(m_positionDetector.EnableAfterXSec(0.05f));
        }
    }
}