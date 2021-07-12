using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace BTA
{

    public class Enemy : ACharacter
    {
        protected enum EnemyState
        {
            Active = 0,
            Immune
        }

        protected EnemyState m_enemyState = EnemyState.Immune;

        //GameManager m_gameMgr;
        PlayerManager m_playerMgr;
        protected EnemyManager m_enemyMgr;


        protected EnemyFighting m_fighting;
        private EnemySounds m_sounds;

        protected NavMeshMovement m_movement;
        protected NavMeshAgent m_navMeshAgent;

        [SerializeField] private float m_slimValue = 1f;

        BoxCollider m_hitBoxCollilder;

        [Header("Attack")]
        [SerializeField] protected float m_distanceEnemyAttack = 1.5f;
        [SerializeField] protected float m_timerAttack = 5f;
        protected float m_currentTimerAttack = 0f;

        [Header("Spawn")]
        [SerializeField] float m_spawnDuration = 1f;
        [SerializeField] float m_despawnDuration = 1f;

        protected Player m_targetPlayer = null;
        public Player GetTargetPlayer { get { return m_targetPlayer; } }

        // Use this for initialization
        protected virtual void Start()
        {
            //m_gameMgr = GameManager.Instance;
            m_playerMgr = LevelManager.Instance.GetInstanceOf<PlayerManager>();
            m_enemyMgr = LevelManager.Instance.GetInstanceOf<EnemyManager>();

            m_animator = GetComponentInChildren<Animator>();
            m_movement = GetComponent<NavMeshMovement>();
            m_fighting = GetComponent<EnemyFighting>();
            m_sounds = GetComponentInChildren<EnemySounds>();

            m_hitBoxCollilder = transform.Find("HitBox").GetComponent<BoxCollider>();

            UpdateState(EnemyState.Immune);
            StartCoroutine(WaitInitializeCollider());
        }

        // Update is called once per frame
        protected virtual void Update()
        {
            if (m_currentTimerAttack > 0)
                m_currentTimerAttack -= Time.deltaTime;
        }

        public void OnHit()
        {
            if (m_sounds)
                m_sounds.LauchHitSounds();
        }

        public void OnDie(AttackTrigger trigger, Damageable damageable)
        {
            Debug.Log("On Enemy Die");
            m_animator.SetBool("Dead", true);
            m_animator.Play("Death");
            //GameManager.Instance.GetInstanceOf<GameplayManager>().OnEnemyDie();
            if(trigger.Owner)
            {
                Player player = trigger.Owner.GetComponent<Player>();
                player.AddSlimeBall(m_slimValue);
            }
            
            UpdateState(EnemyState.Immune);
            Destroy(gameObject, m_despawnDuration);
        }

        public virtual void MoveEnemy()
        {
        }

        protected Player UpdateSideEnemy()
        {
            m_targetPlayer = FindTheNearestPlayer();
            Vector3 playerRelativePosition = m_targetPlayer.transform.position - transform.position;

            if (playerRelativePosition.x < 0)
                m_movement.XFlip(1);
            else if (playerRelativePosition.x > 0)
                m_movement.XFlip(-1);

            return m_targetPlayer;
        }

        protected Player FindTheNearestPlayer()
        {
            Player player1 = m_enemyMgr.GetCurrentPlayerInGame[0];
            Player player2 = m_enemyMgr.GetCurrentPlayerInGame[1];

            if (player1.isActiveAndEnabled == false || player2.isActiveAndEnabled == false)
            {
                if (player1.isActiveAndEnabled)
                    return player1;
                else
                    return player2;
            }
            else
            {
                if (player1.GetDameageable().IsAlive())
                {
                    if (player2.GetDameageable().IsAlive())
                    {
                        float distBetweenPlayer1AndEnemy = Vector3.Distance(transform.position, player1.transform.position);
                        float distBetweenPlayer2AndEnemy = Vector3.Distance(transform.position, player2.transform.position);

                        if (distBetweenPlayer1AndEnemy < distBetweenPlayer2AndEnemy)
                            return player1;
                        else
                            return player2;
                    }
                    else
                        return player1;
                }
                else
                    return player2;

            }
        }

        protected void LightAttack()
        {
            m_fighting.AttackInput(AttackType.LightAttack);
            m_currentTimerAttack = m_timerAttack;
        }

        protected void UpdateState(EnemyState newState)
        {
            m_enemyState = newState;
            switch (newState)
            {
                case EnemyState.Active:
                    {
                        Enable();
                        break;
                    }
                case EnemyState.Immune:
                    {
                        Disable();
                        break;
                    }
                default:
                    break;
            }
        }

        protected void Enable()
        {
            m_hitBoxCollilder.enabled = true;
            m_movement.GetNavMeshAgent.enabled = true;
        }

        protected void Disable()
        {
            m_hitBoxCollilder.enabled = false;
            m_movement.GetNavMeshAgent.enabled = false;
        }

        IEnumerator WaitInitializeCollider()
        {
            yield return new WaitForSeconds(m_spawnDuration);
         
            UpdateState(EnemyState.Active);

            yield return new WaitForEndOfFrame();
        }
    }
}