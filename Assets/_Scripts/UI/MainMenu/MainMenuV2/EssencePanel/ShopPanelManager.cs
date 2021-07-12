using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;


namespace BTA
{
    using ScrollerPage = MoveScrollRect.ScrollerPage;

    public class ShopPanelManager : MonoBehaviour
    {

        [SerializeField] private PlayerPanelController m_playerPanelController = null;
        public PlayerPanelController PlayerPanelController { get { return m_playerPanelController; } }
        [SerializeField] private DescriptorPanelManager m_descriptorPanelManager = null;
        public DescriptorPanelManager DescriptorPanelManager { get { return m_descriptorPanelManager; } }

        public Image m_flaskTypeImage = null;
        public Sprite m_utilityFlaskIcon = null;
        public Sprite m_styleFlaskIcon = null;
        public Sprite m_comboFlaskIcon = null;

        public GameObject[] m_upperButtonGameObject = null;
        public GameObject[] m_middleButtonGameObject = null;
        public GameObject[] m_downButtonGameObject = null;

        public EquipedFlask m_currentFlaskEmplacement = null;

        public ShopCategoryTab m_utilityTab = null;
        public ShopCategoryTab m_comboTab = null;
        public ShopCategoryTab m_styleTab = null;

        private ScrollerPage m_lastPage = ScrollerPage.UPPER;

        void Start()
        {
            //  UpdateCurrentRowFlaskInformations();
        }

        void Update()
        {
        }

        private void OnEnable()
        {
            LinkButtonsEventWithFlaskSelector();
        }

        private void OnDisable()
        {
            UnlinkButtonsEventWithFlaskSelector();
        }

        public void EnableAllButtonsAsSelectable()
        {
            foreach (GameObject gao in m_upperButtonGameObject)
            {
                EquiperFlask equiper = gao.GetComponent<EquiperFlask>();
                if (!equiper.m_isEquiped)
                    equiper.interactable = true;
            }
                

            foreach (GameObject gao in m_middleButtonGameObject)
            {
                EquiperFlask equiper = gao.GetComponent<EquiperFlask>();
                if (!equiper.m_isEquiped)
                    equiper.interactable = true;
            }

            foreach (GameObject gao in m_downButtonGameObject)
            {
                EquiperFlask equiper = gao.GetComponent<EquiperFlask>();
                if (!equiper.m_isEquiped)
                    equiper.interactable = true;
            }
        }

        private void EnableButtonRowAsSelectable(GameObject[] buttonRow)
        {
            foreach (GameObject gao in buttonRow)
            {
                if (!gao.activeSelf)
                    continue;
                EquiperFlask equiper = gao.GetComponent<EquiperFlask>();
                if (!equiper.m_isEquiped)
                    equiper.interactable = true;
            }
        }

        public EquiperFlask GetEquiperFromFlask(FlaskData data)
        {
            if (!data)
                return null;

            foreach(GameObject gao in m_upperButtonGameObject)
            {
                if (!gao.activeSelf)
                    continue;
                EquiperFlask equiper = gao.GetComponent<EquiperFlask>();
                if (equiper.m_flaskData == data)
                    return equiper;
            }

            foreach (GameObject gao in m_middleButtonGameObject)
            {
                if (!gao.activeSelf)
                    continue;
                EquiperFlask equiper = gao.GetComponent<EquiperFlask>();
                if (equiper.m_flaskData == data)
                    return equiper;
            }

            foreach (GameObject gao in m_downButtonGameObject)
            {
                if (!gao.activeSelf)
                    continue;
                EquiperFlask equiper = gao.GetComponent<EquiperFlask>();
                if (equiper.m_flaskData == data)
                    return equiper;
            }

            return null;
        }

        public void DisableAllButtonsAsSelectable()
        {
            foreach (GameObject gao in m_upperButtonGameObject)
                gao.GetComponent<EquiperFlask>().interactable = false;

            foreach (GameObject gao in m_middleButtonGameObject)
                gao.GetComponent<EquiperFlask>().interactable = false;

            foreach (GameObject gao in m_downButtonGameObject)
                gao.GetComponent<EquiperFlask>().interactable = false;
        }

        private void DisableButtonRowAsSelectable(GameObject[] buttonRow)
        {
            foreach (GameObject gao in buttonRow)
                gao.GetComponent<EquiperFlask>().interactable = false;
        }

        private GameObject GetFirstButtonSelectable(GameObject[] m_buttonList)
        {
            foreach (GameObject gao in m_buttonList)
            {
                if (!gao.activeSelf)
                    continue;

                if (gao.GetComponent<EquiperFlask>().m_isEquiped)
                    continue;

                return gao;
            }
            return null;
        }

        private void EnableAllButtonInListAsSelectable(GameObject[] list)
        {
            foreach (GameObject gao in list)
            {
                if (!gao.activeSelf)
                    continue;

                if (gao.GetComponent<EquiperFlask>().m_isEquiped)
                    continue;

                gao.GetComponent<EquiperFlask>().interactable = true;
            }
        }

        private void OnNewScrollPage(ScrollerPage page)
        {

            GameObject[] buttonList = GetButtonListFromScrollPage(m_lastPage);

            DisableButtonRowAsSelectable(buttonList);

            m_lastPage = page;
            SetEventSystemFocus();
            //UpdateCurrentRowFlaskInformations();
        }

        private void SetFlaskIcon()
        {
            ResetShopTabIcon();

            if (m_lastPage == ScrollerPage.BOTTOM)
                m_styleTab.OnTabSelected();
            else if (m_lastPage == ScrollerPage.MIDDLE)
                m_comboTab.OnTabSelected();
            else
                m_utilityTab.OnTabSelected();
        }

