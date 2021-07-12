using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;


namespace BTA
{

    public class MenuGUI : MonoBehaviour
    {
        private EventSystem m_eventSystem;
        private SceneLoader m_sceneLoader;

        [SerializeField] private GameObject m_menuPanel;
        [SerializeField] private GameObject m_loadingPanel;
        [SerializeField] private GameObject m_selectLevelPanel;
        [SerializeField] private GameObject m_selectCharacterPanel;
        [SerializeField] private GameObject m_optionPanel;

        [SerializeField] private CharacterSelectionGUI m_characterSelectionGUI;


        void Start()
        {
            m_eventSystem = EventSystem.current;
            m_sceneLoader = GameManager.Instance.GetInstanceOf<SceneLoader>();
        }

        public void OnLevelSelected(string levelName)
        {
            m_loadingPanel.SetActive(true);
            m_selectLevelPanel.SetActive(false);

            GameManager.Instance.GetInstanceOf<GameState>().ChangeCurrentState(GAME_STATE.LAUNCH_GAME);
            m_sceneLoader.LoadScenebyName(levelName);
        }

        public void OnSelectLevelButton()
        {
            m_selectLevelPanel.SetActive(true);
            m_menuPanel.SetActive(false);
            m_eventSystem.SetSelectedGameObject(m_selectLevelPanel.transform.GetChild(0).gameObject, null);
        }

        public void OnExitButton()
        {
            GameManager.Instance.GetInstanceOf<GameState>().ChangeCurrentState(GAME_STATE.EXIT);
        }

        public void OnOptionButton()
        {
            m_optionPanel.SetActive(true);
            m_menuPanel.SetActive(false);
            m_eventSystem.SetSelectedGameObject(m_optionPanel.transform.GetChild(0).gameObject, null);
        }

        public void OnMenubutton()
        {
            m_selectLevelPanel.SetActive(false);
            m_selectCharacterPanel.SetActive(false);
            m_optionPanel.SetActive(false);

            m_menuPanel.SetActive(true);
            m_eventSystem.SetSelectedGameObject(m_menuPanel.transform.GetChild(0).gameObject, null);
        }
    }
}