using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace BTA
{
    public enum GAME_STATE
    {
        INIT = 0,
        MAIN_MENU,
        LAUNCH_GAME,
        LOADING,
        IN_GAME,
        PAUSE,
        END,
        EXIT
    }

    public class GameState : MonoBehaviour
    {

        private bool m_isInit = false;
        public bool IsInit { get { return m_isInit; } }

        private bool m_created = false;

        private GAME_STATE m_currentState;
        public GAME_STATE CurrentState { get { return m_currentState; } }

        public delegate void d_stateHandler();

        private d_stateHandler m_onStateInit = null;
        public d_stateHandler OnStateInit { get { return m_onStateInit; } set { m_onStateInit = value; } }

        private d_stateHandler m_onStateMainMenu = null;
        public d_stateHandler OnStateMainMenu { get { return m_onStateMainMenu; } set { m_onStateMainMenu = value; } }

        private d_stateHandler m_onStateLoading = null;
        public d_stateHandler OnStateLoading { get { return m_onStateLoading; } set { m_onStateLoading = value; } }

        private d_stateHandler m_onStateInGame = null;
        public d_stateHandler OnStateInGame { get { return m_onStateInGame; } set { m_onStateInGame = value; } }

        private d_stateHandler m_onStatePause = null;
        public d_stateHandler OnStatePause { get { return m_onStatePause; } set { m_onStatePause = value; } }

        private d_stateHandler m_onStateLaunchGame = null;
        public d_stateHandler OnStateLaunchGame { get { return m_onStateLaunchGame; } set { m_onStateLaunchGame = value; } }

        private d_stateHandler m_onStateExit = null;
        public d_stateHandler OnStateExit { get { return m_onStateExit; } set { m_onStateExit = value; } }

        private d_stateHandler m_onStateEnd = null;
        public d_stateHandler OnStateEnd { get { return m_onStateEnd; } set { m_onStateEnd = value; } }

        private void Awake()
        {
            if (!m_created)
            {
                DontDestroyOnLoad(gameObject);
                m_created = true;
            }
            m_currentState = GAME_STATE.INIT;
        }

        // Use this for initialization
        void Start()
        {
        }

        public void ChangeCurrentState(GAME_STATE newState)
        {
            m_currentState = newState;
            OnChangeState();
        }

        private void OnChangeState()
        {
            switch (m_currentState)
            {
                case GAME_STATE.INIT:
                    if (m_onStateInit != null)
                        m_onStateInit();
                    break;
                case GAME_STATE.MAIN_MENU:
                    if (m_onStateMainMenu != null)
                        m_onStateMainMenu();
                    Time.timeScale = 1f;
                    break;
                case GAME_STATE.LOADING:
                    if (m_onStateLoading != null)
                        m_onStateLoading();
                    break;
                case GAME_STATE.IN_GAME:
                    if (m_onStateInGame != null)
                        m_onStateInGame();
                    break;
                case GAME_STATE.PAUSE:
                    if (m_onStatePause != null)
                        m_onStatePause();
                    break;
                case GAME_STATE.LAUNCH_GAME:
                    if (m_onStateLaunchGame != null)
                        m_onStateLaunchGame();
                    break;
                case GAME_STATE.EXIT:
                    if (m_onStateExit != null)
                        m_onStateExit();
                    break;
                case GAME_STATE.END:
                    if (m_onStateEnd != null)
                        m_onStateEnd();
                    break;
                default:
                    break;
            }
        }
    }

} // namespace BTA

