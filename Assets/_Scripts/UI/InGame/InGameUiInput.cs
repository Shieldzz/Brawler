using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BTA
{

    public class InGameUiInput : MonoBehaviour
    {
        InputManager m_inputMgr;
        PlayerManager m_playerMgr;
        public GameObject m_pausePanel;

        private bool m_canPause = true;

        void Start()
        {
            GameManager gameMgr = GameManager.Instance;
            m_inputMgr = gameMgr.GetInstanceOf<InputManager>();
            m_playerMgr = LevelManager.Instance.GetInstanceOf<PlayerManager>();

            //gameMgr.OnSoloMode.AddListener(OnSolo);
            //gameMgr.OnDuoMode.AddListener(OnDuo);

            m_inputMgr.GetEvent(GamePadID.Controller1, GamePadInput.ButtonStart).AddListener(OnPauseButton);
            m_inputMgr.GetEvent(GamePadID.Controller2, GamePadInput.ButtonStart).AddListener(OnPauseButton);
        }

        public void SetupForWinPanel()
        {
            m_playerMgr.Pause(true);
            m_inputMgr.GetEvent(GamePadID.Controller1, GamePadInput.ButtonStart).RemoveListener(OnPauseButton);
            m_inputMgr.GetEvent(GamePadID.Controller2, GamePadInput.ButtonStart).RemoveListener(OnPauseButton);
        }

        void Update()
        {
            if (m_canPause)
            {
                if (Input.GetKeyDown(KeyCode.Escape))
                {
                    OnPauseButton();
                }
            }
        }

        private void OnPauseButton()
        {
            if (m_pausePanel.activeSelf)
            {
                GetComponent<PauseUI>().OnResumeButton();
            }
            else if (!m_pausePanel.activeSelf && !m_playerMgr.IsPaused)
            {
                GetComponent<PauseUI>().OnActivated();
                m_pausePanel.SetActive(true);
            }

        }

        public void DisablePause()
        {
            if (m_pausePanel.activeSelf)
                GetComponent<PauseUI>().OnResumeButton();
            m_canPause = false;
        }

        public void EnablePause()
        {
            m_canPause = true;
        }

    }
}