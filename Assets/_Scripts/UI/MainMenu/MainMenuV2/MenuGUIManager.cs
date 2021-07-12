using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BTA
{

    public class MenuGUIManager : MonoBehaviour
    {

        static private MenuGUIManager m_instance = null;
        static public MenuGUIManager Instance
        {
            get
            {
                if (!m_instance)
                    m_instance = new MenuGUIManager();
                return m_instance;
            }
        }

        private void Awake()
        {
            if (!m_instance)
                m_instance = this;
        }

        [SerializeField]
        private GameObject m_mainPanel = null;
        [SerializeField]
        private GameObject m_essencePanel = null;
        [SerializeField]
        private GameObject m_helperPanel = null;
        [SerializeField]
        private GameObject m_mapPanel = null;
        [SerializeField]
        private GameObject m_optionsPanel = null;
        [SerializeField]
        private GameObject m_creditPanel = null;

        void Start()
        {
        }

        private void OnEnable()
        {
            EnableMainPanel();
        }

        public void EnableMainPanel()
        {
            m_creditPanel.SetActive(false);
            m_essencePanel.SetActive(false);
            m_mapPanel.SetActive(false);
            m_optionsPanel.SetActive(false);
            m_mainPanel.SetActive(true);
        }

        public void EnableEssencePanel()
        {
            m_creditPanel.SetActive(false);
            m_mainPanel.SetActive(false);
            m_mapPanel.SetActive(false);
            m_optionsPanel.SetActive(false);
            m_essencePanel.SetActive(true);
        }

        public void EnableMapPanel()
        {
            m_creditPanel.SetActive(false);
            m_mainPanel.SetActive(false);
            m_essencePanel.SetActive(false);
            m_optionsPanel.SetActive(false);
            m_mapPanel.SetActive(true); 
        }

        public void  EnableOptionsPanel()
        {
            m_creditPanel.SetActive(false);
            m_mainPanel.SetActive(false);
            m_essencePanel.SetActive(false);
            m_mapPanel.SetActive(false);
            m_optionsPanel.SetActive(true);
        }

        public void EnableCredit()
        {
            m_mainPanel.SetActive(false);
            m_essencePanel.SetActive(false);
            m_mapPanel.SetActive(false);
            m_optionsPanel.SetActive(false);
            m_creditPanel.SetActive(true);
        }

        public void Quit()
        {
            Application.Quit();
        }

    }
}