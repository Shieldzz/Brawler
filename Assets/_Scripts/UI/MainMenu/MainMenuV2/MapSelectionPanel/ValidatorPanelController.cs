using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace BTA
{

    public class ValidatorPanelController : MonoBehaviour
    {
        [SerializeField] MapSelectionController m_mapViewController = null;

        public Text m_contractNumberText = null;
        public Text m_contractNameText = null;
        public Text m_contractRequester = null;
        public Text m_contractDescriptionText = null;

        public Button m_startButton = null;
        public Image m_levelPreview = null;

        private LevelData m_levelData = null;

        void Start()
        {

        }

        void Update()
        {

        }

        public void SetSelectedLevel(LevelData levelData)
        {
            m_levelData = levelData;
            DescribeLevel();
            LinkButtonToLevel();
            m_mapViewController.SetHelpersToContractView();
            EventSystem.current.SetSelectedGameObject(m_startButton.gameObject);
        }

        private void DescribeLevel()
        {
            m_contractNumberText.text = m_levelData.m_contractNumber;
            m_contractNameText.text = m_levelData.m_levelName;
            m_contractRequester.text = m_levelData.m_requester;
            m_contractDescriptionText.text = m_levelData.m_levelDescription;
            m_levelPreview.sprite = m_levelData.m_levelPreview;
        }

        private void ResetLevelInspector()
        {
            m_contractNumberText.text = "";
            m_contractNameText.text = "";
            m_contractDescriptionText.text = "";
            m_contractRequester.text = "";
            m_levelPreview.sprite = null;
        }

        private void LinkButtonToLevel()
        {
            m_startButton.onClick.AddListener(StartLevel);
        }

        private void UnlinkButtonToLevel()
        {
            m_startButton.onClick.RemoveAllListeners();
        }

        private void StartLevel()
        {
            // TODO Use Scene Loader to load Loading screen
            // pass m_levelData.m_sceneName as parameter

            m_mapViewController.SceneTransition.TransitToScene(m_levelData.m_sceneName);
            //GameManager.Instance.GetInstanceOf<SceneLoader>().LoadSceneWithLoadingScreen(m_levelData.m_sceneName);

            //GameManager.Instance.GetInstanceOf<SceneLoader>().LoadScenebyName(m_levelData.m_sceneName);
            EventSystem.current.SetSelectedGameObject(null);
            GameManager.Instance.GetInstanceOf<InputManager>().CleanAllGamePadEvent();
        }

        public void CloseValidatorPanel()
        {
            ResetLevelInspector();
            UnlinkButtonToLevel();
            m_levelData = null;
        }

    }

}