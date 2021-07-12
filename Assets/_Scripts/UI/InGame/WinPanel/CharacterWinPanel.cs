using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace BTA
{

    public class CharacterWinPanel : MonoBehaviour
    {

        [SerializeField] private PlayerInfoController m_winPanel;
        [SerializeField] private PlayerInfoController m_losePanel;

        public PlayerInfoController WinPanel { get { return m_winPanel; } }
        public PlayerInfoController LosePanel { get { return m_losePanel; } }

        public CharacterEnum Init()
        {
            PlayerManager playerManager = LevelManager.Instance.GetInstanceOf<PlayerManager>();
            Player p1 = playerManager.GetPlayer(PlayerManager.PlayerInstance.PLAYER1);
            Player p2 = playerManager.GetPlayer(PlayerManager.PlayerInstance.PLAYER2);

            if (p1.SlimBallCount >= p2.SlimBallCount)
            {
                m_winPanel.Init(p1);
                m_losePanel.Init(p2);
                return CharacterEnum.MELEE;
            }
            else
            {
                m_winPanel.Init(p2);
                m_losePanel.Init(p1);
                return CharacterEnum.DISTANCE;
            }
        }
    }
}