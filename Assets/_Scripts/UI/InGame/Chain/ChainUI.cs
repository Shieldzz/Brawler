using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChainUI : MonoBehaviour
{
    [SerializeField] private Text m_text;
    [SerializeField] private Image m_chainBar;
    [SerializeField] private Text m_timerText;

    private float m_chainTimer;
    private float m_maxChainTimer;

    private void Update()
    {
        if (m_chainTimer > 0f)
        {
            m_chainTimer -= Time.deltaTime;
            m_timerText.text = m_chainTimer.ToString("#.00");
            float ratio = m_chainTimer / m_maxChainTimer;

            if (ratio <= 0f)
            {
                gameObject.SetActive(false);
                return;
            }

            m_chainBar.fillAmount = ratio;
        }
    }

    public void OnChainUpdate(int Chain, float MaxTime)
    {
        if (Chain == 0)
            gameObject.SetActive(false);

        else if (MaxTime > 0f)
            gameObject.SetActive(true);


        m_text.text = Chain.ToString();
        m_maxChainTimer = MaxTime;
        m_chainTimer = MaxTime;
        m_timerText.text = m_chainTimer.ToString("#.00");
    }
}
