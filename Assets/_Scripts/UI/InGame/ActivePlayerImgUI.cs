using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


namespace BTA
{

    public class ActivePlayerImgUI : MonoBehaviour
    {
        PlayerManager m_playerMgr;

        [SerializeField] GameObject m_cacSpriteHead;
        [SerializeField] GameObject m_distSpriteHead;

        // Use this for initialization
        void Start()
        {
            m_playerMgr = LevelManager.Instance.GetInstanceOf<PlayerManager>();
            m_playerMgr.OnSwitchPlayer.AddListener(UpdateSprite);

            if (m_playerMgr.GetPlayer(PlayerManager.PlayerInstance.PLAYER1).isActiveAndEnabled)
            {
                m_cacSpriteHead.SetActive(true);
                m_distSpriteHead.SetActive(false);
            }
            else
            {
                m_distSpriteHead.SetActive(true);
                m_cacSpriteHead.SetActive(false);
            }
        }

        private void UpdateSprite(GameObject playerGao)
        {
            if (playerGao.GetComponent<PlayerCacFighting>())
            {
                m_cacSpriteHead.SetActive(true);
                m_distSpriteHead.SetActive(false);
            }
            else
            {
                m_distSpriteHead.SetActive(true);
                m_cacSpriteHead.SetActive(false);
            }
        }
    }
}
