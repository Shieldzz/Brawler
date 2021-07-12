using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace BTA
{
    public class SlimeBallUI : MonoBehaviour
    {
        private Text m_text;

        public void Start()
        {
            m_text = GetComponentInChildren<Text>();
        }

        public void OnSlimeBallUpdate(float slimeBallCount)
        {
            if (m_text)
                m_text.text = slimeBallCount.ToString();
        }
    }
}
