using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace BTA
{
    public class EnemySpawner : MonoBehaviour
    {
        GameObject m_cacEnemyPrefab;
        GameObject m_speCacEnemyPrefab;
        GameObject m_distEnemyPrefab;
        //GameObject m_speDistEnemyPrefab;

        [SerializeField] List<EnemySpawnData> m_SpawnPoints = new List<EnemySpawnData>();
        List<GameObject> m_waveEnemys = new List<GameObject>();

        private int m_enemySpawnedAlive = 0;

        public UnityEvent OnWaveKilled = new UnityEvent();

        // Use this for initialization
        void Awake()
        {
            m_cacEnemyPrefab = Resources.Load("Entity/Enemy/CacEnemy") as GameObject;
            m_speCacEnemyPrefab = Resources.Load("Entity/Enemy/CacEnemy_Special") as GameObject;
            m_distEnemyPrefab = Resources.Load("Entity/Enemy/DistEnemy") as GameObject;
            //m_speDistEnemyPrefab = Resources.Load("Entity/Enemy/DistEnemy_Special") as GameObject;


            if (!m_cacEnemyPrefab) Debug.LogError("spawner " + this + " has no CacEnemy Prefab ...");
            if (!m_speCacEnemyPrefab) Debug.LogError("spawner " + this + " has no SpecialCacEnemy Prefab ...");
            if (!m_distEnemyPrefab) Debug.LogError("spawner " + this + " has no DistEnemy Prefab ...");
            //if (!m_speDistEnemyPrefab) Debug.LogError("spawner " + this + " has no SpecialDistEnemy Prefab ...");
        }

        public void SpawnWave()
        {
            foreach(EnemySpawnData spawnData in m_SpawnPoints)
            {
                GameObject enemyPrefab = GetPrefabFromSpawnData(spawnData);
                if (enemyPrefab)
                    InstantiateEnemy(enemyPrefab, spawnData.transform);
            }
        }

        public void DespawnWave()
        {
            //foreach(GameObject enemy in m_waveEnemys)
            for (int i = 0; i < m_waveEnemys.Count; i++)
            {
                if (m_waveEnemys[i])
                {
                    m_waveEnemys[i].SetActive(false);
                    Destroy(m_waveEnemys[i], 2f);
                }
                /*enemy.SetActive(false); // TODO : may need to call a Enemy func to make them "disappear"
                Destroy(enemy, 2f);*/
            }
        }

        private GameObject GetPrefabFromSpawnData(EnemySpawnData spawnData)
        {
            switch (spawnData.type)
            {
                case EnemyType.CacEnemy:
                    return m_cacEnemyPrefab;
                case EnemyType.SpeCacEnemy:
                    return m_speCacEnemyPrefab;
                case EnemyType.DistEnemy:
                    return m_distEnemyPrefab;
                //case EnemyType.SpeDistEnemy:
                //    return m_speDistEnemyPrefab;
                default:
                    return null;
            }
        }

        private void InstantiateEnemy(GameObject prefab, Transform newTransform)
        {
            GameObject enemy = GameObject.Instantiate(prefab);
            m_waveEnemys.Add(enemy);

            LevelManager.Instance.GetInstanceOf<EnemyManager>().AddEnemies(enemy);
            enemy.transform.position = newTransform.position;
            enemy.transform.rotation = newTransform.rotation;

            m_enemySpawnedAlive++;

            Damageable damageable = enemy.GetComponentInChildren<Damageable>();
            damageable.OnDieEvent.AddListener(OnSpawnedEnemyKilled);
        }
       
        private void OnSpawnedEnemyKilled(AttackTrigger dmgr, Damageable dmgbl)
        {
            m_waveEnemys.Remove(dmgbl.gameObject);
            m_enemySpawnedAlive--;
            if (m_enemySpawnedAlive == 0)
            {
                m_waveEnemys.Clear();
                OnWaveKilled.Invoke();
            }
            
        }
    }
}