        private void ResetShopTabIcon()
        {
            m_comboTab.OnTabUnselected();
            m_styleTab.OnTabUnselected();
            m_utilityTab.OnTabUnselected();
        }

        private GameObject[] GetButtonListFromScrollPage(ScrollerPage page)
        {
            if (page == ScrollerPage.BOTTOM)
                return m_downButtonGameObject;
            else if (page == ScrollerPage.MIDDLE)
                return m_middleButtonGameObject;
            else
                return m_upperButtonGameObject;
        }

        public void UpdateCurrentRowFlaskInformations()
        {
            GameObject[] list = GetButtonListFromScrollPage(m_lastPage);
            foreach (GameObject gao in list)
            {
                if(gao.activeSelf)
                    gao.GetComponent<EquiperFlask>().UpdateBuyFlask();
                else
                    gao.GetComponent<EquiperFlask>().PriceIndicator.gameObject.SetActive(false);

            }
        }

        public void SetEventSystemFocus()
        {
            GameObject[] buttonList = GetButtonListFromScrollPage(m_lastPage);
            EnableAllButtonInListAsSelectable(buttonList);
            UpdateCurrentRowFlaskInformations();
            FocusEventOnGameObject(GetFirstButtonSelectable(buttonList));
            SetFlaskIcon();
        }

        public void UpdatePriceIndicator()
        {
            GameObject[] buttonList = GetButtonListFromScrollPage(m_lastPage);
            
            foreach(GameObject gao in buttonList)
                gao.GetComponent<EquiperFlask>().SetPriceIndicator();
        }

        private void FocusEventOnGameObject(GameObject gaoToFocus)
        {
            if (gaoToFocus)
            {
                EventSystem.current.SetSelectedGameObject(gaoToFocus);
                gaoToFocus.GetComponent<EquiperFlask>().OnSelect(null);
            }
        }

        public void LinkButtonsEventWithFlaskSelector()
        {
            UnlinkButtonsEventWithFlaskSelector();

            LinkButtonEventWithFlaskSelector(m_downButtonGameObject);
            LinkButtonEventWithFlaskSelector(m_middleButtonGameObject);
            LinkButtonEventWithFlaskSelector(m_upperButtonGameObject);
        }

        private void LinkButtonEventWithFlaskSelector(GameObject[] list)
        {
            foreach (GameObject gao in list)
            {
                EquiperFlask equiperFlask = gao.GetComponent<EquiperFlask>();
                if (equiperFlask.m_flaskData.m_isLock)
                    continue;
                if (!equiperFlask.m_isBuy)
                {
                    equiperFlask.m_onClick.AddListener(m_descriptorPanelManager.EnableBuyValidatorPanel);
                }
                else
                {
                    equiperFlask.m_onClick.AddListener(EquipFlask);
                }
            }
        }

        public void EquipFlask(EquiperFlask equiperFlask)
        {
            DisableScrolling();
            m_currentFlaskEmplacement.Equip(equiperFlask);
            m_playerPanelController.SetFocusOnFlask();
        }

        private void UnlinkButtonsEventWithFlaskSelector()
        {
            UnlinkButtonEventWithFlaskSelector(m_downButtonGameObject);
            UnlinkButtonEventWithFlaskSelector(m_middleButtonGameObject);
            UnlinkButtonEventWithFlaskSelector(m_upperButtonGameObject);
        }

        private void UnlinkButtonEventWithFlaskSelector(GameObject[] list)
        { 
            foreach (GameObject gao in list)
            {
                EquiperFlask equiper = gao.GetComponent<EquiperFlask>();
                equiper.m_onClick.RemoveAllListeners();
            }
        }

        public void SetToolbarDataOnShop()
        {
            HelperUIManager helperManager = HelperUIManager.Instance;
            helperManager.DisableAllHelper();
            helperManager.ClearAllSpecificControllerActions(m_playerPanelController.CurrentGamePad);

            helperManager.EnableHelper(helperManager.m_helper1GameObject);
            helperManager.SetHelperInformation(helperManager.m_helper1GameObject, helperManager.m_spriteIconA, "Buy");
            helperManager.EnableHelper(helperManager.m_helper2GameObject);
            if(m_playerPanelController.CurrentGamePad == GamePadID.Controller1)
            {
                helperManager.m_controller1Helper2Action += m_playerPanelController.SetFocusOnFlask;
                helperManager.m_controller1Helper2Action += DisableScrolling;
                helperManager.SetHelperInformation(helperManager.m_helper2GameObject, helperManager.m_spriteIconB, "Cancel", helperManager.m_controller1Helper2Action, GamePadID.Controller1);
            }
            else if (m_playerPanelController.CurrentGamePad == GamePadID.Controller2)
            {
                helperManager.m_controller2Helper2Action += m_playerPanelController.SetFocusOnFlask;
                helperManager.m_controller2Helper2Action += DisableScrolling;
                helperManager.SetHelperInformation(helperManager.m_helper2GameObject, helperManager.m_spriteIconB, "Cancel", helperManager.m_controller2Helper2Action, GamePadID.Controller2);
            }


            EnableScrolling();
        }

        private void EnableScrolling()
        {
            MoveScrollRect scroller = GetComponent<MoveScrollRect>();
            scroller.m_enableScrolling = true;
            scroller.m_OnScroll.AddListener(OnNewScrollPage);
        }

        public void DisableScrolling()
        {
            MoveScrollRect scroller = GetComponent<MoveScrollRect>();
            scroller.m_enableScrolling = false;
            scroller.m_OnScroll.RemoveAllListeners();
        }
    }
}