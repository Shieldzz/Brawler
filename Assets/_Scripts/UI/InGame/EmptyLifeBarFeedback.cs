using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EmptyLifeBarFeedback : MonoBehaviour
{
    [SerializeField] float m_blinkDuration = 1.5f;

    [Header("Blinking Image")]
    [SerializeField] Image m_img;
    [SerializeField] Color m_imgBaseColor;
    [SerializeField] Color m_imgBlinkColor;

    [Header("Blinking Image")]
    [SerializeField] Text m_txt;
    [SerializeField] Color m_txtBaseColor;
    [SerializeField] Color m_txtBlinkColor;

    bool lifeEmpty = false;

    public void OnEmptyLife()
    {
        if (lifeEmpty)
            return;

        lifeEmpty = true;
        StartCoroutine(Blink());
    }

    public void OnRecoveredLife()
    {
        lifeEmpty = false;
    }

    IEnumerator Blink()
    {
        float t = 0f;
        bool raising = true;

        while (true)
        {
            m_img.color = Color.Lerp(m_imgBaseColor, m_imgBlinkColor, t);
            m_txt.color = Color.Lerp(m_txtBaseColor, m_txtBlinkColor, t);

            if (raising)
            {
                t += Time.deltaTime / m_blinkDuration;
                if (t >= 1f)
                    raising = false;
            }
            else
            {
                t -= Time.deltaTime / m_blinkDuration;
                if (t <= 0f)
                    raising = true;
            }

            if (!lifeEmpty && t <= 0f)
                break;

            yield return null;
        }
        m_img.color = m_imgBaseColor;
        m_txt.color = m_txtBaseColor;

        yield return new WaitForEndOfFrame();
    }

}
