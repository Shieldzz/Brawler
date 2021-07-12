using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Events;
using UnityEngine.UI;

namespace BTA
{

    public class EquipedFlask : Button
    {

        public class EquipedFlaskClickEvent : UnityEvent<EquipedFlask> { }
        public EquipedFlaskClickEvent m_onClick = new EquipedFlaskClickEvent();

        public DescriptorPanelManager m_descriptorPanel = null;
        public FlaskData m_equipedFlask = null;
        public int m_equipedIndex = -1;
        public Sprite m_unequipSprite = null;

        private EquiperFlask m_currentEquiper = null;
        [SerializeField] private PlayerPanelController m_playerPanelController = null;
        [SerializeField] private Image m_flaskImage = null;
        public GameObject FlaskDecorator { get { return m_flaskImage.gameObject; } }
        private Vector3 m_flaskImagePos = Vector3.zero;

        private bool m_stayOnHighlight = false;

        protected override void Start()
        {
            base.Start();

            

            m_unequipSprite = m_flaskImage.sprite;
            FlaskManager flaskManager = FlaskManager.Instance;

            m_flaskImagePos = m_flaskImage.rectTransform.localPosition;

            CharacterEnum character = m_descriptorPanel.PlayerPanelController.CurrentCharacter;
            if(character == CharacterEnum.MELEE)
                m_equipedFlask = flaskManager.m_meleeEquipedFlaskData[m_equipedIndex];
            else if (character == CharacterEnum.DISTANCE)
                m_equipedFlask = flaskManager.m_distanceEquipedFlaskData[m_equipedIndex];

            m_currentEquiper = m_descriptorPanel.PlayerPanelController.ShopPanelManager.GetEquiperFromFlask(m_equipedFlask);
            if (m_currentEquiper)
                Equip(m_currentEquiper);
        }

        void Update()
        {

        }

        private void custonClickCaller()
        {
            OnSubmit(new BaseEventData(EventSystem.current));
        }

        override public void OnSelect(BaseEventData eventData)
        {
            HelperUIManager helperManager = HelperUIManager.Instance;
            m_descriptorPanel.DescribeFlask(m_equipedFlask);
            m_descriptorPanel.ExplainFlask(m_equipedFlask);
            helperManager.EnableHelper(helperManager.m_helper3GameObject);
            helperManager.EnableHelper(helperManager.m_helper4GameObject);

            //InputManager m_inputManager = GameManager.Instance.GetInstanceOf<InputManager>();
            //
            //m_inputManager.GetEvent(m_playerPanelController.CurrentGamePad, GamePadInput.ButtonA).RemoveAllListeners();
            //m_inputManager.GetEvent(m_playerPanelController.CurrentGamePad, GamePadInput.ButtonA).AddListener(custonClickCaller);

            if (m_playerPanelController.CurrentGamePad == GamePadID.Controller1)
            {
                helperManager.ClearAction(ref helperManager.m_controller1Helper1Action, GamePadID.Controller1, GamePadInput.ButtonA);
                helperManager.SetHelperInformation(helperManager.m_helper1GameObject, helperManager.m_spriteIconA, "Validate", helperManager.m_controller1Helper1Action += custonClickCaller, GamePadID.Controller1);

                helperManager.ClearAction(ref helperManager.m_controller1Helper3Action, m_playerPanelController.CurrentGamePad, GamePadInput.ButtonX);
                helperManager.SetHelperInformation(helperManager.m_helper3GameObject, helperManager.m_spriteIconX, "Remove", helperManager.m_controller1Helper3Action += Unequip, GamePadID.Controller1);

                helperManager.ClearAction(ref helperManager.m_controller1Helper4Action, m_playerPanelController.CurrentGamePad, GamePadInput.ButtonY);
                helperManager.SetHelperInformation(helperManager.m_helper4GameObject, helperManager.m_spriteIconY, "Toggle Details", helperManager.m_controller1Helper4Action += m_descriptorPanel.EnableDisableNarrativePanelFromEquiped, GamePadID.Controller1);
            }
            else if (m_playerPanelController.CurrentGamePad == GamePadID.Controller2)
            {
                helperManager.ClearAction(ref helperManager.m_controller2Helper1Action, GamePadID.Controller2, GamePadInput.ButtonA);
                helperManager.SetHelperInformation(helperManager.m_helper1GameObject, helperManager.m_spriteIconA, "Validate", helperManager.m_controller2Helper1Action += custonClickCaller, GamePadID.Controller2);

                helperManager.ClearAction(ref helperManager.m_controller2Helper3Action, m_playerPanelController.CurrentGamePad, GamePadInput.ButtonX);
                helperManager.SetHelperInformation(helperManager.m_helper3GameObject, helperManager.m_spriteIconX, "Remove", helperManager.m_controller2Helper3Action += Unequip, GamePadID.Controller2);

                helperManager.ClearAction(ref helperManager.m_controller2Helper4Action, m_playerPanelController.CurrentGamePad, GamePadInput.ButtonY);
                helperManager.SetHelperInformation(helperManager.m_helper4GameObject, helperManager.m_spriteIconY, "Toggle Details", helperManager.m_controller2Helper4Action += m_descriptorPanel.EnableDisableNarrativePanelFromEquiped, GamePadID.Controller2);
            }

            m_flaskImage.GetComponent<Button>().OnSelect(null);
            base.OnSelect(eventData);
        }

        public override void OnDeselect(BaseEventData eventData)
        {
            m_flaskImage.GetComponent<Button>().OnDeselect(null);
            base.OnDeselect(eventData);
        }

        protected override void OnDisable()
        {
            m_flaskImage.GetComponent<Button>().OnDeselect(null);
            m_flaskImage.GetComponent<RectTransform>().localPosition = m_flaskImagePos;
        }

        public override void OnSubmit(BaseEventData eventData)
        {
            m_flaskImage.GetComponent<Animator>().SetBool("StayHighlight", true);
            m_onClick.Invoke(this);
        }

        public void EnableAsInteractable()
        {
            interactable = true;
            m_flaskImage.GetComponent<Animator>().SetBool("StayHighlight", false);
        }

        public void Equip(EquiperFlask equiper)
        {
            Unequip();
            m_currentEquiper = equiper;
            m_equipedFlask = equiper.m_flaskData;
            equiper.m_isEquiped = true;

            FlaskManager flaskManager = FlaskManager.Instance;
            flaskManager.AddFlaskDataToPlayer(m_playerPanelController.CurrentCharacter, m_equipedFlask, m_equipedIndex);

            if (equiper.m_flaskData.m_flaskSprite)
                m_flaskImage.sprite = equiper.m_flaskData.m_flaskSprite;
        }

        public void Unequip()
        {
            if (!m_currentEquiper)
                return;

            m_currentEquiper.m_isEquiped = false;
            m_currentEquiper = null;
            m_equipedFlask = null;
            m_descriptorPanel.DescribeFlask(null);
            FlaskManager flaskManager = FlaskManager.Instance;
            flaskManager.RemoveEquipedFlask(m_equipedIndex, m_playerPanelController.CurrentCharacter);
            m_flaskImage.sprite = m_unequipSprite;
            //GetComponent<Image>().sprite = m_unequipSprite;
            //TODO: Use variable to know if it's player1 or player2
        }
    }
}