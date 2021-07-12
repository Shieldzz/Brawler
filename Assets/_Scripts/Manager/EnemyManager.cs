using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.Events;

namespace BTA
{

    public class EnemyManager : MonoBehaviour
    {
        PlayerManager m_playerMgr;
        [SerializeField] int m_enemyNumber = 1;

        [Serializable]
        public class GameEvent : UnityEvent {}

        public GameEvent OnLastEnemyDied;

        private List<Enemy> m_enemyList = new List<Enemy>();
        public  List<Enemy> GetEnemyList { get { return m_enemyList; } }
        private List<Player> m_currentPlayerInGame = new List<Player>();
        public  List<Player> GetCurrentPlayerInGame { get { return m_currentPlayerInGame; } }

        // Use this for initialization
        void Start()
        {
            m_playerMgr = GameObject.FindObjectOfType<PlayerManager>();
            m_currentPlayerInGame.Add(m_playerMgr.GetPlayer(PlayerManager.PlayerInstance.PLAYER1));
            m_currentPlayerInGame.Add(m_playerMgr.GetPlayer(PlayerManager.PlayerInstance.PLAYER2));
        }

        // Update is called once per frame
        void Update()
        {
        }

        public void AddEnemies(GameObject enemy)
        {
            m_enemyList.Add(enemy.GetComponent<Enemy>());
        }

        public void EnemyIsDead()
        {
            m_enemyNumber--;
            if (m_enemyNumber <= 0)
                OnLastEnemyDied.Invoke();
        }

    }

} // namespace BTA
