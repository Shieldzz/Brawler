using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.Events;

namespace BTA
{

    public class PlayerPanelController : MonoBehaviour
    {
        [SerializeField] private GameObject m_flask1GameObject = null;
        [SerializeField] private GameObject m_flask2GameObject = null;
        [SerializeField] private GameObject m_flask3GameObject = null;
        [SerializeField] private GameObject m_narrativeGameObject = null;
        [SerializeField] private GameObject m_moneyIndicatorGameObject = null;

        [SerializeField] private DescriptorPanelManager m_descriptorPanelManager = null;
        [SerializeField] private GameObject m_pricesPanelGameObject = null;
        [SerializeField] private ShopPanelManager m_shopPanelManager = null;
        public ShopPanelManager ShopPanelManager { get { return m_shopPanelManager; } }
        public EssencePanelUIManager m_essencePanelUIManager = null;

        [SerializeField] private CharacterEnum m_currentCharacter = CharacterEnum.NONE;
        public CharacterEnum CurrentCharacter { get { return m_currentCharacter; } }
        [SerializeField] private GamePadID m_currentGamePad = GamePadID.None;
        public GamePadID CurrentGamePad { get { return m_currentGamePad; } }

        public bool m_isDuo = false;
        private Vector3 m_panelTransform = Vector3.zero;
        public Vector3 PanelTransform { get { return m_panelTransform; } }
        private bool m_isTranslating = false;
        public bool IsTranslating { get { return m_isTranslating; } }
        private Vector3 m_velocity = Vector3.zero;
        private float m_smoothTime = 0.3f;

        private void Awake()
        {
        }

        void Start()
        {
            m_panelTransform = GetComponent<RectTransform>().transform.localPosition;
        }

        void Update()
        {
            if (m_isTranslating)
            {
                SmoothTranslation();
            }
        }

        public void OnEnable()
        {
            
        }

        private void SmoothTranslation()
        {
            RectTransform rect = GetComponent<RectTransform>();
            rect.transform.localPosition = Vector3.SmoothDamp(rect.transform.localPosition, m_panelTransform, ref m_velocity, m_smoothTime);
            if (rect.transform.localPosition == m_panelTransform)
                m_isTranslating = false;
        }

        public void TranslatePanel(Vector3 direction)
        {
            m_panelTransform += direction;
            m_isTranslating = true; 
        }   

        public FlaskData[] GetEquipedFlask()
        {
            FlaskData[] flasks = new FlaskData[3];

            EquipedFlask equipedFlask1 = m_flask1GameObject.GetComponent<EquipedFlask>();
            EquipedFlask equipedFlask2 = m_flask2GameObject.GetComponent<EquipedFlask>();
            EquipedFlask equipedFlask3 = m_flask3GameObject.GetComponent<EquipedFlask>();

            if (equipedFlask1.m_equipedFlask)
                flasks[0] = equipedFlask1.m_equipedFlask;
            if (equipedFlask2.m_equipedFlask)
                flasks[1] = equipedFlask2.m_equipedFlask;
            if (equipedFlask3.m_equipedFlask)
                flasks[2] = equipedFlask3.m_equipedFlask;

            return flasks;
        }

        public void Init(GamePadID gamePad, CharacterEnum characterEnum)
        {
            m_currentCharacter = characterEnum;
            m_currentGamePad = gamePad;
        }

        public void EnablePanel()
        {
            LinkFlaskEventWithShop();
            SetEventSystemForBegining();
        }

        public void DisablePanel()
        {
            UnlinkFlaskEventWithShop();
        }

        private void LinkFlaskEventWithShop()
        {
            m_flask1GameObject.GetComponent<EquipedFlask>().m_onClick.AddListener(ActivateShop);
            m_flask2GameObject.GetComponent<EquipedFlask>().m_onClick.AddListener(ActivateShop);
            m_flask3GameObject.GetComponent<EquipedFlask>().m_onClick.AddListener(ActivateShop);
        }

