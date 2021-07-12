using System;
using UnityEngine;
using Cinemachine;
using UnityEngine.Events;

namespace BTA
{

    public class PlayerManager : MonoBehaviour
    {
        GameManager m_gameMgr;
        GameplayManager m_gameplayMgr;

        public enum PlayerInstance
        {
            PLAYER1,
            PLAYER2
        }

        [SerializeField] private Player m_playerCac;
        [SerializeField] private Player m_playerDist;

        public class SwitchPlayerEvent : UnityEvent<GameObject> { }
        public SwitchPlayerEvent OnSwitchPlayer = new SwitchPlayerEvent();

        bool m_paused = false;
        public bool IsPaused { get { return m_paused; } }

        // Use this for initialization
        void Start()
        {
            m_gameMgr = GameManager.Instance;
            m_gameplayMgr = m_gameMgr.GetInstanceOf<GameplayManager>();
            //m_gameMgr.OnSoloMode.AddListener(SwitchToSoloMode);
            //m_gameMgr.OnDuoMode.AddListener(SwitchToDuoMode);

            m_playerCac.AssignGamePad(m_gameplayMgr.m_cacPlayerGamepad);
            m_playerDist.AssignGamePad(m_gameplayMgr.m_distPlayerGamepad);

            if (m_gameMgr.GetGameMode() == GameMode.Solo)
                m_playerDist.gameObject.SetActive(false);
        }

        public Player GetPlayer(PlayerInstance player)
        {
            if (player == PlayerInstance.PLAYER1)
                return m_playerCac;
            else
                return m_playerDist;
        }

        private void AssignPlayerGamePad()
        {
            m_playerCac.AssignGamePad(m_gameplayMgr.m_cacPlayerGamepad);
            m_playerDist.AssignGamePad(m_gameplayMgr.m_distPlayerGamepad);
        }

        public void Update()
        {
            //Debug.Log("Update = " + m_playerCac);
        }

        public void Pause(bool active)
        {
            m_paused = active;

            //Debug.Log("Pause = " + m_playerCac);
            if (m_playerCac.isActiveAndEnabled)
                m_playerCac.Pause(active);
            if (m_playerDist.isActiveAndEnabled)
                m_playerDist.Pause(active);
        }

        //private void SwitchToSoloMode()
        //{
        //    GamePadID id = m_gameMgr.GetInstanceOf<InputManager>().ConnectedGamePad;

        //    if (m_playerCac.GetGamePadID() == id)
        //        m_playerDist.gameObject.SetActive(false);
        //    else if (m_playerDist.GetGamePadID() == id)
        //        m_playerCac.gameObject.SetActive(false);

        //}

        //private void SwitchToDuoMode()
        //{
        //    if (m_playerCac.gameObject.activeSelf)
        //        EnablePlayerAtLocation(m_playerDist.gameObject, m_playerCac.transform);
        //    if (m_playerDist.gameObject.activeSelf)
        //        EnablePlayerAtLocation(m_playerCac.gameObject, m_playerDist.transform);
        //}


        public void SwitchPlayer()
        {
            if (m_gameMgr.GetGameMode() == GameMode.Solo)
                SoloSwitch();
            //else
            //    DuoSwitch();
        }

        private void SoloSwitch()
        {
            if (m_playerCac.gameObject.activeSelf)
            {
                //Debug.Log("switch alive ? " + m_playerDist.GetDameageable().IsAlive());
                if (!m_playerDist.GetDameageable().IsAlive())
                    return;

                //Debug.Log("PLop Switch");

                m_playerCac.gameObject.SetActive(false);
                EnablePlayerAtLocation(m_playerDist.gameObject, m_playerCac.transform);
                OnSwitchPlayer.Invoke(m_playerDist.gameObject);

            }
            else if (m_playerDist.gameObject.activeSelf)
            {
                //Debug.Log("boulou");
                if (!m_playerCac.GetDameageable().IsAlive())
                    return;
                m_playerDist.gameObject.SetActive(false);
                EnablePlayerAtLocation(m_playerCac.gameObject, m_playerDist.transform);
                OnSwitchPlayer.Invoke(m_playerCac.gameObject);
            }
        }

        //private void DuoSwitch()
        //{
        //    //TODO : Implement switch with 2 player
        //}

        void EnablePlayerAtLocation(GameObject playerToEnable, Transform location)
        {
            playerToEnable.transform.position = location.position;
            playerToEnable.transform.rotation = location.rotation;
            playerToEnable.transform.localScale = location.localScale;
            playerToEnable.SetActive(true);
        }

        public bool ArePlayersAlive()
        {
            if (!m_playerCac.GetDameageable().IsAlive())
                return false;
            else if (!m_playerDist.GetDameageable().IsAlive())
                return false;

            return true;
        }

        public bool ArePlayersDead()
        {
            if (m_playerCac.GetDameageable().IsAlive())
                return false;
            else if (m_playerDist.GetDameageable().IsAlive())
                return false;
            return true;
        }

    }

} // namespace BTA

