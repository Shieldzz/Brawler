using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BTA
{

    public class LevelManager : MonoBehaviour
    {
        private static LevelManager m_instance = null;
        public static LevelManager Instance
        {
            get
            {
                if (!m_instance)
                    Debug.LogError("No LevelManager Instance ...");
                return m_instance;
            }
        }

        private PlayerManager m_playerManager = null;
        private CameraManager m_cameraManager = null;
        private EnemyManager m_enemyManager = null;
        private ParalaxManager m_paralaxManager = null;
        private NarrativeManager m_narativeManager = null;

        [SerializeField] public LevelData m_level1 = null;
        [SerializeField] public LevelData m_level2 = null;
        [SerializeField] public LevelData m_level3 = null;

        public T GetInstanceOf<T>() where T : class
        {
            if (typeof(T) == typeof(EnemyManager))
                return m_enemyManager as T;
            else if (typeof(T) == typeof(PlayerManager))
                return m_playerManager as T;
            else if (typeof(T) == typeof(CameraManager))
                return m_cameraManager as T;
            else if (typeof(T) == typeof(ParalaxManager))
                return m_paralaxManager as T;
            else if (typeof(T) == typeof(NarrativeManager))
                return m_narativeManager as T;

            return default(T);
        }

        private void Awake()
        {
            m_instance = this;
            m_playerManager = GetComponentInChildren<PlayerManager>();
            m_cameraManager = GetComponentInChildren<CameraManager>();
            m_enemyManager = GetComponentInChildren<EnemyManager>();
            m_paralaxManager = GetComponentInChildren<ParalaxManager>();
            m_narativeManager = GetComponentInChildren<NarrativeManager>();
        }

        // Use this for initialization
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }

        public LevelData GetLevelDataFromLevelName(string levelName)
        {
            if (levelName == m_level1.m_sceneName)
                return m_level1;
            else if (levelName == m_level2.m_sceneName)
                return m_level2;
            else if (levelName == m_level3.m_sceneName)
                return m_level3;

            return null;
        }
    }

} // namespace BTA

