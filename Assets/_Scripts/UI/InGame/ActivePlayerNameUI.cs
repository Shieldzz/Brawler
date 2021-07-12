using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


namespace BTA
{

    public class ActivePlayerNameUI : MonoBehaviour
    {
        PlayerManager m_playerMgr;

        Text m_playerName;

        string m_cacName = "Amara";
        string m_distName = "Jude";

        // Use this for initialization
        void Start()
        {
            m_playerName = GetComponent<Text>();

            m_playerMgr = LevelManager.Instance.GetInstanceOf<PlayerManager>();
            m_playerMgr.OnSwitchPlayer.AddListener(UpdateSprite);

            if (m_playerMgr.GetPlayer(PlayerManager.PlayerInstance.PLAYER1).isActiveAndEnabled)
                m_playerName.text = m_cacName;
            else
                m_playerName.text = m_distName;
        }

        private void UpdateSprite(GameObject playerGao)
        {
            if (playerGao.GetComponent<PlayerCacFighting>())
                m_playerName.text = m_cacName;
            else
                m_playerName.text = m_distName;
        }
    }
}

