using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace BTA
{

    public class BattleZoneEndFeedback : MonoBehaviour
    {
        [SerializeField] float m_feedbackDuration = 3f;
        [SerializeField] float m_blinkDuration = 0.5f;

        [Header("Blinking Image")]
        [SerializeField] Image m_img;
        [SerializeField] Color m_imgBaseColor;
        [SerializeField] Color m_imgBlinkColor;

        [Header("Blinking Image")]
        [SerializeField] Text m_txt;
        [SerializeField] Color m_txtBaseColor;
        [SerializeField] Color m_txtBlinkColor;

        public void StartBlink()
        {
            StartCoroutine(Blink());
        }

        IEnumerator Blink()
        {
            float t = 0f;
            float feedbackTimer = 0f;
            bool raising = true;

            m_txt.color = m_txtBlinkColor;

            while (true)
            {
                m_img.color = Color.Lerp(m_imgBaseColor, m_imgBlinkColor, t);
                //m_txt.color = Color.Lerp(m_txtBaseColor, m_txtBlinkColor, t);

                if (feedbackTimer >= m_feedbackDuration && t <= 0f)
                    break;

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

                feedbackTimer += Time.deltaTime;

                yield return null;
            }

            m_txt.color = m_txtBaseColor;
            m_img.color = m_imgBaseColor;

            yield return new WaitForEndOfFrame();
        }
    }
}
