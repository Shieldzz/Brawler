using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace BTA
{

    public class PlayerInfoController : MonoBehaviour
    {
        Player m_player;

        [SerializeField] private Image m_amaraSprite;
        [SerializeField] private Image m_judeSprite;

        [SerializeField] private Text m_playerIDText;
        [SerializeField] private Text m_slimeCountText;

        [SerializeField] private Text m_speakingBlockText;

        public void Init(Player player)
        {
            m_player = player;
        }

        public void ShowPlayerSprite()
        {
            if (m_player.GetComponent<PlayerCacFighting>())
            {
                m_amaraSprite.enabled = true;
                m_judeSprite.enabled = false;
            }
            else
            {
                m_amaraSprite.enabled = false;
                m_judeSprite.enabled = true;
            }
        }

        public void ShowPlayerID()
        {
            m_playerIDText.enabled = true;

            GamePadID playerID = m_player.GetGamePadID();
            if (playerID == GamePadID.Controller1)
                m_playerIDText.text = "P1";
            else if (playerID == GamePadID.Controller2)
                m_playerIDText.text = "P2";
        }

        public void ShowSlimeCount(float duration)
        {
            m_slimeCountText.enabled = true;
            StartCoroutine(SmoothTranslation(m_slimeCountText, 0, (int)m_player.SlimBallCount, duration));
        }

        public void ActiveNarration(TextBlock speach)
        {
            if (!m_speakingBlockText)
                return;

            m_speakingBlockText.transform.parent.gameObject.SetActive(true);
            m_speakingBlockText.text = speach.text;

            if (speach.hasSound)
                FMODUnity.RuntimeManager.PlayOneShot(speach.sound);
        }

        IEnumerator SmoothTranslation(Text translatingText, int start, int end, float duration)
        {
            float t = 0f;

            while (t <= 1)
            {
                translatingText.text = Mathf.Lerp(start, end, t).ToString("0");

                t += Time.deltaTime / duration;

                yield return null;
            }

            translatingText.text = end.ToString();
        }
    }
}