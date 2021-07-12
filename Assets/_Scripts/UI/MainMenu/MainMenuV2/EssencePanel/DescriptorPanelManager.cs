using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BTA
{

    public class DescriptorPanelManager : MonoBehaviour
    {
        [SerializeField] private PlayerPanelController m_playerPanelController = null;
        public PlayerPanelController PlayerPanelController { get { return m_playerPanelController; } }
        [SerializeField] private FlaskDescriptorPanel m_flaskDescriptorPanel = null;
        [SerializeField] private InformaterPanelController m_informaterPanelControle = null;
        [SerializeField] private ValidatorPanel m_validatorPanel = null;
        [SerializeField] private WarningPanelController m_warningPanel = null;
        [SerializeField] private NarrativePanel m_narrativePanel = null;

        private bool m_toggleDetails = false;

        private FlaskData m_currentEquipedFlask = null;

        void Start()
        {

        }

        void Update()
        {

        }

        private float GetNocivity()
        {
            FlaskData[] flasks = m_playerPanelController.GetEquipedFlask();
            float total = 0f;
            foreach(FlaskData flask in flasks)
            {
                if(flask)
                    total += flask.nocivity;
            }

            return total;
        }

        public void DescribeFlask(FlaskData data)
        {
            m_currentEquipedFlask = data;
            m_informaterPanelControle.gameObject.SetActive(false);
            m_flaskDescriptorPanel.gameObject.SetActive(true);
            DisableNarrativePanel();

            float nocivity = GetNocivity();
            m_flaskDescriptorPanel.DescribeFlask(data);
            //m_warningPanel.SetWarningText("Toxicity", "C-9 toxicity: " + nocivity + "%");
        }

        public void InspectCurrentFlask(FlaskData data)
        {
            m_flaskDescriptorPanel.gameObject.SetActive(false);
            DisableNarrativePanel();
            m_informaterPanelControle.gameObject.SetActive(true);

            m_informaterPanelControle.InspectEquipedFlask(data);
        }

        public void InspectSelectedFlask(FlaskData data)
        {
            //InspectCurrentFlask(m_currentEquipedFlask);
            m_informaterPanelControle.gameObject.SetActive(true);
            m_informaterPanelControle.InspectSelectedFlask(data);
            m_informaterPanelControle.InspectEquipedFlask(m_currentEquipedFlask);
        }

        public void EnableBuyValidatorPanel(EquiperFlask equiper)
        {
            if (!equiper)
                return;

            if (!equiper.m_flaskData)
                return;

            m_validatorPanel.gameObject.SetActive(true);
            m_validatorPanel.DisplayFlaskData(equiper);
        }

        public void ExplainFlask(FlaskData flaskData)
        {
            m_narrativePanel.ExplainFlask(flaskData);
        }

        public void DisableBuyValidatorPanel()
        {
            m_validatorPanel.Close();
            m_validatorPanel.gameObject.SetActive(false);
            m_playerPanelController.SetFocusOnShop();
            m_playerPanelController.ShopPanelManager.LinkButtonsEventWithFlaskSelector();
        }

        public void DisableNarrativePanel()
        {
            m_toggleDetails = false;
            m_narrativePanel.gameObject.SetActive(false);
        }

        public void EnableDisableNarrativePanelFromEquiper()
        {
            m_toggleDetails = !m_toggleDetails;

            m_narrativePanel.gameObject.SetActive(m_toggleDetails);
            m_informaterPanelControle.gameObject.SetActive(!m_toggleDetails);
        }

        public void EnableDisableNarrativePanelFromEquiped()
        {
            m_toggleDetails = !m_toggleDetails;

            m_narrativePanel.gameObject.SetActive(m_toggleDetails);
            m_flaskDescriptorPanel.gameObject.SetActive(!m_toggleDetails);
        }
    }
}