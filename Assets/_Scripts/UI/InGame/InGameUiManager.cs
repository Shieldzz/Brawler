using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace BTA
{
    public class InGameUiManager : MonoBehaviour
    {
        public enum PANEL
        {
            WIN_LOSE,
            PAUSE,
        }
        private EventSystem m_eventSystem;

        public GameObject m_losePanel;
        public GameObject m_winPanel;
        public GameObject m_pausePanel;

        #region Sounds
        [FMODUnity.EventRef]
        public string m_winSound;
        [FMODUnity.EventRef]
        public string m_looseSound;
        #endregion

        PauseUI pauseUI;

        void Start()
        {
            m_eventSystem = EventSystem.current;
            pauseUI = GetComponent<PauseUI>();
        }

        void Update()
        {
        }

        public void OnLoseEvent()
        {
            m_losePanel.SetActive(true);
            LaunchLooseSound();
            pauseUI.OnActivated();
            m_eventSystem.SetSelectedGameObject(m_losePanel.GetComponent<LosePanel>().Retry);
        }

        public void OnWinEvent()
        {
            pauseUI.OnDesactivated();
            GetComponent<InGameUiInput>().SetupForWinPanel();
            m_winPanel.SetActive(true);
            m_winPanel.GetComponent<WinPanelController>().Init();
            LaunchWinSound();
        }

        public void OnRetryButton()
        {
            pauseUI.OnDesactivated();
            GameManager.Instance.GetInstanceOf<SceneLoader>().Reload();
        }

        public void OnMenuButton()
        {
            pauseUI.OnDesactivated();
            GameManager.Instance.GetInstanceOf<GameState>().ChangeCurrentState(GAME_STATE.MAIN_MENU);
        }

        public void SetEventSystemTo(PANEL panel)
        {
            if (panel == PANEL.PAUSE)
            {
                GameObject resume = m_pausePanel.GetComponent<PausePanel>().Resume;
                m_eventSystem.SetSelectedGameObject(resume, null);
                resume.GetComponent<Button>().OnSelect(null);
                Debug.Log("preselected button = " + m_eventSystem.currentSelectedGameObject.GetComponent<Button>().ToString());
            }
            //else if (panel == PANEL.WIN_LOSE)
            //{
            //    m_eventSystem.SetSelectedGameObject(m_winLosePanel.transform.GetChild(0).gameObject, null);
            //    Debug.Log("preselected button = " + m_eventSystem.currentSelectedGameObject.GetComponent<Button>().ToString());
            //}
        }

        public void LaunchWinSound()
        {
            FMODUnity.RuntimeManager.PlayOneShot(m_winSound);
        }

        public void LaunchLooseSound()
        {
            FMODUnity.RuntimeManager.PlayOneShot(m_looseSound);
        }
    }
}