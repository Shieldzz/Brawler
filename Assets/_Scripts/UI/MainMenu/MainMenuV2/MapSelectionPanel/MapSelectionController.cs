using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace BTA
{

    public class MapSelectionController : MonoBehaviour
    {

        [SerializeField] private GameObject m_level1ButtonGameObject = null;
        [SerializeField] private GameObject m_level2ButtonGameObject = null;
        [SerializeField] private GameObject m_level3ButtonGameObject = null;
        [SerializeField] private ValidatorPanelController m_validatorPanel = null;

        public LevelData m_level1Data = null;
        public LevelData m_level2Data = null;
        public LevelData m_level3Data = null;

        private CityButton m_level1Button = null;
        private CityButton m_level2Button = null;
        private CityButton m_level3Button = null;

        private SceneTransition m_sceneTransition = null;
        public SceneTransition SceneTransition { get { return m_sceneTransition; } }

        private void Awake()
        {
            m_level1Button = m_level1ButtonGameObject.GetComponent<CityButton>();
            m_level2Button = m_level2ButtonGameObject.GetComponent<CityButton>();
            m_level3Button = m_level3ButtonGameObject.GetComponent<CityButton>();

            m_level1Button.onClick.AddListener(OnLevel1);
            m_level2Button.onClick.AddListener(OnLevel2);
            m_level3Button.onClick.AddListener(OnLevel3);


            m_sceneTransition = GetComponent<SceneTransition>();
        }

        void Start()
        {
            SetLevelButtonNameWithLevelData();
        }

        private void OnEnable()
        {
            OnMapView();
            //EventSystem.current.gameObject.GetComponent<StandaloneInputModule>().submitButton = "NoneSubmit";
        }

        private void OnDisable()
        {
            UnlinkEvent();
            //EventSystem.current.gameObject.GetComponent<StandaloneInputModule>().submitButton = "Submit";
        }

        private void LinkButtonWithValidatorPanel()
        {
            m_level1Button.onClick.AddListener(OnLevel1);
            m_level2Button.onClick.AddListener(OnLevel2);
            m_level3Button.onClick.AddListener(OnLevel3);
        }

        private void SetLevelButtonNameWithLevelData()
        {
        }

        private void OnLevel1()
        {
            if (!m_level1Data)
                return;

            m_validatorPanel.gameObject.SetActive(true);
            m_validatorPanel.SetSelectedLevel(m_level1Data);
        }
        private void OnLevel2()
        {
            if (!m_level2Data)
                return;

            m_validatorPanel.gameObject.SetActive(true);
            m_validatorPanel.SetSelectedLevel(m_level2Data);
        }
        private void OnLevel3()
        {
            if (!m_level3Data)
                return;

            m_validatorPanel.gameObject.SetActive(true);
            m_validatorPanel.SetSelectedLevel(m_level3Data);
        }

        private void UnlinkEvent()
        {
            HelperUIManager helperManager = HelperUIManager.Instance;

            helperManager.ClearAllControllerActions();
        }

        public void SetHelperToMapView()
        {
            HelperUIManager helperManager = HelperUIManager.Instance;

            bool isDuo = GameManager.Instance.GetGameMode() == GameMode.Duo;

            helperManager.ClearAllSpecificControllerActions(GamePadID.Controller1);
            if(isDuo)
                helperManager.ClearAllSpecificControllerActions(GamePadID.Controller2);
            helperManager.DisableAllHelper();

            helperManager.EnableHelper(helperManager.m_helper2GameObject);
            helperManager.SetHelperInformation(helperManager.m_helper2GameObject, helperManager.m_spriteIconB, "Back", helperManager.m_controller1Helper1Action += MenuGUIManager.Instance.EnableMainPanel, GamePadID.Controller1);
            if(isDuo)
                helperManager.SetHelperInformation(helperManager.m_helper2GameObject, helperManager.m_spriteIconB, "Back", helperManager.m_controller2Helper1Action += MenuGUIManager.Instance.EnableMainPanel, GamePadID.Controller2);
        }

        public void SetHelpersToContractView()
        {
            bool isDuo = GameManager.Instance.GetGameMode() == GameMode.Duo;

            InputManager inputManager = GameManager.Instance.GetInstanceOf<InputManager>();

            HelperUIManager helperManager = HelperUIManager.Instance;
            helperManager.ClearAllSpecificControllerActions(GamePadID.Controller1);
            if(isDuo)
                helperManager.ClearAllSpecificControllerActions(GamePadID.Controller2);

            helperManager.DisableAllHelper();

            helperManager.EnableHelper(helperManager.m_helper1GameObject);
            helperManager.SetHelperInformation(helperManager.m_helper1GameObject, helperManager.m_spriteIconA, "GO !");
            inputManager.GetEvent(GamePadID.Controller1, GamePadInput.ButtonA).AddListener(m_validatorPanel.m_startButton.GetComponent<HoldToSign>().StartHolding);
            inputManager.GetEvent(GamePadID.Controller1, GamePadInput.ButtonARelease).AddListener(m_validatorPanel.m_startButton.GetComponent<HoldToSign>().StopHolding);


            helperManager.EnableHelper(helperManager.m_helper2GameObject);
            helperManager.SetHelperInformation(helperManager.m_helper2GameObject, helperManager.m_spriteIconB, "Back", helperManager.m_controller1Helper1Action += OnMapView);
            if(isDuo)
                helperManager.SetHelperInformation(helperManager.m_helper2GameObject, helperManager.m_spriteIconB, "Back", helperManager.m_controller2Helper1Action += OnMapView, GamePadID.Controller2);
        }

        private void OnMapView()
        {
            m_validatorPanel.CloseValidatorPanel();
            m_validatorPanel.gameObject.SetActive(false);
            SetHelperToMapView();
            EventSystem.current.SetSelectedGameObject(m_level1ButtonGameObject);
            m_level1Button.OnSelect(null);
        }
    }
}