using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace BTA
{
    public class PlayerSelectorManager : MonoBehaviour
    {
        private bool m_created = false;

        private InputManager m_inputManager = null;
        private GameplayManager m_gameplayManager = null;

        [SerializeField] private MainPanelUIManager m_mainPanelUIManager = null;

        [SerializeField] private GameObject m_soloBgGameObject;
        [SerializeField] private GameObject m_bothBgGameObject = null;

        [SerializeField] private Controller m_controller1 = null;
        [SerializeField] private Controller m_controller2 = null;

        [SerializeField] private PlayerDecorator m_meleeDecorator = null;
        [SerializeField] private PlayerDecorator m_distDecorator = null;

        [SerializeField] private GameObject m_soloDecorator = null;
        [SerializeField] private GameObject m_multiDecorator = null;

        private void Awake()
        {
            m_gameplayManager = GameManager.Instance.GetInstanceOf<GameplayManager>();
            m_inputManager = GameManager.Instance.GetInstanceOf<InputManager>();
        }

        void Start()
        {
            m_controller1.CurrentGamePad = GamePadID.Controller1;
            m_controller2.CurrentGamePad = GamePadID.Controller2;

            LinkControllerInput(GamePadID.Controller1, m_controller1);
            LinkControllerInput(GamePadID.Controller2, m_controller2);

            m_controller1.m_selectedCharacter.AddListener(SelectCharacter);
            m_controller2.m_selectedCharacter.AddListener(SelectCharacter);

            m_controller1.m_isMelee = true;

            if (GameManager.Instance.GetGameMode() == GameMode.Solo)
            {
                m_bothBgGameObject.SetActive(true);
                m_soloBgGameObject.SetActive(false);

                m_multiDecorator.SetActive(false);
                m_soloDecorator.SetActive(true);

                m_gameplayManager.m_cacPlayerGamepad = GamePadID.Controller1;
                m_gameplayManager.m_distPlayerGamepad = GamePadID.Controller1;
                m_meleeDecorator.CenterDecorator();
                m_distDecorator.CenterDecorator();
            }
            else
            {
                m_bothBgGameObject.SetActive(false);
                m_soloBgGameObject.SetActive(true);

                m_multiDecorator.SetActive(true);
                m_soloDecorator.SetActive(false);

                Debug.Log("Duo hotfix !");
                m_gameplayManager.m_cacPlayerGamepad = GamePadID.Controller1;
                m_gameplayManager.m_distPlayerGamepad = GamePadID.Controller2;
                m_meleeDecorator.SpreadDecorator();
                m_distDecorator.SpreadDecorator();
            }

            m_created = true;
        }

        private void OnEnable()
        {
            if (m_created)
            {
                LinkControllerInput(GamePadID.Controller1, m_controller1);
                LinkControllerInput(GamePadID.Controller2, m_controller2);
            }
            GameManager gameMgr = GameManager.Instance;
            //if (gameMgr.GetGameMode() == GameMode.Solo)
            //{
            //    Debug.Log("Solo hotfix !");
            //    m_gameplayManager.m_cacPlayerGamepad = GamePadID.Controller1;
            //    m_gameplayManager.m_distPlayerGamepad = GamePadID.Controller1;
            //    m_meleeDecorator.CenterDecorator();
            //    m_distDecorator.CenterDecorator();
            //}
            //else if (gameMgr.GetGameMode() == GameMode.Duo)
            //{
            //    Debug.Log("Duo hotfix !");
            //    m_gameplayManager.m_cacPlayerGamepad = GamePadID.Controller1;
            //    m_gameplayManager.m_distPlayerGamepad = GamePadID.Controller2;
            //    m_meleeDecorator.SpreadDecorator();
            //    m_distDecorator.SpreadDecorator();
            //}
        }

        private void OnDisable()
        {
            UnlinkControllerInput(GamePadID.Controller1);
            UnlinkControllerInput(GamePadID.Controller2);
        }

        void Update()
        {

        }

        public void AddPlayer()
        {
            UnlinkControllerInput(GamePadID.Controller1);
            UnlinkControllerInput(GamePadID.Controller2);

            LinkControllerInput(GamePadID.Controller1, m_controller1);
            LinkControllerInput(GamePadID.Controller2, m_controller2);


            m_bothBgGameObject.SetActive(false);
            m_soloBgGameObject.SetActive(true);

            m_multiDecorator.SetActive(true);
            m_soloDecorator.SetActive(false);

            m_meleeDecorator.SpreadDecorator();
            m_distDecorator.SpreadDecorator();
            m_gameplayManager.m_cacPlayerGamepad = GamePadID.Controller1;
            m_gameplayManager.m_distPlayerGamepad = GamePadID.Controller2;
            //m_controller1.OnMoveLeft();
            //m_controller2.OnMoveRight();
            EnableMenuButton();
        }

        public void RemovePlayer()
        {
            m_bothBgGameObject.SetActive(true);
            m_soloBgGameObject.SetActive(false);


            m_multiDecorator.SetActive(false);
            m_soloDecorator.SetActive(true);

            m_controller1.Reset();
            m_controller2.Reset();
            m_meleeDecorator.CenterDecorator();
            m_distDecorator.CenterDecorator();
            m_gameplayManager.m_cacPlayerGamepad = GamePadID.Controller1;
            m_gameplayManager.m_distPlayerGamepad = GamePadID.Controller1;
            EnableMenuButton();
        }

        private void LinkControllerInput(GamePadID gamePad, Controller controller)
        {
            InputEvent leftBumperEvent = m_inputManager.GetEvent(gamePad, GamePadInput.LeftBumper);
            InputEvent rightBumperEvent = m_inputManager.GetEvent(gamePad, GamePadInput.RightBumper);

            if (GameManager.Instance.GetGameMode() == GameMode.Duo)
            {
                leftBumperEvent.AddListener(controller.Swap);
                rightBumperEvent.AddListener(controller.Swap);
            }
            
        }

        private void UnlinkControllerInput(GamePadID gamePad)
        {
            m_inputManager.GetEvent(gamePad, GamePadInput.LeftBumper).RemoveAllListeners();
            m_inputManager.GetEvent(gamePad, GamePadInput.RightBumper).RemoveAllListeners();
        }

        private void SelectCharacter(GamePadID gamePad, CharacterEnum character)
        {
            if (character == CharacterEnum.MELEE)
                m_gameplayManager.m_cacPlayerGamepad = gamePad;
            else if (character == CharacterEnum.DISTANCE)
                m_gameplayManager.m_distPlayerGamepad = gamePad;

            EnableMenuButton();

            Debug.Log("Melee character is selected by ["+m_gameplayManager.m_cacPlayerGamepad+"]");
            Debug.Log("Distance character is selected by [" + m_gameplayManager.m_distPlayerGamepad + "]");
        }

        private CharacterEnum GetCharacterFromGamePad(GamePadID gamePad)
        {
            if (m_gameplayManager.m_cacPlayerGamepad == gamePad)
                return CharacterEnum.MELEE;
            else if (m_gameplayManager.m_distPlayerGamepad == gamePad)
                return CharacterEnum.DISTANCE;

            return CharacterEnum.NONE;
        }

        private GamePadID GetGamePadFromCharacter(CharacterEnum character)
        {
            if (character == CharacterEnum.DISTANCE)
                return m_gameplayManager.m_distPlayerGamepad;
            else if (character == CharacterEnum.MELEE)
                return m_gameplayManager.m_cacPlayerGamepad;

            return GamePadID.None;
        }

        private void EnableMenuButton()
        {
            if (GameManager.Instance.GetGameMode() == GameMode.Solo)
            {
                m_mainPanelUIManager.m_cityMapGameObject.GetComponent<Button>().interactable = true;
                return;
            }

            GamePadID cacGamePad = m_gameplayManager.m_cacPlayerGamepad;
            GamePadID distGamePad = m_gameplayManager.m_distPlayerGamepad;
        }
    }
}