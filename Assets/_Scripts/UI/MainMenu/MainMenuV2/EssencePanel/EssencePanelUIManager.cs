using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace BTA
{

    public class EssencePanelUIManager : MonoBehaviour
    {

        private EventSystem m_currentEventSystem = EventSystem.current;

        public GameObject m_player1Panel = null;
        public GameObject m_player2Panel = null;

        private bool m_isPlayer1PanelEnable = true;

        void Start()
        {
        }

        void Update()
        {

        }

        public void OnEnable()
        {
            //EventSystem.current.gameObject.GetComponent<StandaloneInputModule>().submitButton = "NoneSubmit";

            GameplayManager gameplayManager = GameManager.Instance.GetInstanceOf<GameplayManager>();
            gameplayManager.ShopVisited = true;

            m_player2Panel.SetActive(true);
            m_player1Panel.SetActive(true);

            if (GameManager.Instance.GetGameMode() == GameMode.Solo)
            {
                PlayerPanelController playerPanel1Controller = m_player1Panel.GetComponent<PlayerPanelController>();
                PlayerPanelController playerPanel2Controller = m_player2Panel.GetComponent<PlayerPanelController>();

                playerPanel1Controller.Init(GamePadID.Controller1, CharacterEnum.MELEE);
                playerPanel2Controller.Init(GamePadID.Controller1, CharacterEnum.DISTANCE);

                if (m_isPlayer1PanelEnable)
                {
                    playerPanel1Controller.EnablePanel();
                    BlockPanelInput(m_player2Panel);
                }
                else
                {
                    playerPanel2Controller.EnablePanel();
                    BlockPanelInput(m_player1Panel);
                }
            }
            else
            {
                PlayerPanelController playerPanel1Controller = m_player1Panel.GetComponent<PlayerPanelController>();
                PlayerPanelController playerPanel2Controller = m_player2Panel.GetComponent<PlayerPanelController>();
            
                playerPanel1Controller.m_isDuo = true;
                playerPanel2Controller.m_isDuo = true;

                playerPanel1Controller.Init(gameplayManager.m_cacPlayerGamepad, CharacterEnum.MELEE);
                playerPanel2Controller.Init(gameplayManager.m_distPlayerGamepad, CharacterEnum.DISTANCE);

                if (m_isPlayer1PanelEnable)
                {
                    playerPanel2Controller.EnablePanel();
                    playerPanel1Controller.EnablePanel();
                    BlockPanelInput(m_player2Panel);
                }
                else
                {
                    playerPanel1Controller.EnablePanel();
                    playerPanel2Controller.EnablePanel();
                    BlockPanelInput(m_player1Panel);
                }

            }
        }

        public void OnDisable()
        {
            m_player1Panel.GetComponent<PlayerPanelController>().EnablePanel();
            m_player2Panel.GetComponent<PlayerPanelController>().EnablePanel();

            m_player2Panel.SetActive(false);
            m_player1Panel.SetActive(false);

            //EventSystem.current.gameObject.GetComponent<StandaloneInputModule>().submitButton = "Submit";
        }

        public void SwitchPanel()
        {
            //if(m_player1Panel.GetComponent<PlayerPanelController>().IsTranslating)
            //    return;


            m_isPlayer1PanelEnable = !m_isPlayer1PanelEnable;
            if (m_isPlayer1PanelEnable)
            {
                m_player2Panel.GetComponent<PlayerPanelController>().ShopPanelManager.DescriptorPanelManager.DisableBuyValidatorPanel();
                m_player2Panel.GetComponent<PlayerPanelController>().ShopPanelManager.DescriptorPanelManager.DisableNarrativePanel();
                EnablePlayerPanel2();
                BlockPanelInput(m_player2Panel);
                EnablePlayerPanel1();
                CalculateTranslationFromTo(m_player1Panel.GetComponent<PlayerPanelController>(), m_player2Panel.GetComponent<PlayerPanelController>());
                m_player1Panel.GetComponent<PlayerPanelController>().ShopPanelManager.UpdateCurrentRowFlaskInformations();
            }
            else
            {
                m_player1Panel.GetComponent<PlayerPanelController>().ShopPanelManager.DescriptorPanelManager.DisableBuyValidatorPanel();
                m_player1Panel.GetComponent<PlayerPanelController>().ShopPanelManager.DescriptorPanelManager.DisableNarrativePanel();
                EnablePlayerPanel1();
                BlockPanelInput(m_player1Panel);
                EnablePlayerPanel2();
                CalculateTranslationFromTo(m_player2Panel.GetComponent<PlayerPanelController>(), m_player1Panel.GetComponent<PlayerPanelController>());
                m_player2Panel.GetComponent<PlayerPanelController>().ShopPanelManager.UpdateCurrentRowFlaskInformations();
            }
        }

        private void CalculateTranslationFromTo(PlayerPanelController from, PlayerPanelController to)
        {
            //Vector3 origin = from.GetComponent<RectTransform>().transform.localPosition;
            //Vector3 destination = to.GetComponent<RectTransform>().transform.localPosition;

            Vector3 origin = from.PanelTransform;
            Vector3 destination = to.PanelTransform;

            Vector3 direction = destination - origin;
            from.TranslatePanel(direction);
            to.TranslatePanel(direction);
        }

        private void EnablePlayerPanel1()
        {
            m_player2Panel.GetComponent<PlayerPanelController>().DisablePanel();
            m_player1Panel.GetComponent<PlayerPanelController>().EnablePanel();
            BlockPanelInput(m_player2Panel);
        }
        private void EnablePlayerPanel2()
        {
            m_player1Panel.GetComponent<PlayerPanelController>().DisablePanel();
            m_player2Panel.GetComponent<PlayerPanelController>().EnablePanel();
            BlockPanelInput(m_player1Panel);
        }

        private void BlockPanelInput(GameObject panel)
        {
            panel.GetComponent<PlayerPanelController>().DisableAllPanelButtonAsSelectable();
        }
    }
}