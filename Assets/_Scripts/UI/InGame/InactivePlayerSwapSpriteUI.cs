using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace BTA
{

    public class InactivePlayerSwapSpriteUI : MonoBehaviour
    {
        PlayerManager m_playerMgr;

        Image m_spriteRenderer;

        [SerializeField] Sprite m_inactiveCacSprite;
        [SerializeField] Sprite m_inactiveDistSprite;

        // Use this for initialization
        void Start()
        {
            m_spriteRenderer = GetComponent<Image>();

            m_playerMgr = LevelManager.Instance.GetInstanceOf<PlayerManager>();
            m_playerMgr.OnSwitchPlayer.AddListener(UpdateSprite);

            if (m_playerMgr.GetPlayer(PlayerManager.PlayerInstance.PLAYER1).isActiveAndEnabled)
                m_spriteRenderer.sprite = m_inactiveDistSprite;
            else
                m_spriteRenderer.sprite = m_inactiveCacSprite;
        }

        private void UpdateSprite(GameObject playerGao)
        {
            if (playerGao.GetComponent<PlayerCacFighting>())
                m_spriteRenderer.sprite = m_inactiveDistSprite;
            else
                m_spriteRenderer.sprite = m_inactiveCacSprite;
        }
    }
}
