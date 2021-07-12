using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BTA
{
    public class PauseUI : MonoBehaviour
    {
        PlayerManager m_playerMgr;
        NarrativeManager m_narrativeMgr;
        public GameObject m_pausePanel;

        void Start()
        {
            LevelManager levelMgr = LevelManager.Instance;

            m_playerMgr = levelMgr.GetInstanceOf<PlayerManager>();
            m_narrativeMgr = levelMgr.GetInstanceOf<NarrativeManager>();
        }

        void Update()
        {
        }

        public void OnResumeButton()
        {
            OnDesactivated();
            m_pausePanel.SetActive(false);
        }

        public void OnMenuButton()
        {
            OnDesactivated();
            GameManager.Instance.GetInstanceOf<GameState>().ChangeCurrentState(GAME_STATE.MAIN_MENU);
            //m_loadingCanvas.gameObject.SetActive(true);
        }

        public void OnActivated()
        {
            GetComponent<InGameUiManager>().SetEventSystemTo(InGameUiManager.PANEL.PAUSE);
            Time.timeScale = 0;
            m_playerMgr.Pause(true);
        }

        public void OnDesactivated()
        {
            Time.timeScale = 1;

            if (!m_narrativeMgr.IsInNarration)
                m_playerMgr.Pause(false);
        }

    }
}