        private void UnlinkFlaskEventWithShop()
        {
            m_flask1GameObject.GetComponent<EquipedFlask>().m_onClick.RemoveAllListeners();
            m_flask2GameObject.GetComponent<EquipedFlask>().m_onClick.RemoveAllListeners();
            m_flask3GameObject.GetComponent<EquipedFlask>().m_onClick.RemoveAllListeners();
        }

        public void SetEventSystemForBegining()
        {
            SetFocusOnFlask();
        }

        private void DisableFlaskButtonAsSelectable()
        {
            m_flask1GameObject.GetComponent<EquipedFlask>().interactable = false;
            m_flask2GameObject.GetComponent<EquipedFlask>().interactable = false;
            m_flask3GameObject.GetComponent<EquipedFlask>().interactable = false;
            m_narrativeGameObject.GetComponent<Button>().interactable = false;

            m_flask1GameObject.GetComponent<EquipedFlask>().FlaskDecorator.GetComponent<Button>().interactable = false;
            m_flask2GameObject.GetComponent<EquipedFlask>().FlaskDecorator.GetComponent<Button>().interactable = false;
            m_flask3GameObject.GetComponent<EquipedFlask>().FlaskDecorator.GetComponent<Button>().interactable = false;
            m_narrativeGameObject.GetComponent<NarrativeFlaskEmplacement>().FlaskDecorator.GetComponent<Button>().interactable = false;
        }

        private void EnableFlaskButtonAsSelectable()
        {
            m_flask1GameObject.GetComponent<EquipedFlask>().EnableAsInteractable();
            m_flask2GameObject.GetComponent<EquipedFlask>().EnableAsInteractable();
            m_flask3GameObject.GetComponent<EquipedFlask>().EnableAsInteractable();
            m_narrativeGameObject.GetComponent<NarrativeFlaskEmplacement>().interactable = true;

            m_flask1GameObject.GetComponent<EquipedFlask>().FlaskDecorator.GetComponent<Button>().interactable = true;
            m_flask2GameObject.GetComponent<EquipedFlask>().FlaskDecorator.GetComponent<Button>().interactable = true;
            m_flask3GameObject.GetComponent<EquipedFlask>().FlaskDecorator.GetComponent<Button>().interactable = true;
            m_narrativeGameObject.GetComponent<NarrativeFlaskEmplacement>().FlaskDecorator.GetComponent<Button>().interactable = true;

            EventSystem.current.SetSelectedGameObject(m_flask1GameObject);
            m_flask1GameObject.GetComponent<EquipedFlask>().OnSelect(null);
        }

        private void ActivateShop(EquipedFlask equipedFlask)
        {
            SetFocusOnShop();
            m_shopPanelManager.m_currentFlaskEmplacement = equipedFlask;
        }

        public void SetFocusOnShop()
        {
            DisableFlaskButtonAsSelectable();
            m_shopPanelManager.SetToolbarDataOnShop();
            m_shopPanelManager.SetEventSystemFocus();
        }

        public void SetFocusOnFlask()
        {
            EnableFlaskButtonAsSelectable();
            m_shopPanelManager.DisableAllButtonsAsSelectable();
            EventSystem.current.SetSelectedGameObject(m_flask1GameObject);
            SetToolbarDataOnFlask();
            m_flask1GameObject.GetComponent<EquipedFlask>().OnSelect(new BaseEventData(EventSystem.current));
            m_descriptorPanelManager.DescribeFlask(m_flask1GameObject.GetComponent<EquipedFlask>().m_equipedFlask);
        }

