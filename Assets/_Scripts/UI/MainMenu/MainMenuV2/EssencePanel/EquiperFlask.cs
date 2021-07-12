using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace BTA
{
    public class EquiperFlask : Button
    {
        [SerializeField] private Text m_priceIndicator = null;
        public Text PriceIndicator { get { return m_priceIndicator; } }
        private PriceIndicator m_realPriceIndicator;


        public class EquiperFlaskClickEvent : UnityEvent<EquiperFlask> { }
        public EquiperFlaskClickEvent m_onClick = new EquiperFlaskClickEvent();
        public Image m_flaskChildImage = null;

        public DescriptorPanelManager m_descriptorPanel = null;
        public FlaskData m_flaskData = null;
        public bool m_isBuy = false;
        public bool m_isEquiped = false;
        private Sprite m_basicSprite = null;
        public Sprite m_selectSprite = null;

        protected override void Start()
        {
            base.Start();
            m_realPriceIndicator = m_priceIndicator.GetComponent<PriceIndicator>();

            if (!m_flaskData)
            {
                gameObject.SetActive(false);
                m_priceIndicator.gameObject.SetActive(false);
                return;
            }

            m_basicSprite = GetComponent<Image>().sprite;

            if (m_flaskData.m_flaskSprite)
            {
                m_flaskChildImage.sprite = m_flaskData.m_flaskSprite;
                UpdateBuyFlask();
            }
        }

        void Update()
        {

        }

        public void UpdateBuyFlask()
        {
            if (!m_flaskData)
            {
                m_priceIndicator.gameObject.SetActive(false);
                gameObject.SetActive(false);
                return;
            }

            m_priceIndicator.gameObject.SetActive(true);
            FlaskManager flaskManager = FlaskManager.Instance;
            if (flaskManager.m_ownFlask.Contains(m_flaskData))
            {
                m_isBuy = true;
                m_onClick.RemoveAllListeners();
                m_onClick.AddListener(m_descriptorPanel.PlayerPanelController.ShopPanelManager.EquipFlask);
            }
            SetPriceIndicator();
        }

        private void CustomSubmitCaller()
        {
            OnSubmit(new BaseEventData(EventSystem.current));
        }

        override public void OnSelect(BaseEventData eventData)
        {
            FlaskManager flaskManager = FlaskManager.Instance;
            if (flaskManager.m_ownFlask.Contains(m_flaskData))
                m_isBuy = true;

            HelperUIManager helperManager = HelperUIManager.Instance;

            helperManager.SetHelperInformation(helperManager.m_helper1GameObject, helperManager.m_spriteIconA, "Buy");
            m_descriptorPanel.InspectSelectedFlask(m_flaskData);
            m_descriptorPanel.ExplainFlask(m_flaskData);

            GamePadID currentController = m_descriptorPanel.PlayerPanelController.CurrentGamePad;

            if (currentController == GamePadID.Controller1)
            {
                helperManager.ClearAction(ref helperManager.m_controller1Helper1Action, currentController, GamePadInput.ButtonA);
                helperManager.SetHelperInformation(helperManager.m_helper1GameObject, helperManager.m_spriteIconA, "Buy", helperManager.m_controller1Helper1Action += CustomSubmitCaller, GamePadID.Controller1);

                helperManager.ClearAction(ref helperManager.m_controller1Helper4Action, currentController, GamePadInput.ButtonY);
                helperManager.EnableHelper(helperManager.m_helper4GameObject);
                helperManager.SetHelperInformation(helperManager.m_helper4GameObject, helperManager.m_spriteIconY, "Toggle Details", helperManager.m_controller1Helper4Action += m_descriptorPanel.EnableDisableNarrativePanelFromEquiper, GamePadID.Controller1);
            }
            else if (currentController == GamePadID.Controller2)
            {
                helperManager.ClearAction(ref helperManager.m_controller2Helper1Action, currentController, GamePadInput.ButtonA);
                helperManager.SetHelperInformation(helperManager.m_helper1GameObject, helperManager.m_spriteIconA, "Buy", helperManager.m_controller2Helper1Action += CustomSubmitCaller, GamePadID.Controller2);

                helperManager.ClearAction(ref helperManager.m_controller2Helper4Action, currentController, GamePadInput.ButtonY);
                helperManager.EnableHelper(helperManager.m_helper4GameObject);
                helperManager.SetHelperInformation(helperManager.m_helper4GameObject, helperManager.m_spriteIconY, "Toggle Details", helperManager.m_controller2Helper4Action += m_descriptorPanel.EnableDisableNarrativePanelFromEquiper, GamePadID.Controller2);
            }

            if (m_isBuy)
                helperManager.SetHelperInformation(helperManager.m_helper1GameObject, helperManager.m_spriteIconA, "Equip");

            GetComponent<Image>().sprite = m_selectSprite;

            base.OnSelect(eventData);
        }

        public override void OnDeselect(BaseEventData eventData)
        {
            GetComponent<Image>().sprite = m_basicSprite;
            base.OnDeselect(eventData);
        }

        public override void OnSubmit(BaseEventData eventData)
        {
            m_onClick.Invoke(this);
        }

        public void Buy()
        {
            m_isBuy = true;
            m_descriptorPanel.DisableBuyValidatorPanel();

            GameplayManager gameplayManager = GameManager.Instance.GetInstanceOf<GameplayManager>();
            GameManager.Instance.GetInstanceOf<GameplayManager>().Money = gameplayManager.Money - m_flaskData.price;
            FlaskManager.Instance.OwnNewFlask(m_flaskData);
        }

        public void SetPriceIndicator()
        {
            if (!m_flaskData)
            {
                m_realPriceIndicator.SetNone();
                return;
            }

            if (m_flaskData.m_isLock)
            {
                m_realPriceIndicator.SetLock();
                return;
            }

            if (m_isBuy)
            {
                m_realPriceIndicator.SetOwned();
                return;
            }

            m_realPriceIndicator.SetPrice(m_flaskData.price);
        }
    }
}