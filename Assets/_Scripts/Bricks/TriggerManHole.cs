using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BTA
{
    public class TriggerManHole : MonoBehaviour {

        private List<ManHole> m_manHoleList = new List<ManHole>();

        PlayerManager m_playerMgr;
        GameObject m_player1;
        GameObject m_player2;

        bool m_isActive = false;

        bool m_isInPlayer1 = false;
        bool m_isInPlayer2 = false;

        // Use this for initialization
        void Start() {
            m_playerMgr = LevelManager.Instance.GetInstanceOf<PlayerManager>();
            m_player1 = m_playerMgr.GetPlayer(PlayerManager.PlayerInstance.PLAYER1).gameObject;
            m_player2 = m_playerMgr.GetPlayer(PlayerManager.PlayerInstance.PLAYER2).gameObject;

            SetListManHole();
        }

        // Update is called once per frame
        void Update() {
            if (m_player1.gameObject.activeSelf && m_isInPlayer1)
                m_isInPlayer1 = false;
            if (m_player2.gameObject.activeSelf && m_isInPlayer2)
                m_isInPlayer2 = false;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.name == m_player1.name || other.name == m_player2.name && !m_isActive)
            {
                if (other.name == m_player1.name)
                    m_isInPlayer1 = true;
                if (other.name == m_player2.name)
                    m_isInPlayer2 = true;

                m_isActive = true;
                SetManHoleActiveState();
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.name == m_player1.name || other.name == m_player2.name)
            {
                if (other.name == m_player1.name)
                    m_isInPlayer1 = false;
                if (other.name == m_player2.name)
                    m_isInPlayer2 = false;

                if (m_isInPlayer1 == false && m_isInPlayer2 == false)
                {
                    m_isActive = false;
                    SetManHoleActiveState();
                }
            }
        }

        void SetManHoleActiveState()
        {
            for (int i = 0; i < m_manHoleList.Count; i++)
            {
                m_manHoleList[i].IsActive = m_isActive;
                m_manHoleList[i].SwitchToNextState();
            }
        }

        void SetListManHole()
        {
            foreach (Transform child in transform)
            {
                ManHole manHole = child.GetComponent<ManHole>();
                if (manHole != null)
                {
                    m_manHoleList.Add(manHole);
                }
            }
        }

    }
}