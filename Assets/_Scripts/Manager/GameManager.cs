using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System;

namespace BTA
{
    public enum GameMode
    {
        Solo = 0,
        Duo
    }

    public class GameManager : MonoBehaviour
    {
        #region Manager
        private static GameManager m_instance = null;
        public static GameManager Instance
        {
            get
            {
                if (!m_instance)
                    m_instance = new GameManager();
                return m_instance;
            }
        }
        private static bool m_created = false;
        #endregion

        private bool m_canLaunchGame = true;
        public bool CanLaunchGame { get { return m_canLaunchGame; } set { m_canLaunchGame = value; } }

        private InputManager m_inputManager = null;
        private SoundManager m_soundManager = null;
        private SceneLoader m_sceneLoader = null;
        private SerializeManager m_serializeManager = null;
        private GameState m_gameState = null;
        private GameplayManager m_gameplayManager = null;

        private GameMode m_mode;

        public class SwitchModeEvent : UnityEvent { };

        public SwitchModeEvent OnSoloMode = new SwitchModeEvent();
        public SwitchModeEvent OnDuoMode = new SwitchModeEvent();


        public T GetInstanceOf<T>() where T : MonoBehaviour
        {
            if (typeof(T) == typeof(InputManager))
                return m_inputManager as T;
            else if (typeof(T) == typeof(SoundManager))
                return m_soundManager as T;
            else if (typeof(T) == typeof(SceneLoader))
                return m_sceneLoader as T;
            else if (typeof(T) == typeof(SerializeManager))
                return m_serializeManager as T;
            else if (typeof(T) == typeof(GameState))
                return m_gameState as T;
            else if (typeof(T) == typeof(GameplayManager))
                return m_gameplayManager as T;

            Debug.LogError("GetInstanceOf return default ...");
            return default(T);
        }

        void Awake()
        {
            if (GameObject.FindObjectsOfType<GameManager>().Length > 1)
            {
                Destroy(this);
                return;
            }

            if (!m_created)
            {
                DontDestroyOnLoad(gameObject);
                m_created = true;
                m_instance = this;
            }
            Init();
        }

        void Init()
        {
            CreateManager();
            m_gameState.OnStateExit += ExitGame;
        }

        private T InstanciateManager<T>(string name = "GameObject") where T : MonoBehaviour
        {
            GameObject gao = new GameObject(name);
            gao.AddComponent<T>();
            return gao.GetComponent<T>();
        }

        private void CreateManager()
        {
            if (m_gameState == null)
                m_gameState = InstanciateManager<GameState>("GameState");

            if (m_inputManager == null)
                m_inputManager = InstanciateManager<InputManager>("InputManager");

            if (m_soundManager == null)
                m_soundManager = InstanciateManager<SoundManager>("Sound Manager");

            if (m_sceneLoader == null)
                m_sceneLoader = InstanciateManager<SceneLoader>("SceneLoader");

            if (m_serializeManager == null)
                m_serializeManager = InstanciateManager<SerializeManager>("SerializeManager");

            if (m_gameplayManager == null)
                m_gameplayManager = InstanciateManager<GameplayManager>("GamePlayManager");
        }

        void Update()
        {
            if (m_gameState && m_gameState.CurrentState == GAME_STATE.INIT && m_canLaunchGame)
            {
                if (Input.anyKey)
                    m_gameState.ChangeCurrentState(GAME_STATE.MAIN_MENU);
            }
        }

        public void SetGameMode(GameMode newGameMode)
        {
            if (newGameMode == m_mode)
                return;

            m_mode = newGameMode;

            switch (newGameMode)
            {
                case GameMode.Solo:
                    Debug.Log("Solo Event !");
                    OnSoloMode.Invoke();
                    break;
                case GameMode.Duo:
                    Debug.Log("Duo Event !");
                    OnDuoMode.Invoke();
                    break;
                default:
                    break;
            }
        }

        public GameMode GetGameMode()
        {
            return m_mode;
        }

        private void ExitGame()
        {
            Application.Quit();
        }
    }
} // namespace BTA

