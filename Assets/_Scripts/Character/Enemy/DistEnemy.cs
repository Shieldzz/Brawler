using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace BTA
{

    public class DistEnemy : Enemy
    {
        CameraScreenCollider m_cameraScreenCollider;

        [Header("Range")]
        [SerializeField] float m_rangeAttack = 5.0f;

        [Header("Teleportation")]
        //[SerializeField] float m_cdReanbleTeleportation = 2.0f;
        [SerializeField] float m_cdEscapeTeleportation = 4.0f;
        private float m_timerEscapeTeleportation = 0.0f;
        [SerializeField] float m_distanceForEscape = 5.0f;
        [SerializeField, Range(0,4)] float m_distanceToEscape;

        [Header("Damage")]
        [SerializeField] int m_damageForTp = 75;
        private int m_currentDamageToTP = 0;

        private bool m_isTp = false;
        private bool m_isEscape = false;

        EnemiesTpEventHandler m_handler;

        // Use this for initialization
        protected override void Start()
        {
            m_cameraScreenCollider = Camera.main.GetComponent<CameraScreenCollider>();
            if (!m_cameraScreenCollider)
                Debug.LogError("Screen Collider is null");
            m_handler = GetComponentInChildren<EnemiesTpEventHandler>();

            m_handler.OnTpStateEvent.AddListener(UpdateStateTeleport);

            base.Start();



            //m_timerEscapeTeleportation = m_cdEscapeTeleportation;
        }

        // Update is called once per frame
        protected override void Update()
        {
            base.Update();
            if (m_enemyState == EnemyState.Active)
                EnemyAI();
        }

        void EnemyAI()
        {
            Player target = UpdateSideEnemy();
            Vector3 targetPosition = target.transform.position;
            targetPosition.y = 0;

            if (m_movement.IsFreeze == false)
            {
                float distance = Mathf.Abs(targetPosition.x - transform.position.x);
                if (distance < m_rangeAttack)
                    targetPosition.x = transform.position.x;

                if (m_movement.GetNavMeshAgent.isActiveAndEnabled)
                    ShouldITeleport(m_targetPlayer);

                if (m_currentTimerAttack <= 0 && m_movement.GetNavMeshAgent.isActiveAndEnabled)
                    LightAttack();

                m_movement.UpdateMovement(targetPosition);

            }
            else
                m_movement.GetNavMeshAgent.velocity = Vector3.zero;
        }


        void ShouldITeleport(Player target)
        {
            Vector3 targetPosition = target.transform.position;
            float frontTargetDistance = Vector3.Distance(target.m_frontPlayer.transform.position, transform.position);
            float backTargetDistance = Vector3.Distance(target.m_backPlayer.transform.position, transform.position);

            if (target == m_enemyMgr.GetCurrentPlayerInGame[0])
            {
                float distance = Mathf.Abs(targetPosition.x - transform.position.x);
                if (m_currentDamageToTP >= m_damageForTp)
                {
                    TpEscapePlayer(targetPosition);
                    m_currentDamageToTP = 0;
                }
                else if (distance < m_distanceForEscape && !m_isTp)
                {
                    if (m_timerEscapeTeleportation > 0)
                        m_timerEscapeTeleportation -= Time.deltaTime;
                    else
                        TpEscapePlayer(targetPosition);
                }
                else if (distance > m_distanceForEscape && !m_isTp)
                {
                    m_timerEscapeTeleportation = m_cdEscapeTeleportation;
                }

            }
        }

        void Teleportation(Vector3 position)
        {
            if (!m_isTp)
            {
                Debug.Log("Teleportation");
               // UpdateState(EnemyState.INVULNERABLE);
                transform.position = position;
                m_animator.SetTrigger("Teleport");
                //StartCoroutine(ResetSpriteTeleportation());
            }
        }


        void TpEscapePlayer(Vector3 targetPosition)
        {
            Vector3 direction = transform.position - targetPosition;
            direction.Normalize();
            direction = new Vector3(direction.x, 0, 0);
            direction *= (UnityEngine.Random.Range(0, 2) + m_distanceToEscape);

            Vector3 position = transform.position + direction;

            if (position.x > m_cameraScreenCollider.GetRightBorderPosition().x || position.x < m_cameraScreenCollider.GetLeftBorderPosition().x)
                direction *= -1;

            position = transform.position + direction;
            Teleportation(position);
            m_timerEscapeTeleportation = m_cdEscapeTeleportation;
        }

        ///*
        //IEnumerator ResetSpriteTeleportation()
        //{
        //    m_isTp = true;
        //    m_animator.SetTrigger("Teleport");
        //    float t = 0f;
        //    while (t <= 1.0f)
        //    {
        //        t += Time.deltaTime / m_cdReanbleTeleportation;
        //        yield return null;
        //    }

        //    m_isTp = false;
        //    //UpdateState(EnemyState.ACTIF);
        //    yield return new WaitForEndOfFrame();
        //}
        //*/
        public void UpdateStateTeleport(bool isTp)
        {
            if (isTp)
                UpdateState(EnemyState.Active);
            else
                UpdateState(EnemyState.Immune);
            m_isTp = isTp;
        }

        public void TakeDamage(AttackTrigger trigger, Damageable damageable)
        {
            m_currentDamageToTP += (int)trigger.Damage;
        }
    }
}
