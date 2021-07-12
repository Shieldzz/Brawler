using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace BTA
{
    public class TimedBattleZone : MonoBehaviour
    {
        [Header("Timers")]
        [SerializeField] float m_battleZoneDuration = 20f;
        [SerializeField] float m_timeBetweenWave = 2f;

        float m_battleZoneTimer = 0f;
        float m_waveTimer = 0f;
        public UnityEvent OnBattleZoneEnd = new UnityEvent();

        [SerializeField] TimeUI m_timeUI;
        [SerializeField] List<EnemySpawner> m_enemySpawners = new List<EnemySpawner>();

        int m_waveSpawnedCount = 0;
        int m_lastWaveSpawnedID = 0;

        public void Activate()
        {
            m_battleZoneTimer = m_battleZoneDuration;
            SpawnWaveFromSpawnerID(0);

            m_timeUI.Activate(m_battleZoneDuration);
        }

        public void Update()
        {
            if (m_battleZoneTimer > 0f)
            {
                m_battleZoneTimer -= Time.deltaTime;

                m_waveTimer -= Time.deltaTime;
                if (m_waveTimer <= 0f)
                {
                    if (m_waveSpawnedCount < 2)
                        SpawnRandomEnemyWave();
                    else
                        m_waveTimer = m_timeBetweenWave;
                }
            }
        }

        public void OnWaveKilled()
        {
            m_waveSpawnedCount--;

            if (m_battleZoneTimer <= 3.5f && m_waveSpawnedCount == 0)
            {
                OnBattleZoneEnd.Invoke();
                return;
            }

            if (m_waveSpawnedCount == 0)
                SpawnRandomEnemyWave();
        }

        private void SpawnRandomEnemyWave()
        {
            int maxSpawnerID = m_enemySpawners.Count - 1;
            int nextWaveID = 0;

            do
            {
                nextWaveID = Random.Range(0, maxSpawnerID);
            }
            while (nextWaveID == m_lastWaveSpawnedID);

            SpawnWaveFromSpawnerID(nextWaveID);
        }

        private void SpawnWaveFromSpawnerID(int ID)
        {
            if (ID >= m_enemySpawners.Count)
                return;
            if (!m_enemySpawners[ID])
                return;

            m_enemySpawners[ID].SpawnWave();
            m_lastWaveSpawnedID = ID;
            m_waveSpawnedCount++;

            m_waveTimer = m_timeBetweenWave;
        }
    }
}
