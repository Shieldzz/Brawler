using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace BTA
{
    public class TimeUI : MonoBehaviour
    {
        Text m_text;

        float m_timer = 0f;

        // Use this for initialization
        void Awake()
        {
            m_text = GetComponent<Text>();
            if (m_timer <= 0f)
                gameObject.SetActive(false);
        }

        public void Activate(float time)
        {
            m_timer = time;
            gameObject.SetActive(true);
            UpdateUI();
        }

        // Update is called once per frame
        void Update()
        {
            if (m_timer > 0f)
            {
                m_timer -= Time.deltaTime;
                UpdateUI();
                if (m_timer <= 0f)
                    gameObject.SetActive(false); //play sound / flash effect & else
            }

        }

        private void UpdateUI()
        {
            m_text.text = m_timer.ToString("#.00");
        }

    }
}
