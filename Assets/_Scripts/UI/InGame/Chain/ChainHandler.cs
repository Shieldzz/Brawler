using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

namespace BTA
{

    public class ChainHandler : MonoBehaviour
    {
        [SerializeField] private float m_minCircleFilling = 0.02f;
        [SerializeField] private List<Image> m_chainGauges = new List<Image>();
        private int m_currentGaugeLevel = 0;

        // Use this for initialization
        void Start()
        {
            m_chainGauges[m_currentGaugeLevel].gameObject.SetActive(true);
            m_chainGauges[m_currentGaugeLevel].fillAmount = m_minCircleFilling;
        }

        public void OnChainGaugeUIUpdate(int newGaugelevel, int newGaugeValue, int maxGaugeValue)
        {
            // Show the new Gauge
            if (m_currentGaugeLevel != newGaugelevel)
            {
                m_chainGauges[m_currentGaugeLevel].gameObject.SetActive(false);
                m_currentGaugeLevel = newGaugelevel;
                m_chainGauges[newGaugelevel].gameObject.SetActive(true);
            }

            //Assign Ratio
            float gaugeRatio = (float)newGaugeValue / (float)maxGaugeValue;

            if (gaugeRatio < m_minCircleFilling)
                gaugeRatio = m_minCircleFilling;

            m_chainGauges[m_currentGaugeLevel].fillAmount = gaugeRatio;
        }
    }
}