        private void SetToolbarDataOnFlask()
        {
            InputManager inputManager = GameManager.Instance.GetInstanceOf<InputManager>();
            HelperUIManager helperManager = HelperUIManager.Instance;
            helperManager.DisableAllHelper();
            helperManager.ClearAllSpecificControllerActions(CurrentGamePad);

            helperManager.EnableHelper(helperManager.m_helper1GameObject);
            helperManager.SetHelperInformation(helperManager.m_helper1GameObject, helperManager.m_spriteIconA, "Confirm");

            helperManager.EnableHelper(helperManager.m_helper5GameObject);
            helperManager.SetHelperInformation(helperManager.m_helper5GameObject, helperManager.m_spriteIconLbRb, "Change Side");
            inputManager.GetEvent(CurrentGamePad, GamePadInput.LeftBumper).AddListener(m_essencePanelUIManager.SwitchPanel);
            inputManager.GetEvent(CurrentGamePad, GamePadInput.RightBumper).AddListener(m_essencePanelUIManager.SwitchPanel);

            if (CurrentGamePad == GamePadID.Controller1)
            {
                helperManager.EnableHelper(helperManager.m_helper2GameObject);
                helperManager.SetHelperInformation(helperManager.m_helper2GameObject, helperManager.m_spriteIconB, "Cancel", helperManager.m_controller1Helper2Action += MenuGUIManager.Instance.EnableMainPanel, GamePadID.Controller1);
            }
            else if(CurrentGamePad == GamePadID.Controller2)
            {
                helperManager.EnableHelper(helperManager.m_helper2GameObject);
                helperManager.SetHelperInformation(helperManager.m_helper2GameObject, helperManager.m_spriteIconB, "Cancel", helperManager.m_controller2Helper2Action += MenuGUIManager.Instance.EnableMainPanel, GamePadID.Controller2);
            }
        }

        public void DisableAllPanelButtonAsSelectable()
        {
            DisableFlaskButtonAsSelectable();
            m_shopPanelManager.DisableAllButtonsAsSelectable();
            m_shopPanelManager.DisableScrolling();
            if(m_isDuo)
                DisableCustomEvent();
        }

        private void DisableCustomEvent()
        {
            InputManager inputManager = GameManager.Instance.GetInstanceOf<InputManager>();
            HelperUIManager helperManager = HelperUIManager.Instance;

            if (CurrentGamePad == GamePadID.Controller1)
            {
                helperManager.ClearAction(ref helperManager.m_controller1Helper1Action, CurrentGamePad, GamePadInput.ButtonA);
                helperManager.ClearAction(ref helperManager.m_controller1Helper2Action, CurrentGamePad, GamePadInput.ButtonB);
                helperManager.ClearAction(ref helperManager.m_controller1Helper3Action, CurrentGamePad, GamePadInput.ButtonX);
                helperManager.ClearAction(ref helperManager.m_controller1Helper4Action, CurrentGamePad, GamePadInput.ButtonY);
            }
            else if (CurrentGamePad == GamePadID.Controller2)
            {
                helperManager.ClearAction(ref helperManager.m_controller2Helper1Action, CurrentGamePad, GamePadInput.ButtonA);
                helperManager.ClearAction(ref helperManager.m_controller2Helper2Action, CurrentGamePad, GamePadInput.ButtonB);
                helperManager.ClearAction(ref helperManager.m_controller2Helper3Action, CurrentGamePad, GamePadInput.ButtonX);
                helperManager.ClearAction(ref helperManager.m_controller2Helper4Action, CurrentGamePad, GamePadInput.ButtonY);
            }
        }

        private void EnableCancelEvent()
        {
            InputManager inputManager = GameManager.Instance.GetInstanceOf<InputManager>();
            HelperUIManager helperManager = HelperUIManager.Instance;

            if (CurrentGamePad == GamePadID.Controller1)
            {
                helperManager.ClearAction(ref helperManager.m_controller1Helper2Action, CurrentGamePad, GamePadInput.ButtonB);
                helperManager.SetHelperInformation(helperManager.m_helper2GameObject, helperManager.m_spriteIconB, "Cancel", helperManager.m_controller1Helper2Action += MenuGUIManager.Instance.EnableMainPanel, CurrentGamePad);
            }
            else if (CurrentGamePad == GamePadID.Controller2)
            {
                helperManager.ClearAction(ref helperManager.m_controller2Helper2Action, CurrentGamePad, GamePadInput.ButtonB);
                helperManager.SetHelperInformation(helperManager.m_helper2GameObject, helperManager.m_spriteIconB, "Cancel", helperManager.m_controller2Helper2Action += MenuGUIManager.Instance.EnableMainPanel, CurrentGamePad);
            }
        }

    }
}