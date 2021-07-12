using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace BTA
{

    public class MainPanelUIManager : MonoBehaviour
    {

        public GameObject m_cityMapGameObject = null;
        public GameObject m_essencesGameObject = null;
        public GameObject m_optionGameObject = null;
        public GameObject m_quitGameObject = null;

        public PlayerSelectorManager m_playerSelectorManager = null;

        private MainMenuButton m_cityMapButton = null;
        private MainMenuButton m_essenceButton = null;
        private MainMenuButton m_optionButton = null;
        private MainMenuButton m_quitButton = null;

        private MenuGUIManager m_menuiGUIManager = null;

        private void Awake()
        {
            m_menuiGUIManager = MenuGUIManager.Instance;

            m_cityMapButton = m_cityMapGameObject.GetComponent<MainMenuButton>();
            m_essenceButton = m_essencesGameObject.GetComponent<MainMenuButton>();
            m_optionButton = m_optionGameObject.GetComponent<MainMenuButton>();
            m_quitButton = m_quitGameObject.GetComponent<MainMenuButton>();
        }

        void Start()
        {
            SetupButtonEvent();
            GameManager gameManager = GameManager.Instance;

            if (gameManager.GetGameMode() == GameMode.Duo)
                OnAddPlayer();

            gameManager.OnDuoMode.AddListener(OnAddPlayer);
            gameManager.OnSoloMode.AddListener(OnRemovePlayer);
        }

        private void SetupButtonEvent()
        {
            m_cityMapButton.onClick.AddListener(OnCityMapClick);
            m_essenceButton.onClick.AddListener(OnEssenceClick);
            m_optionButton.onClick.AddListener(OnOptionClick);
            m_quitButton.onClick.AddListener(OnQuitClick);
        }

        private void OnEnable()
        {
            LinkHelperBar();

            EventSystem.current.SetSelectedGameObject(m_cityMapGameObject);
            m_cityMapButton.OnSelect(null);
        }

        void Update()
        {
        }

        private void OnDisable()
        {
            UnlinkHelperBar();
        }

        private void LinkHelperBar()
        {
            HelperUIManager helperManager = HelperUIManager.Instance;
            helperManager.DisableAllHelper();
            helperManager.ClearAllControllerActions();

            helperManager.ClearAction(ref helperManager.m_controller1Helper1Action, GamePadID.Controller1, GamePadInput.ButtonA);
            helperManager.ClearAction(ref helperManager.m_controller2Helper1Action, GamePadID.Controller2, GamePadInput.ButtonA);

            helperManager.EnableHelper(helperManager.m_helper1GameObject);
            helperManager.SetHelperInformation(helperManager.m_helper1GameObject, helperManager.m_spriteIconA, "Confirm");

            helperManager.SetHelperInformation(helperManager.m_helper4GameObject, helperManager.m_spriteIconLbRb, "Swap Characters");


            if (GameManager.Instance.GetGameMode() == GameMode.Solo)
            {
                helperManager.EnableHelper(helperManager.m_helper5GameObject);
                helperManager.SetHelperInformation(helperManager.m_helper5GameObject, helperManager.m_spriteIconStart, "Add Player", helperManager.m_controller2Helper5Action += OnAddPlayer, GamePadID.Controller2);
            }
            else
            {
                helperManager.EnableHelper(helperManager.m_helper4GameObject);

                helperManager.EnableHelper(helperManager.m_helper5GameObject);
                helperManager.SetHelperInformation(helperManager.m_helper5GameObject, helperManager.m_spriteIconStart, "Remove Player", helperManager.m_controller2Helper5Action += OnRemovePlayer, GamePadID.Controller2);
            }
        }

        private void UpdateLinkHelperBar()
        {
            HelperUIManager helperManager = HelperUIManager.Instance;

            if (GameManager.Instance.GetGameMode() == GameMode.Solo)
            {
                helperManager.DisableHelper(helperManager.m_helper4GameObject);

                helperManager.ClearAction(ref helperManager.m_controller2Helper5Action, GamePadID.Controller2, GamePadInput.ButtonStart, OnRemovePlayer);
                helperManager.SetHelperInformation(helperManager.m_helper5GameObject, helperManager.m_spriteIconStart, "Add Player", helperManager.m_controller2Helper5Action += OnAddPlayer, GamePadID.Controller2);
            }
            else
            {
                helperManager.EnableHelper(helperManager.m_helper4GameObject);

                helperManager.ClearAction(ref helperManager.m_controller2Helper5Action, GamePadID.Controller2, GamePadInput.ButtonStart, OnAddPlayer);
                helperManager.SetHelperInformation(helperManager.m_helper5GameObject, helperManager.m_spriteIconStart, "Remove Player", helperManager.m_controller2Helper5Action += OnRemovePlayer, GamePadID.Controller2);
            }
        }

        private void UnlinkHelperBar()
        {
            HelperUIManager helperManager = HelperUIManager.Instance;
            helperManager.DisableAllHelper();
            helperManager.ClearAllControllerActions();
        }

        private void OnCityMapClick()
        {
            Debug.Log("GameMode: " + GameManager.Instance.GetGameMode());
            m_menuiGUIManager.EnableMapPanel();
            Debug.Log("OnCityMapClick");
        }

        private void OnEssenceClick()
        {
            m_menuiGUIManager.EnableEssencePanel();
            Debug.Log("OnEssenceClick");
        }

        private void OnOptionClick()
        {
            m_menuiGUIManager.EnableOptionsPanel();
            Debug.Log("OnOptionClick");
        }

        private void OnQuitClick()
        {
            GameManager.Instance.GetInstanceOf<GameState>().ChangeCurrentState(GAME_STATE.EXIT);
            Debug.Log("OnQuitClick");
        }

        private void OnAddPlayer()
        {
            m_playerSelectorManager.AddPlayer();
            GameManager.Instance.SetGameMode(GameMode.Duo);
            UpdateLinkHelperBar();

        }

        private void OnRemovePlayer()
        {
            m_playerSelectorManager.RemovePlayer();
            GameManager.Instance.SetGameMode(GameMode.Solo);
            UpdateLinkHelperBar();
        }
    }

}