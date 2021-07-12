using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace BTA
{
    public class PlayerUI : MonoBehaviour
    {
        private PlayerManager m_playerMgr;
        private Player m_player1;
        private Player m_player2;

        [SerializeField] private GameObject m_soloPanel;
        [SerializeField] private PlayerUIContainer m_mainUI;
        [SerializeField] private PlayerUIContainer m_secondUI;

        [SerializeField] private GameObject m_duoPanel;
        [SerializeField] private PlayerUIContainer m_leftUI;
        [SerializeField] private PlayerUIContainer m_rightUI;

        void Start()
        {
            GameManager gm = GameManager.Instance;
            gm.OnSoloMode.AddListener(OnSoloMode);
            gm.OnDuoMode.AddListener(OnDuoMode);

            m_playerMgr = LevelManager.Instance.GetInstanceOf<PlayerManager>();
            m_playerMgr.OnSwitchPlayer.AddListener(OnPlayerSwitch);

            if (gm.GetGameMode() == GameMode.Solo)
                OnSoloMode();
            else if (gm.GetGameMode() == GameMode.Duo)
                OnDuoMode();
        }

        private void OnSoloMode()
        {
            m_soloPanel.SetActive(true);
            m_duoPanel.SetActive(false);
            m_player1 = m_playerMgr.GetPlayer(PlayerManager.PlayerInstance.PLAYER1);
            m_player2 = null;

            LinkPlayerToUI(m_player1, m_mainUI);
        }

        private void OnDuoMode()
        {
            m_soloPanel.SetActive(false);
            m_duoPanel.SetActive(true);

            m_player1 = m_playerMgr.GetPlayer(PlayerManager.PlayerInstance.PLAYER1);
            m_player2 = m_playerMgr.GetPlayer(PlayerManager.PlayerInstance.PLAYER2);

            LinkPlayerToUI(m_player1, m_leftUI);
            LinkPlayerToUI(m_player2, m_rightUI);
        }

        private void OnPlayerSwitch(GameObject playerGao)
        {
            //Update Second
            LinkPlayerToHealth(m_player1, m_secondUI);
            LinkPlayerToChainSystem(m_player1, m_secondUI);

            //Switch
            m_player1 = playerGao.GetComponent<Player>();

            //Update Main
            LinkPlayerToHealth(m_player1, m_mainUI);
            LinkPlayerToChainSystem(m_player1, m_mainUI);
            LinkPlayerToSlimeBall(m_player1, m_mainUI);
        }

        //TODO : Switch gao UICanvas to Script to Ensure that Health/Chain/Flask are Here
        private void LinkPlayerToUI(Player player, PlayerUIContainer UICanvas)
        {
            LinkPlayerToHealth(player, UICanvas);
            LinkPlayerToChainSystem(player, UICanvas);
            LinkPlayerToSlimeBall(player, UICanvas);
        }

        private void LinkPlayerToHealth(Player player, PlayerUIContainer UICanvas)
        {
            HealthBar healthBar = UICanvas.HealthBar;

            if (!healthBar)
            {
                Debug.LogError("GameObject has no HealthBar ...");
                return;
            }

            Damageable damageable = player.GetComponentInChildren<Damageable>();

            damageable.OnTakeDamage.RemoveAllListeners();
            damageable.OnHeal.RemoveAllListeners();
            player.OnRecoveryLifeUpdate.RemoveAllListeners();
            player.OnCorruptedLifeUpdate.RemoveAllListeners();

            damageable.OnTakeDamage.AddListener(healthBar.OnLifeBarChange);
            damageable.OnHeal.AddListener(healthBar.OnGainLife);
            player.OnRecoveryLifeUpdate.AddListener(healthBar.OnRecoveryBarUpdate);
            player.OnCorruptedLifeUpdate.AddListener(healthBar.OnCorruptedBarUpdate);

            healthBar.UpdateFromPlayer(player);
        }

        private void LinkPlayerToChainSystem(Player player, PlayerUIContainer UICanvas)
        {
            ChainHandler chainHandler = UICanvas.ChainHandler;
            ChainUI chainUI = UICanvas.ChainUI;

            PlayerFighting fighting = player.GetComponent<PlayerFighting>();

            if (chainHandler)
            {
                fighting.OnChainGaugeUpdateEvent.RemoveAllListeners();
                fighting.OnChainGaugeUpdateEvent.AddListener(chainHandler.OnChainGaugeUIUpdate);
                fighting.UpdateChainGauge();
            }

            if (chainUI)
            {
                fighting.OnChainUpdateEvent.RemoveAllListeners();
                fighting.OnChainUpdateEvent.AddListener(chainUI.OnChainUpdate);
                fighting.UpdateChain();
            }
        }

        private void LinkPlayerToSlimeBall(Player player, PlayerUIContainer UICanvas)
        {
            SlimeBallUI slimeBallUI = UICanvas.SlimeBallUI;

            if (slimeBallUI)
            {
                player.OnSlimeBallUpdate.RemoveAllListeners();
                player.OnSlimeBallUpdate.AddListener(slimeBallUI.OnSlimeBallUpdate);
                player.UpdateSlimeBall();
            }
        }

    }
}