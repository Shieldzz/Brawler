using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace BTA
{

    public class ValidatorPanel : MonoBehaviour
    {
        [SerializeField] private DescriptorPanelManager m_descriptorPanel = null;
        [SerializeField] private Text m_title = null;
        [SerializeField] private Text m_selectedEssenceName = null;
        [SerializeField] private Text m_selectedEssencePrice = null;
        [SerializeField] private BuyButton m_buyButton = null;
        [SerializeField] private Image m_flaskImage = null;
        [SerializeField] private Text m_currentMoney = null;
        [SerializeField] private Text m_moneyLeft = null;

        private EquiperFlask m_currentEquiperFlask = null;

        // Use this for initialization
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }

        public void DisplayFlaskData(EquiperFlask equiper)
        {
            m_descriptorPanel.PlayerPanelController.ShopPanelManager.DisableAllButtonsAsSelectable();
            m_descriptorPanel.PlayerPanelController.ShopPanelManager.DisableScrolling();

            float tmpMoneyData = GameManager.Instance.GetInstanceOf<GameplayManager>().Money;

            m_currentEquiperFlask = equiper;
            m_title.text = equiper.m_flaskData.flaskName;
            m_flaskImage.sprite = equiper.m_flaskData.m_flaskSprite;
            DisplayFlaskDialogData(equiper.m_flaskData.flaskName, equiper.m_flaskData.price);
            DisplayPriceTransaction(tmpMoneyData, equiper.m_flaskData.price);
            SetFocus();
            if (tmpMoneyData - equiper.m_flaskData.price < 0)
                LinkEvent(false);
            else
                LinkEvent();
        }

        public void Close()
        {
            UnlinkEvent();
        }

        private void DisplayFlaskDialogData(string flaskName, float flaskPrice)
        {
            m_selectedEssenceName.text = flaskName;
            m_selectedEssencePrice.text = flaskPrice.ToString();
        }

        private void DisplayPriceTransaction(float playerMoney, float flaskPrice)
        {
            float res = playerMoney - flaskPrice;
            m_currentMoney.text = playerMoney.ToString();
            m_moneyLeft.text = res.ToString();
            //string toPrint = playerMoney.ToString() + "$ > " + res.ToString() + "$";
            //m_priceTransactionGameObject.GetComponent<Text>().text = toPrint;
        }

        private void SetFocus()
        {
            EventSystem.current.SetSelectedGameObject(m_buyButton.gameObject);
        }

        private void LinkEvent(bool canBuy = true)
        {
            HelperUIManager helperManager = HelperUIManager.Instance;
            helperManager.DisableAllHelper();
            GamePadID currentGamePad = m_descriptorPanel.PlayerPanelController.CurrentGamePad;
            helperManager.ClearAllSpecificControllerActions(currentGamePad);

            HoldToBuy holdToBuy = m_buyButton.GetComponent<HoldToBuy>();

            
            helperManager.EnableHelper(helperManager.m_helper1GameObject);
            helperManager.ClearAction(ref helperManager.m_controller1Helper1Action, currentGamePad, GamePadInput.ButtonA);

            InputManager inputManager = GameManager.Instance.GetInstanceOf<InputManager>();
            inputManager.GetEvent(currentGamePad, GamePadInput.ButtonA).AddListener(holdToBuy.StartHolding);
            inputManager.GetEvent(currentGamePad, GamePadInput.ButtonARelease).AddListener(holdToBuy.StopHolding);

            if (currentGamePad == GamePadID.Controller1)
            {
                //helperManager.ClearAction(ref helperManager.m_controller1Helper1Action, currentGamePad, GamePadInput.ButtonA);
                //helperManager.SetHelperInformation(helperManager.m_helper1GameObject, helperManager.m_spriteIconA, "Hold to buy", helperManager.m_controller1Helper1Action += m_buyButton.CustonSubmitCaller, GamePadID.Controller1);
                helperManager.SetHelperInformation(helperManager.m_helper1GameObject, helperManager.m_spriteIconA, "Hold to buy");

                helperManager.SetHelperInformation(helperManager.m_helper2GameObject, helperManager.m_spriteIconB, "Cancel", helperManager.m_controller1Helper2Action += m_descriptorPanel.DisableBuyValidatorPanel, currentGamePad);
                helperManager.EnableHelper(helperManager.m_helper2GameObject);
            }
            else if(currentGamePad == GamePadID.Controller2)
            {
                //helperManager.ClearAction(ref helperManager.m_controller2Helper1Action, currentGamePad, GamePadInput.ButtonA);
                //helperManager.SetHelperInformation(helperManager.m_helper1GameObject, helperManager.m_spriteIconA, "Hold to buy", helperManager.m_controller2Helper1Action += m_buyButton.CustonSubmitCaller, GamePadID.Controller2);
                helperManager.SetHelperInformation(helperManager.m_helper1GameObject, helperManager.m_spriteIconA, "Hold to buy");

                helperManager.SetHelperInformation(helperManager.m_helper2GameObject, helperManager.m_spriteIconB, "Cancel", helperManager.m_controller2Helper2Action += m_descriptorPanel.DisableBuyValidatorPanel, currentGamePad);
                helperManager.EnableHelper(helperManager.m_helper2GameObject);
            }

            if(canBuy)
                m_buyButton.onClick.AddListener(m_currentEquiperFlask.Buy);
        }

        private void UnlinkEvent()
        {
            HelperUIManager helperUIManager = HelperUIManager.Instance;
            helperUIManager.DisableAllHelper();
            GamePadID currentGamePad = m_descriptorPanel.PlayerPanelController.CurrentGamePad;
            helperUIManager.ClearAllSpecificControllerActions(currentGamePad);

            m_buyButton.onClick.RemoveAllListeners();

            InputManager inputManager = GameManager.Instance.GetInstanceOf<InputManager>();
            inputManager.GetEvent(currentGamePad, GamePadInput.ButtonA).RemoveAllListeners();
            inputManager.GetEvent(currentGamePad, GamePadInput.ButtonARelease).RemoveAllListeners();
        }
    }
}