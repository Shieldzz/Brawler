using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BTA
{

    public class CacEnemy : Enemy
    {
        [SerializeField] private Collider m_attackTrigger;

        // Use this for initialization
        protected override void Start()
        {
            base.Start();
        }

        // Update is called once per frame
        protected override void Update()
        {
            base.Update();
            if (m_enemyState == EnemyState.Active)
                EnemyAI();
        }

        public override void MoveEnemy()
        {
 
        }

        void EnemyAI()
        {
            Player target = UpdateSideEnemy();
            Vector3 targetPosition = target.transform.position;

            float frontTargetDistance = Vector3.Distance(m_targetPlayer.m_frontPlayer.transform.position, transform.position);
            float backTargetDistance = Vector3.Distance(m_targetPlayer.m_backPlayer.transform.position, transform.position);

            if (frontTargetDistance < backTargetDistance)
                targetPosition = m_targetPlayer.m_frontPlayer.transform.position;
            else if (frontTargetDistance > backTargetDistance)
                targetPosition = m_targetPlayer.m_backPlayer.transform.position;

            Vector3 transformPos = transform.position;
            transformPos.y = 0;
            Vector3 position = targetPosition;
            position.y = 0;

            float distance = Vector3.Distance(transformPos, position);
            if (m_movement.IsFreeze == false)
            {
                if (m_distanceEnemyAttack < distance)
                    m_movement.UpdateMovement(targetPosition);
                else if (m_distanceEnemyAttack >= distance && m_currentTimerAttack <= 0 && m_movement.GetNavMeshAgent.isActiveAndEnabled)
                    LightAttack();
            }
            else
                m_movement.GetNavMeshAgent.velocity = Vector3.zero;
        }
    }
}
