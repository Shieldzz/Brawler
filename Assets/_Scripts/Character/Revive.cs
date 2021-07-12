using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BTA
{

    public class Revive : MonoBehaviour
    {

        private BoxCollider m_reviveCollider = null;
        private Player m_player = null;
        public Player Player { get { return m_player; } set { m_player = value; } }

        private bool m_isReviving = false;
        private bool m_isMateIsInArea = false;

        public float m_reviveTime = 3f;
        private float m_reviveTimer = 0f;

        private int m_enemiesCount = 0;
        private List<Enemy> m_enemyList = null;

        // Use this for initialization
        void Start()
        {
            m_enemyList = new List<Enemy>();
        }

        // Update is called once per frame
        void Update()
        {
            if (m_enemyList.Count != 0 && m_isMateIsInArea)
            {
                for(int idx = m_enemyList.Count -1; idx >= 0; idx--)
                {
                    if (!m_enemyList[idx])
                    {
                        m_enemyList.RemoveAt(idx);
                        continue;
                    }
                    if (!m_enemyList[idx].gameObject.activeSelf)
                        m_enemyList.RemoveAt(idx);
                }
            }

            if (m_isMateIsInArea && m_enemyList.Count == 0 && !m_isReviving)
                StartReviveTimer();
           
        }


        private void OnTriggerEnter(Collider other)
        {
            if (other.tag == "Player" && other.GetComponent<Player>() != m_player)
            {
                m_isMateIsInArea = true;
                if (m_enemyList.Count == 0)
                    StartReviveTimer();
            }
            else if(other.tag == "Enemy")
            {
                m_enemyList.Add(other.GetComponent<Enemy>());
                m_enemiesCount++;
                CancelRevive();
            }
        }


        private void OnTriggerExit(Collider other)
        {
            if (other.tag == "Player")
            {
                m_isMateIsInArea = false;
                CancelRevive();
            }
            else if(other.tag == "Enemy")
            {
                m_enemyList.Remove(other.GetComponent<Enemy>());
                m_enemiesCount--;
            }
        }

        private IEnumerator ReviveCountdown()
        {
            if (!m_isMateIsInArea)
                yield return null;
            while(m_reviveTimer <= 1f)
            {
                m_reviveTimer += Time.deltaTime / m_reviveTime;
                yield return null;
            }

            ReviveDone();
        }

        private void StartReviveTimer()
        {
            m_isReviving = true;
            StartCoroutine(ReviveCountdown());
        }

        private void CancelRevive()
        {
            StopAllCoroutines();
            m_isReviving = false;
            m_reviveTimer = 0f;
        }

        private void ReviveDone()
        {
            m_isReviving = false;
            m_isMateIsInArea = false;
            m_reviveTimer = 0f;
            m_enemiesCount = 0;
            m_enemyList.Clear();
            StopCoroutine(ReviveCountdown());
            m_player.Revive();
        }

    }
}